using Svelto.ECS;
using UnityEngine;

public enum DisguiseType
{
    None,
    Default,
}

public struct Disguise : IEntityComponent
{
    public DisguiseType Type;
    public EGID SlimeId;
    public bool ShouldRemove;

    // Flyaway animation parameters
    public float RemovalAnimatingTime;
    public Vector2 RemovalFlyVector;
    public float RemovalRotationSpeed;
    public Vector2 RemovalStartingPosition;
}

public class DisguiseEntity : GenericEntityDescriptorAndGroup<
    Disguise,
    GameObjectReference,
    RectTransformReference,
    RectPosition>
{ }