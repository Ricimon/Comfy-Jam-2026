using Svelto.DataStructures.Experimental;
using Svelto.ECS;

public enum SlimeColor
{
    None,
    Blue,
    Red,
    Yellow,
}

public struct Slime : IEntityComponent
{
    public bool CanPickUp;
    public SlimeColor SlimeColor;
    public ValueIndex SlimeGameObjectId;
    public EGID DisguiseId;
}