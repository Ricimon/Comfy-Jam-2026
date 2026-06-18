using UnityEngine;

public class SlimeSpawnerEntityInitializer : MonoBehaviour
{
    public GameObject slimePrefab;
    public GameObject slimeYellowPrefab;
    public GameObject slimeBluePrefab;
    public SpawnerConfig spawnerConfig;

    public void Start()
    {
        var goId = GameContext.GameObjectResourceManager.Add(gameObject);
        var id = GameContext.GameObjectResourceManager.Add(slimePrefab);
        var idYellow = GameContext.GameObjectResourceManager.Add(slimeYellowPrefab);
        var idBlue = GameContext.GameObjectResourceManager.Add(slimeBluePrefab);
        var idSpawnRate = GameContext.AnimationCurveResourceManager.Add(spawnerConfig.SpawnRate);

        GameContext.World.RemoveEntitiesFromGroup(SlimeSpawnerGroup.Group);

        var entity = GameContext.World.Entity<SlimeSpawnerEntityDescriptor>(SlimeSpawnerGroup.Group);

        entity.Init(new SlimeSpawner
        {
            SlimePrefabId = id,
            SlimeYellowPrefabId = idYellow,
            SlimeBluePrefabId = idBlue,
            SpawnRateCurveId = idSpawnRate,
            TimeUntilSpawn = 1,
            SpawnInterval = 1.0f,
        });

        entity.Init(new GameObjectReference
        {
            Id = goId,
        });
    }
}