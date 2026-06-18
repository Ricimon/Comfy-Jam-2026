using UnityEngine;

public class SlimeDropletSpawnerInitializer : MonoBehaviour
{
    public GameObject slimeDropletPrefab;

    private void Start()
    {
        GameContext.World.RemoveEntitiesFromGroup(SlimeDropletSpawnerEntity.Group);

        var e = GameContext.World.Entity<SlimeDropletSpawnerEntity>(SlimeDropletSpawnerEntity.Group);

        e.Init(new SlimeDropletSpawner
        {
            DropletPrefabId = GameContext.ResourceManagers.Add(slimeDropletPrefab),
        });
    }
}