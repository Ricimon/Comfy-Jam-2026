using UnityEngine;

public class DisguseSpawnerInitializer : MonoBehaviour
{
    public GameObject disguiseDefaultPrefab;
    public GameObject disguiseBlueMaskPrefab;
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
            DisguiseBlueMaskPrefabId = GameContext.GameObjectResourceManager.Add(disguiseBlueMaskPrefab),
            DisguiseYellowHoodiePrefabId = GameContext.GameObjectResourceManager.Add(disguiseYellowHoodiePrefab),
            DisguiseBlueHoodiePrefabId = GameContext.GameObjectResourceManager.Add(disguiseBlueHoodiePrefab),
        });

        entity.Init(new GameObjectReference
        {
            Id = goId,
        });
    }
}