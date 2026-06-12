using Svelto.ECS;
using UnityEngine;

public class PenEntityInitializer : MonoBehaviour
{
    public SortingPenType sortingPenType;

    private void Start()
    {
        var id = GameContext.GameObjectResourceManager.Add(gameObject);

        var isSortingPen = sortingPenType != SortingPenType.None;

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
            entity = GameContext.World.Entity<PenEntityDescriptor>(PenGroupTag.BuildGroup);
        }

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