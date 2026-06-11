using UnityEngine;

public class CanvasEntityInitializer : MonoBehaviour
{
    private void Start()
    {
        var id = GameContext.GameObjectResourceManager.Add(gameObject);
        var entity = GameContext.World.Entity<CanvasEntityDescriptor>(Canvas.Group);

        entity.Init(new GameObjectReference
        {
            Id = id,
        });

        if (TryGetComponent(out EntityReferenceHolder holder))
        {
            holder.EGID = entity.EGID;
        }
    }
}