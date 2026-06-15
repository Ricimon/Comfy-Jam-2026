using Svelto.ECS;

public class CanvasEntityDescriptor : GenericEntityDescriptor<
    GameCanvas,
    GameObjectReference>
{ }

public class CanvasGroup : NamedExclusiveGroup<CanvasGroup> { }