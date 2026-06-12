using Svelto.ECS;

public class SlimeEntityDescriptor : GenericEntityDescriptor<
    GameObjectReference,
    SlimeBrain,
    RectTransformReference,
    RectPosition,
    RectBoundary>
{ }

public class SlimeGroup : GroupTag<SlimeGroup> { }