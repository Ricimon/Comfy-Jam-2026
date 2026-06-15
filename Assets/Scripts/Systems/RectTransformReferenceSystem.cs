using ECS;
using Svelto.ECS;

public class RectTransformReferenceSystem : ISystem, IReactOnRemoveEx<RectTransformReference>
{
    private readonly RectTransformResourceManager rectTransformResourceManager;

    public RectTransformReferenceSystem(RectTransformResourceManager rectTransformResourceManager)
    {
        this.rectTransformResourceManager = rectTransformResourceManager;
    }

    public void Update()
    {
        
    }

    public void Remove((uint start, uint end) rangeOfEntities, in EntityCollection<RectTransformReference> entities, ExclusiveGroupStruct groupID)
    {
        var (rtrs, _) = entities;
        for (var i = rangeOfEntities.start; i < rangeOfEntities.end; i++)
        {
            rectTransformResourceManager.Remove(rtrs[i].Id);
        }
    }
}