using DBC.Common;
using ECS;
using Mono.Cecil.Cil;
using Svelto.DataStructures.Experimental;
using Svelto.ECS;
using UnityEngine;

public class SlimeSystem : ISystem, IQueryingEntitiesEngine, IReactOnAddEx<GameObjectReference>
{
    public EntitiesDB entitiesDB { get; set; }

    private readonly World world;
    private readonly ResourceManagers resourceManagers;

    public SlimeSystem(World world, ResourceManagers resourceManagers)
    {
        this.world = world;
        this.resourceManagers = resourceManagers;
    }

    public void Ready()
    {
    }

    public void Update()
    {
        entitiesDB.QueryEntities<GameObjectReference, Slime, SlimeBrain>(SlimeGroup.Groups)
            .Each((ref GameObjectReference gor, ref Slime slime, ref SlimeBrain slimeBrain) =>
            {
                // Put slime under correct parent
                // Make slime bigger when grabbed
                var go = resourceManagers.Get<GameObject>(gor.Id);
                var gameCanvas = entitiesDB.GetSingletonComponent<GameCanvas>(CanvasGroup.Group);
                if (slimeBrain.MovementState == MovementState.Grabbed)
                {
                    var grabbedSlimeParent = gameCanvas.GrabbedObjectGoId.ToObject(resourceManagers);
                    if (go.transform.parent != grabbedSlimeParent.transform)
                    {
                        go.transform.SetParent(grabbedSlimeParent.transform);
                        go.transform.SetAsLastSibling();
                    }
                    go.transform.localScale = 1.25f * Vector3.one;
                }
                else
                {
                    var  slimesParent = gameCanvas.SlimesParentGoId.ToObject(resourceManagers);
                    if (go.transform.parent != slimesParent.transform)
                    {
                        go.transform.SetParent(slimesParent.transform);
                        go.transform.SetAsLastSibling();
                    }
                    go.transform.localScale = Vector3.one;
                }

                // Keep slime upright when not flying
                if (slimeBrain.MovementState != MovementState.Flying)
                {
                    go.transform.localRotation = default;
                }

                // Angry eyebrows
                var angryEyebrows = resourceManagers.Get<SlimeGameObject>(slime.SlimeGameObjectId).angryEyebrows;
                angryEyebrows.SetActive(slimeBrain.IsSpeedUp);
            });

        entitiesDB.QueryEntities<Slime, SlimeBrain, RectPosition, RectTransformReference>(SlimeGroup.Groups)
            .Each((EGID egid, ref Slime slime, ref SlimeBrain sb, ref RectPosition rp, ref RectTransformReference rtr) =>
            {
                if (SortingPenGroup.Includes(sb.PenId.groupID))
                {
                    return;
                }

                var udt = entitiesDB.GetSingletonComponent<UpdateDeltaTime>(UpdateDeltaTimeEntity.Group);
                ref var timeAlive = ref slime.TimeAlive;
                timeAlive += udt.ValueSeconds;

                var lifeDuration = 10.0f;

                if (timeAlive > lifeDuration)
                {
                    // Slime dies and lose a life
                    if (slime.TimePlayingDeathAnimation == 0)
                    {
                        var numDroplets = 3;
                        for (var i = 0; i < numDroplets; i++)
                        {
                            var dropletEntity = world.Entity<SlimeDropletEntity>(SlimeDropletEntity.Group);
                            dropletEntity.Init(new SlimeDroplet
                            {
                                Color = resourceManagers.Get<SlimeGameObject>(slime.SlimeGameObjectId).dropletColor,
                            });
                            dropletEntity.Init(new RectPosition
                            {
                                Value = rp.Value,
                            });
                        }

                        entitiesDB.QueryEntities<Lives>(GameStatTag.Group)
                            .Each((ref Lives lives) =>
                            {
                                lives.Value--;
                            });
                    }

                    slime.CanPickUp = false;
                    sb.MovementState = MovementState.NoMovement;
                    ref var deathTime = ref slime.TimePlayingDeathAnimation;
                    deathTime += udt.ValueSeconds;

                    var deathAnimationDuration = 0.5f;

                    var rt = rtr.Id.ToObject(resourceManagers);
                    rt.localScale = new Vector3(1.0f, 1.0f - (deathTime / deathAnimationDuration), 1.0f);

                    if (slime.TimePlayingDeathAnimation > deathAnimationDuration)
                    {
                        world.RemoveEntity<BaseEntityDescriptor>(egid);
                    }
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
                go = resourceManagers.Get<GameObject>(gor.Id);
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
                    SlimeColor.Yellow => resourceManagers.Get<GameObject>(slimeSpawner.SlimeYellowPrefabId),
                    SlimeColor.Blue => resourceManagers.Get<GameObject>(slimeSpawner.SlimeBluePrefabId),
                    _ => resourceManagers.Get<GameObject>(slimeSpawner.SlimePrefabId),
                };
                var parent = resourceManagers.Get<GameObject>(gameCanvas.SlimesParentGoId);

                go = Object.Instantiate(prefab, parent.transform);
                go.transform.SetAsFirstSibling();
                gor.Id = resourceManagers.Add(go);

                rt = go.GetComponent<RectTransform>();
                rtId = resourceManagers.Add(rt);
            }

            entitiesDB.TryGetComponent(id, groupID,
                (ref Slime slime) =>
                {
                    slime.SlimeGameObjectId = resourceManagers.Add(go.GetComponent<SlimeGameObject>());
                });
            entitiesDB.TryGetComponent(id, groupID,
                (ref RectTransformReference rtr) =>
                {
                    rtr.Id = rtId.ToResourceIndex<RectTransform>();
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
                    entitiesDB.TryGetComponent(sb.PenId,
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