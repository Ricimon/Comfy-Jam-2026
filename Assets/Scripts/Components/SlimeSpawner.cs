using Svelto.DataStructures.Experimental;
using Svelto.ECS;

public struct SlimeSpawner : IEntityComponent
{
    public ValueIndex SlimePrefabId;
    public ValueIndex SlimeRedPrefabId;
    public ValueIndex SlimeBluePrefabId;
    public float TimeUntilSpawn;
    public float SpawnInterval;
}