using UnityEngine;

public class SlimeEntityInitializer : MonoBehaviour
{
    private void Start()
    {
        var id = GameContext.GameObjectResourceManager.Add(gameObject);
        var entity = GameContext.World.Entity<SlimeEntityDescriptor>();

        entity.Init(new GameObjectReference
        {
            Id = id,
        });

        entity.Init(new Position
        {
            Value = transform.position,
        });
    }
}