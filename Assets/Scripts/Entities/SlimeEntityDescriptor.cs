using Svelto.ECS;

public class SlimeEntityDescriptor : GenericEntityDescriptor<
    GameObjectReference,
    SlimeBrain,
    RectTransformReference,
    RectPosition,
    RectBoundary,
    Direction>
{ }

public class SlimeGroup : GroupTag<SlimeGroup> { }