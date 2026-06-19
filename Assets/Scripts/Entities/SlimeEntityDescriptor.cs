using ECS;
using Svelto.ECS;

public class SlimeEntityDescriptor : ExtendibleEntityDescriptor<BaseEntityDescriptor>
{
    public SlimeEntityDescriptor()
    {
        ExtendWith(new IComponentBuilder[]
        {
            new ComponentBuilder<GameObjectReference>(),
            new ComponentBuilder<Slime>(),
            new ComponentBuilder<SlimeBrain>(),
            new ComponentBuilder<RectTransformReference>(),
            new ComponentBuilder<RectPosition>(),
            new ComponentBuilder<RectBoundary>(),
            new ComponentBuilder<Direction>(),
        });
    }
}

public class SlimeGroup : GroupTag<SlimeGroup> { }