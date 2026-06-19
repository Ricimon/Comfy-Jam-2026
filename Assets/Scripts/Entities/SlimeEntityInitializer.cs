using UnityEngine;

public class SlimeEntityInitializer : MonoBehaviour
{
    private void Start()
    {
        var goId = GameContext.ResourceManagers.Add(gameObject);
        var entity = SlimeSpawnerSystem.SpawnSlime(GameContext.World, SlimeColor.None);

        entity.Init(new GameObjectReference
        {
            Id = goId,
        });

        // entity.Init(new SlimeBrain
        // {
        //     MovementState = MovementState.Grabbed,
        // });

        entity.Init(new Direction
        {
            Value = Vector2.zero,
        });
    }
}