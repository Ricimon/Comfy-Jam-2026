using Svelto.DataStructures.Experimental;
using Svelto.ECS;

public struct SlimeSpawner : IEntityComponent
{
    public ValueIndex SlimePrefabId;
    public ValueIndex SlimeBluePrefabId;
    public ValueIndex SlimeYellowPrefabId;
    public ValueIndex SpawnRateCurveId;
    public float TimeUntilSpawn;
    public float SpawnInterval;
}