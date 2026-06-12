using Svelto.ECS;

public class SlimeEntityDescriptor : GenericEntityDescriptor<
    GameObjectReference,
    RectTransformReference,
    RectPosition,
    RectBoundary>
{ }

public class SlimeGroup : GroupTag<SlimeGroup> { }