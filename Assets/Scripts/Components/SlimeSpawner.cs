using Svelto.DataStructures.Experimental;
using Svelto.ECS;
using UnityEngine;

public struct SlimeSpawner : IEntityComponent
{
    public ValueIndex SlimePrefabId;
    public ValueIndex SlimeRedPrefabId;
    public ValueIndex SlimeBluePrefabId;
    public float TimeUntilSpawn;
    public float SpawnInterval;
}