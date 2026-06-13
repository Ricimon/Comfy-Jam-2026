using UnityEngine;

public class SlimeEntityInitializer : MonoBehaviour
{
    private void Start()
    {
        var goId = GameContext.GameObjectResourceManager.Add(gameObject);
        var entity = SlimeSpawnerSystem.SpawnSlime(GameContext.World);

        entity.Init(new GameObjectReference
        {
            Id = goId,
        });
    }
}