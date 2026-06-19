using Svelto.ECS;
using UnityEngine;

public class SoundEffectEntityDescriptor : GenericEntityDescriptor<SoundEffect>
{
}

public class SoundEffectGroup : NamedExclusiveGroup<SoundEffectGroup> { }