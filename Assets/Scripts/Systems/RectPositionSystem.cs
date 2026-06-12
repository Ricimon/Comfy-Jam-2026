using ECS;
using Svelto.ECS;

public class RectPositionSystem : ISystem, IQueryingEntitiesEngine
{
    public EntitiesDB entitiesDB { get; set; }

    private readonly RectTransformResourceManager rectTransformResourceManager;

    public RectPositionSystem(RectTransformResourceManager rectTransformResourceManager)
    {
        this.rectTransformResourceManager = rectTransformResourceManager;
    }

    public void Ready()
    {

    }

    public void Update()
    {
        var groups = entitiesDB.FindGroups<RectTransformReference, RectPosition>();
        entitiesDB.QueryEntities<RectTransformReference, RectPosition>(groups)
            .Each((ref RectTransformReference rtr, ref RectPosition rp) =>
            {
                var rt = rectTransformResourceManager[rtr.Id];
                if (rt != null)
                {
                    rt.anchoredPosition = rp.Value;
                }
            });
    }
}
