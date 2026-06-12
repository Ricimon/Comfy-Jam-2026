using UnityEngine;

public class SlimeSpawnerEntityInitializer : MonoBehaviour
{
    public GameObject slimePrefab;

    public void Start()
    {
        var id = GameContext.GameObjectResourceManager.Add(slimePrefab);
        var entity = GameContext.World.Entity<SlimeSpawnerEntityDescriptor>(SlimeSpawnerGroup.Group);

        entity.Init(new SlimeSpawner
        {
            SlimePrefabId = id,
            TimeUntilSpawn = float.PositiveInfinity,
            SpawnInterval = 1.0f,
        });
    }
}