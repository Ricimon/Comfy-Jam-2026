using Svelto.ECS;

public enum SlimeColor
{
    None,
    Blue,
    Red,
}

public struct Slime : IEntityComponent
{
    public bool CanPickUp;
    public SlimeColor SlimeColor;
}