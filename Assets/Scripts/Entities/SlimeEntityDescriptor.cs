using Svelto.ECS;

public class SlimeEntityDescriptor : GenericEntityDescriptor<
    GameObjectReference,
    Position,
    RectBoundary>
{ }

public class SlimeGroup : GroupTag<SlimeGroup> { }