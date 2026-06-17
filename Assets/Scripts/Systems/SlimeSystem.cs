using DBC.Common;
using ECS;
using Svelto.DataStructures.Experimental;
using Svelto.ECS;
using UnityEngine;

public class SlimeSystem : ISystem, IQueryingEntitiesEngine, IReactOnAddEx<GameObjectReference>
{
    public EntitiesDB entitiesDB { get; set; }

    private readonly GameObjectResourceManager gameObjectResourceManager;
    private readonly RectTransformResourceManager rectTransformResourceManager;

    public SlimeSystem(
        GameObjectResourceManager gameObjectResourceManager,
        RectTransformResourceManager rectTransformResourceManager)
    {
        this.gameObjectResourceManager = gameObjectResourceManager;
        this.rectTransformResourceManager = rectTransformResourceManager;
    }

    public void Ready()
    {
    }

    public void Update()
    {
        entitiesDB.QueryEntities<GameObjectReference,  SlimeBrain>(SlimeGroup.Groups)
            .Each((ref GameObjectReference gor, ref SlimeBrain slimeBrain) =>
            {
                // Make slime bigger when grabbed
                var go = gameObjectResourceManager[gor.Id];
                if (slimeBrain.MovementState == MovementState.Grabbed)
                {
                    go.transform.localScale = 1.25f * Vector3.one;
                }
                else
                {
                    go.transform.localScale = Vector3.one;
                }

                // Keep slime upright when not flying
                if (slimeBrain.MovementState != MovementState.Flying)
                {
                    go.transform.localRotation = default;
                }
            });
    }

    public void Add((uint start, uint end) rangeOfEntities, in EntityCollection<GameObjectReference> entities, ExclusiveGroupStruct groupID)
    {
        if (!groupID.FoundIn(SlimeGroup.Groups)) { return; }

        var (gors, entityIds, _) = entities;
        for (var i = rangeOfEntities.start; i < rangeOfEntities.end; i++)
        {
            var id = entityIds[i];
            ref var gor = ref gors[i];

            GameObject go;
            RectTransform rt;
            ValueIndex rtId;

            try
            {
                go = gameObjectResourceManager[gor.Id];
                rt = go.GetComponent<RectTransform>();
                rtId = GameContext.RectTransformResourceManager.Add(rt);
            }
            catch (PreconditionException)
            {
                // Create GameObject
                if (!entitiesDB.TryGetSingletonComponent(SlimeSpawnerGroup.Group, out SlimeSpawner slimeSpawner))
                {
                    return;
                }
                if (!entitiesDB.TryGetSingletonComponent(CanvasGroup.Group, out GameCanvas gameCanvas))
                {
                    return;
                }

                var slime = entitiesDB.QueryEntity<Slime>(id, groupID);
                GameObject prefab = slime.SlimeColor switch
                {
                    SlimeColor.Red => gameObjectResourceManager[slimeSpawner.SlimeRedPrefabId],
                    SlimeColor.Blue => gameObjectResourceManager[slimeSpawner.SlimeBluePrefabId],
                    _ => gameObjectResourceManager[slimeSpawner.SlimePrefabId],
                };
                var parent = gameObjectResourceManager[gameCanvas.SlimesParentGoId];

                go = Object.Instantiate(prefab, parent.transform);
                go.transform.SetAsFirstSibling();
                gor.Id = gameObjectResourceManager.Add(go);

                rt = go.GetComponent<RectTransform>();
                rtId = rectTransformResourceManager.Add(rt);
            }

            entitiesDB.TryGetComponent(id, groupID,
                (ref RectTransformReference rtr) =>
                {
                    rtr.Id = rtId;
                });
            entitiesDB.TryGetComponent(id, groupID,
                (ref RectBoundary rb) =>
                {
                    rb.Width = rt.sizeDelta.x;
                    rb.Height = rt.sizeDelta.y;
                });

            // Randomize position in pen
            entitiesDB.TryGetComponent(id, groupID,
                (ref SlimeBrain sb, ref RectPosition rp, ref RectBoundary rb) =>
                {
                    if (!sb.RandomizePositionInPen) { return; }

                    Vector2 slimeExtents = 0.5f * new Vector2(rt.sizeDelta.x, rt.sizeDelta.y);
                    Vector2 position = default;
                    entitiesDB.TryGetComponent(sb.PenId, PenGroupTag.Groups,
                        (ref RectPosition rp, ref RectBoundary rb) =>
                        {
                            var penExtents = 0.5f * new Vector2(rb.Width, rb.Height);
                            var xMin = rp.Value.x - penExtents.x + slimeExtents.x;
                            var xMax = rp.Value.x + penExtents.x - slimeExtents.x;
                            var yMin = rp.Value.y - penExtents.y + slimeExtents.y;
                            var yMax = rp.Value.y + penExtents.y - slimeExtents.y;
                            position = new(
                                Random.Range(xMin, xMax),
                                Random.Range(yMin, yMax)
                            );
                        });
                    rp.Value = position;
                });

            go.GetComponent<EntityReferenceHolder>().EGID = new(entityIds[i], groupID);
        }
    }
}