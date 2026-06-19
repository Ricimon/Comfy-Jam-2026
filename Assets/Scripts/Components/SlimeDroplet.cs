using Svelto.ECS;
using UnityEngine;

public struct SlimeDroplet : IEntityComponent
{
    public Color Color;
    public float TimeAlive;
}

public class SlimeDropletEntity : GenericEntityDescriptorAndGroup<
    SlimeDroplet,
    FlyawayObject,
    RectPosition,
    RectTransformReference,
    GameObjectReference>
{ }