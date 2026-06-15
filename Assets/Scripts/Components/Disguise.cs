using Svelto.ECS;

public enum DisguiseType
{
    None,
    Default,
}

public struct Disguise : IEntityComponent
{
    public DisguiseType Type;
    public EGID SlimeId;
}

public class DisguiseEntity : GenericEntityDescriptorAndGroup<
    Disguise,
    GameObjectReference>
{ }