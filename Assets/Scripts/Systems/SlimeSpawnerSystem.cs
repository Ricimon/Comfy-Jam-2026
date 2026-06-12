using ECS;
using Svelto.ECS;

public class SlimeSpawnerSystem : ISystem, IQueryingEntitiesEngine
{
    public EntitiesDB entitiesDB { get; set; }

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