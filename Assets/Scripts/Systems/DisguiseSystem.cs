using DBC.Common;
using ECS;
using Svelto.ECS;
using UnityEngine;

public class DisguiseSystem : ISystem, IQueryingEntitiesEngine, IReactOnAddEx<GameObjectReference>
{
    public EntitiesDB entitiesDB { get; set; }

    private readonly World world;
    private readonly GameObjectResourceManager gameObjectResourceManager;

    public DisguiseSystem(World world, GameObjectResourceManager gameObjectResourceManager)
    {
        this.world = world;
        this.gameObjectResourceManager = gameObjectResourceManager;
    }

    public void Ready(){}

    public void Update()
    {

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
                go = gameObjectResourceManager[gor.Id];
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
                    DisguiseType.Default => gameObjectResourceManager[disguiseSpawner.DisguiseDefaultPrefabId],
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
                        parent = gameObjectResourceManager[slimeGor.Id];
                    });

                go = Object.Instantiate(prefab, parent.transform);
                gor.Id = gameObjectResourceManager.Add(go);
            }

            go.GetComponent<EntityReferenceHolder>().EGID = new(entityIds[i], groupID);
        }
    }
}