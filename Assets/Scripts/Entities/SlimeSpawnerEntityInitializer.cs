using UnityEngine;

public class SlimeSpawnerEntityInitializer : MonoBehaviour
{
    public GameObject slimePrefab;
    public GameObject slimeRedPrefab;
    public GameObject slimeBluePrefab;
    public SpawnerConfig spawnerConfig;

    public void Start()
    {
        var goId = GameContext.GameObjectResourceManager.Add(gameObject);
        var id = GameContext.GameObjectResourceManager.Add(slimePrefab);
        var idRed = GameContext.GameObjectResourceManager.Add(slimeRedPrefab);
        var idBlue = GameContext.GameObjectResourceManager.Add(slimeBluePrefab);

        GameContext.World.RemoveEntitiesFromGroup(SlimeSpawnerGroup.Group);

        var entity = GameContext.World.Entity<SlimeSpawnerEntityDescriptor>(SlimeSpawnerGroup.Group);

        entity.Init(new SlimeSpawner
        {
            SlimePrefabId = id,
            SlimeRedPrefabId = idRed,
            SlimeBluePrefabId = idBlue,
            TimeUntilSpawn = 1,
            SpawnInterval = 1.0f,
        });

        entity.Init(new GameObjectReference
        {
            Id = goId,
        });
    }
}