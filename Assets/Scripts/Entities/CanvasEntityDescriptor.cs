using Svelto.ECS;

public class CanvasEntityDescriptor : GenericEntityDescriptor<
    GameObjectReference>
{ }

public class CanvasGroup : NamedExclusiveGroup<CanvasGroup> { }