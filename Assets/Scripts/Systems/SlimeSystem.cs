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
        var count = 0;
        entitiesDB.QueryEntities<GameObjectReference>(SlimeGroup.Groups)
            .Each((ref GameObjectReference gor) =>
            {
                count++;
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
                if (!entitiesDB.TryGetSingletonComponent(CanvasGroup.Group, out GameObjectReference canvasGor))
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
                var parent = gameObjectResourceManager[canvasGor.Id];

                go = Object.Instantiate(prefab, parent.transform);
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
                (ref RectPosition rp) =>
                {
                    rp.Value = rt.anchoredPosition;
                });
            entitiesDB.TryGetComponent(id, groupID,
                (ref RectBoundary rb) =>
                {
                    rb.Width = rt.sizeDelta.x;
                    rb.Height = rt.sizeDelta.y;
                });

            go.GetComponent<EntityReferenceHolder>().EGID = new(entityIds[i], groupID);
        }
    }
}