using ECS;
using Svelto.ECS;

public class SlimeSpawnerSystem : ISystem, IQueryingEntitiesEngine
{
    public EntitiesDB entitiesDB { get; set; }

    public static void SpawnSlime(World world)
    {
        var entity = world.Entity<SlimeEntityDescriptor>(SlimeGroup.BuildGroup);
        entity.Init(new RectTransformReference());

        // entity.Init(new GameObjectReference
        // {
        //     Id = goId,
        // });

        // if (TryGetComponent(out RectTransform rt))
        // {
        //     var rtId = GameContext.RectTransformResourceManager.Add(rt);
        //     entity.Init(new RectTransformReference
        //     {
        //         Id = rtId,
        //     });

        //     entity.Init(new RectPosition
        //     {
        //         Value = rt.anchoredPosition,
        //     });

        //     entity.Init(new RectBoundary
        //     {
        //         Width = rt.sizeDelta.x,
        //         Height = rt.sizeDelta.y,
        //     });
        // }

        // if (TryGetComponent(out EntityReferenceHolder holder))
        // {
        //     holder.EGID = entity.EGID;
        // }
    }

    public void Ready()
    {
    }

    public void Update()
    {
        var (c1, count1) = entitiesDB.QueryEntities<SlimeSpawner>(SlimeSpawnerGroup.Group);
        var (c2, count2) = entitiesDB.QueryEntities<UpdateDeltaTime>(UpdateDeltaTimeEntityDescriptor.Group);
        var deltaTime = count2 > 0 ? c2[0].ValueSeconds : 0;

        for (var i = 0; i < count1; i++)
        {
            ref var spawner = ref c1[i];
            spawner.TimeUntilSpawn -= deltaTime;
            if (spawner.TimeUntilSpawn <= 0)
            {
                UnityEngine.Debug.Log("Spawn!");
                spawner.TimeUntilSpawn += spawner.SpawnInterval;
            }
        }
    }
}