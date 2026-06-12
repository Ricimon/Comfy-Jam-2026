using Svelto.ECS;

public class SlimeEntityDescriptor : GenericEntityDescriptor<
    GameObjectReference,
    Position,
    SlimeBrain,
    RectBoundary>
{ }

public class SlimeGroup : GroupTag<SlimeGroup> { }