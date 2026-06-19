using UnityEngine;

public class DisguseSpawnerInitializer : MonoBehaviour
{
    public GameObject disguiseDefaultPrefab;
    public GameObject disguiseYellowHoodiePrefab;
    public GameObject disguiseBlueHoodiePrefab;

    private void Start()
    {
        var goId = GameContext.GameObjectResourceManager.Add(gameObject);

        GameContext.World.RemoveEntitiesFromGroup(DisguiseSpawnerEntity.Group);

        var entity = GameContext.World.Entity<DisguiseSpawnerEntity>(DisguiseSpawnerEntity.Group);

        entity.Init(new DisguiseSpawner
        {
            DisguiseDefaultPrefabId = GameContext.GameObjectResourceManager.Add(disguiseDefaultPrefab),
            DisguiseYellowHoodiePrefabId = GameContext.GameObjectResourceManager.Add(disguiseYellowHoodiePrefab),
            DisguiseBlueHoodiePrefabId = GameContext.GameObjectResourceManager.Add(disguiseBlueHoodiePrefab),
        });

        entity.Init(new GameObjectReference
        {
            Id = goId,
        });
    }
}