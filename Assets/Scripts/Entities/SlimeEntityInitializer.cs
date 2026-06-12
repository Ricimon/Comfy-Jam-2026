using UnityEngine;

public class SlimeEntityInitializer : MonoBehaviour
{
    private void Start()
    {
        var id = GameContext.GameObjectResourceManager.Add(gameObject);
        var entity = GameContext.World.Entity<SlimeEntityDescriptor>(SlimeGroup.BuildGroup);

        entity.Init(new GameObjectReference
        {
            Id = id,
        });

        entity.Init(new Position
        {
            Value = transform.position,
        });

        if (TryGetComponent(out RectTransform rt))
        {
            entity.Init(new RectBoundary
            {
                Width = rt.sizeDelta.x,
                Height = rt.sizeDelta.y,
            });
        }

        if (TryGetComponent(out EntityReferenceHolder holder))
        {
            holder.EGID = entity.EGID;
        }
    }
}