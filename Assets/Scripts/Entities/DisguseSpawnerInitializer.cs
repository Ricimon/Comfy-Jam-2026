using UnityEngine;

public class DisguseSpawnerInitializer : MonoBehaviour
{
    public GameObject disguiseDefaultPrefab;
    public GameObject disguiseBlueMaskPrefab;
    public GameObject disguiseYellowHoodiePrefab;
    public GameObject disguiseBlueHoodiePrefab;

    private void Start()
    {
        var goId = GameContext.ResourceManagers.Add(gameObject);

        GameContext.World.RemoveEntitiesFromGroup(DisguiseSpawnerEntity.Group);

        var entity = GameContext.World.Entity<DisguiseSpawnerEntity>(DisguiseSpawnerEntity.Group);

        entity.Init(new DisguiseSpawner
        {
            DisguiseDefaultPrefabId = GameContext.ResourceManagers.Add(disguiseDefaultPrefab),
            DisguiseBlueMaskPrefabId = GameContext.ResourceManagers.Add(disguiseBlueMaskPrefab),
            DisguiseYellowHoodiePrefabId = GameContext.ResourceManagers.Add(disguiseYellowHoodiePrefab),
            DisguiseBlueHoodiePrefabId = GameContext.ResourceManagers.Add(disguiseBlueHoodiePrefab),
        });

        entity.Init(new GameObjectReference
        {
            Id = goId,
        });
    }
}