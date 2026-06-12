using Svelto.ECS;

public class PenEntityDescriptor : GenericEntityDescriptor<
    GameObjectReference,
    RectBoundary,
    RectPosition>
{ }

public class SortingPenEntityDescriptor : ExtendibleEntityDescriptor<PenEntityDescriptor>
{
    public SortingPenEntityDescriptor()
    {
        Add<SortingPen>();
    }
}

public class PenGroupTag : GroupTag<PenGroupTag> { }
public class SortingPenGroupTag : GroupTag<SortingPenGroupTag> { }
public class SortingPenGroup : GroupCompound<PenGroupTag, SortingPenGroupTag> { }