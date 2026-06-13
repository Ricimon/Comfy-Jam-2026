using Svelto.ECS;

public class SlimeEntityDescriptor : IEntityDescriptor
{
    private static readonly IComponentBuilder[] _componentBuilders;

    public IComponentBuilder[] componentsToBuild => _componentBuilders;

    static SlimeEntityDescriptor()
    {
        _componentBuilders = new IComponentBuilder[]
        {
            new ComponentBuilder<GameObjectReference>(),
            new ComponentBuilder<Slime>(),
            new ComponentBuilder<SlimeBrain>(),
            new ComponentBuilder<RectTransformReference>(),
            new ComponentBuilder<RectPosition>(),
            new ComponentBuilder<RectBoundary>(),
            new ComponentBuilder<Direction>(),
        };
    }
}

public class SlimeGroup : GroupTag<SlimeGroup> { }