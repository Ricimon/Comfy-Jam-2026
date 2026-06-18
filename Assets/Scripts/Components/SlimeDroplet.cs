using Svelto.ECS;
using UnityEngine;

public struct SlimeDroplet : IEntityComponent
{
    public Color Color;
}

public class SlimeDropletEntity : GenericEntityDescriptorAndGroup<
    SlimeDroplet,
    FlyawayObject,
    RectPosition,
    RectTransformReference,
    GameObjectReference>
{ }