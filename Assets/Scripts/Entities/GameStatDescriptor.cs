using Svelto.ECS;
using UnityEngine;

public class GameStatDescriptor : GenericEntityDescriptor<
    Score,
    Lives,
    ElapsedTime>
{ }

public class GameStatTag : NamedExclusiveGroup<GameStatTag> { }