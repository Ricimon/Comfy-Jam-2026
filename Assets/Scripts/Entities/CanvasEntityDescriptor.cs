using Svelto.ECS;

public class CanvasEntityDescriptor : GenericEntityDescriptor<
    GameObjectReference>
{ }

public class Canvas : NamedExclusiveGroup<Canvas> { }