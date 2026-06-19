using Svelto.ECS;
using UnityEngine;

public class AudioClipEntityDescriptor : GenericEntityDescriptor<AudioClips>
{
}

public class AudioClipGroup : NamedExclusiveGroup<AudioClipGroup> { }