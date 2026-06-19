using UnityEngine;

public class CanvasEntityInitializer : MonoBehaviour
{
    public GameObject slimesParent;
    public GameObject grabbedObjectParent;

    private void Start()
    {
        var id = GameContext.ResourceManagers.Add(gameObject);
        var slimesParentId = GameContext.ResourceManagers.Add(slimesParent);
        var grabbedObjectId = GameContext.ResourceManagers.Add(grabbedObjectParent);
        var entity = GameContext.World.Entity<CanvasEntityDescriptor>(CanvasGroup.Group);

        entity.Init(new GameCanvas
        {
            SlimesParentGoId = slimesParentId.ToResourceIndex<GameObject>(),
            GrabbedObjectGoId = grabbedObjectId.ToResourceIndex<GameObject>(),
        });

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