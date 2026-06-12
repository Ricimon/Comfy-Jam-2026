using Svelto.ECS;

public enum SortingPenType
{
    None,
    Red,
    Blue,
}

public struct SortingPen : IEntityComponent
{
    public SortingPenType Type;
}