using Svelto.ECS;
using UnityEngine;

public struct SlimeDroplet : IEntityComponent
{
    public Color Color;
    public int TransformSiblingIndex;
}

public class SlimeDropletEntity : GenericEntityDescriptorAndGroup<
    SlimeDroplet,
    FlyawayObject,
    RectPosition,
    RectTransformReference,
    GameObjectReference>
{ }