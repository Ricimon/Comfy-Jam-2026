using Svelto.DataStructures.Experimental;
using Svelto.ECS;

public struct SlimeSpawner : IEntityComponent
{
    public ValueIndex SlimePrefabId;
    public float TimeUntilSpawn;
    public float SpawnInterval;
}