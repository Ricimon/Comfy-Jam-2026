using UnityEngine;

public class SlimeEntityInitializer : MonoBehaviour
{
    private void Start()
    {
        var goId = GameContext.GameObjectResourceManager.Add(gameObject);
        var entity = GameContext.World.Entity<SlimeEntityDescriptor>(SlimeGroup.BuildGroup);

        entity.Init(new GameObjectReference
        {
            Id = goId,
        });

        if (TryGetComponent(out RectTransform rt))
        {
            var rtId = GameContext.RectTransformResourceManager.Add(rt);
            entity.Init(new RectTransformReference
            {
                Id = rtId,
            });

            entity.Init(new RectPosition
            {
                Value = rt.anchoredPosition,
            });

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