using Svelto.ECS;
using UnityEngine;

public struct SlimeConfig : IEntityComponent
{
    public ResourceIndex<AnimationCurve> MovementCurveId;
}

public class SlimeConfigEntity : GenericEntityDescriptorAndGroup<SlimeConfig> { }