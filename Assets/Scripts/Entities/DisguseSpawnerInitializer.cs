using UnityEngine;

public class DisguseSpawnerInitializer : MonoBehaviour
{
    public GameObject disguiseDefaultPrefab;

    private void Start()
    {
        var goId = GameContext.GameObjectResourceManager.Add(gameObject);
        var idDefault = GameContext.GameObjectResourceManager.Add(disguiseDefaultPrefab);

        GameContext.World.RemoveEntitiesFromGroup(DisguiseSpawnerEntity.Group);

        var entity = GameContext.World.Entity<DisguiseSpawnerEntity>(DisguiseSpawnerEntity.Group);

        entity.Init(new DisguiseSpawner
        {
            DisguiseDefaultPrefabId = idDefault,
        });

        entity.Init(new GameObjectReference
        {
            Id = goId,
        });
    }
}