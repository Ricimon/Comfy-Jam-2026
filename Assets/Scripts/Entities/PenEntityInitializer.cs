using Svelto.ECS;
using UnityEngine;

public class PenEntityInitializer : MonoBehaviour
{
    public SlimeColor sortingPenType;

    private void Start()
    {
        var id = GameContext.ResourceManagers.Add(gameObject);

        var isSortingPen = sortingPenType != SlimeColor.None;

        EntityInitializer entity;
        if (isSortingPen)
        {
            entity = GameContext.World.Entity<SortingPenEntityDescriptor>(SortingPenGroup.BuildGroup);
            entity.Init(new SortingPen
            {
                Type = sortingPenType,
            });
        }
        else
        {
            entity = GameContext.World.Entity<PenEntityDescriptor>(MainPenGroup.BuildGroup);
        }

        entity.Init(new GameObjectReference
        {
            Id = id,
        });

        if (TryGetComponent(out RectTransform rt))
        {
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