using DBC.Common;
using ECS;
using Svelto.ECS;
using UnityEngine;

public class DisguiseSystem : ISystem, IQueryingEntitiesEngine, IReactOnAddEx<GameObjectReference>
{
    public EntitiesDB entitiesDB { get; set; }

    private readonly World world;
    private readonly ResourceManagers resourceManagers;

    public DisguiseSystem(World world, ResourceManagers resourceManagers)
    {
        this.world = world;
        this.resourceManagers = resourceManagers;
    }

    public void Ready() { }

    public void Update()
    {
        entitiesDB.QueryEntities<Disguise, GameObjectReference, RectTransformReference, RectPosition>(DisguiseEntity.Group)
            .Each((uint id, ref Disguise disguise, ref GameObjectReference gor, ref RectTransformReference rtr, ref RectPosition rp) =>
            {
                if (!disguise.ShouldRemove) { return; }

                if (disguise.SlimeId.IsValid())
                {
                    var slimeGor = entitiesDB.GetComponent<GameObjectReference>(disguise.SlimeId);
                    var slimeRp = entitiesDB.GetComponent<RectPosition>(disguise.SlimeId);
                    var slimeGo = resourceManagers.Get<GameObject>(slimeGor.Id);

                    var go = resourceManagers.Get<GameObject>(gor.Id);

                    // Remove from Slime
                    disguise.SlimeId = default;

                    go.transform.SetParent(slimeGo.transform.parent, true);
                    go.transform.SetSiblingIndex(slimeGo.transform.GetSiblingIndex() + 1);

                    // Set randomized parameters
                    var angle = Random.Range(30.0f, 150.0f) * Mathf.Deg2Rad;
                    var speed = Random.Range(200.0f, 500.0f);
                    disguise.RemovalFlyVector = speed * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                    disguise.RemovalRotationSpeed = Random.Range(200.0f, 400.0f);
                    disguise.RemovalStartingPosition = slimeRp.Value;

                    AudioClipSystem.PlaySFX(SFX.Disguise);
                }

                var deltaTime = entitiesDB.GetSingletonComponent<UpdateDeltaTime>(UpdateDeltaTimeEntity.Group).ValueSeconds;
                ref var time = ref disguise.RemovalAnimatingTime;
                time += deltaTime;

                if (time > 5.0f)
                {
                    world.RemoveEntity<DisguiseEntity>(id, DisguiseEntity.Group);
                    return;
                }

                var x = disguise.RemovalStartingPosition.x + disguise.RemovalFlyVector.x * time;
                var y = disguise.RemovalStartingPosition.y + disguise.RemovalFlyVector.y * time + 0.5f * -1000.0f * time * time;
                rp.Value = new(x, y);

                var rt = rtr.Id.ToObject(resourceManagers);
                rt.Rotate(0, 0, disguise.RemovalRotationSpeed * deltaTime);
            });
    }

    public void Add((uint start, uint end) rangeOfEntities, in EntityCollection<GameObjectReference> entities, ExclusiveGroupStruct groupID)
    {
        if (groupID != DisguiseEntity.Group) { return; }

        var (gors, entityIds, _) = entities;
        for (var i = rangeOfEntities.start; i < rangeOfEntities.end; i++)
        {
            var id = entityIds[i];
            ref var gor = ref gors[i];

            GameObject go;

            try
            {
                go = resourceManagers.Get<GameObject>(gor.Id);
            }
            catch (PreconditionException)
            {
                // Create GameObject
                if (!entitiesDB.TryGetSingletonComponent(DisguiseSpawnerEntity.Group, out DisguiseSpawner disguiseSpawner))
                {
                    return;
                }

                var disguise = entitiesDB.QueryEntity<Disguise>(id, groupID);
                GameObject prefab = disguise.Type switch
                {
                    DisguiseType.Default => disguiseSpawner.DisguiseDefaultPrefabId.ToObject(resourceManagers),
                    DisguiseType.YellowHoodie => disguiseSpawner.DisguiseYellowHoodiePrefabId.ToObject(resourceManagers),
                    DisguiseType.BlueHoodie => disguiseSpawner.DisguiseBlueHoodiePrefabId.ToObject(resourceManagers),
                    _ => null,
                };

                if (prefab == null)
                {
                    world.RemoveEntity<DisguiseEntity>(id, groupID);
                    return;
                }

                GameObject parent = null;
                var slime = entitiesDB.TryGetComponent(disguise.SlimeId,
                    (ref GameObjectReference slimeGor) =>
                    {
                        parent = resourceManagers.Get<GameObject>(slimeGor.Id);
                    });

                go = Object.Instantiate(prefab, parent.transform);
                gor.Id = resourceManagers.Add(go);

                var rt = go.GetComponent<RectTransform>();
                var rtId = resourceManagers.Add(rt);
                entitiesDB.TryGetComponent(id, groupID,
                    (ref RectTransformReference rtr) =>
                    {
                        rtr.Id = rtId.ToResourceIndex<RectTransform>();
                    });
                entitiesDB.TryGetComponent(id, groupID,
                    (ref RectPosition rp) =>
                    {
                        rp.Value = rt.anchoredPosition;
                    });
            }

            go.GetComponent<EntityReferenceHolder>().EGID = new(entityIds[i], groupID);
        }
    }
}