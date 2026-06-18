using Svelto.ECS;
using UnityEngine;

public class GameStatDescriptor : GenericEntityDescriptor<
    Score,
    Lives,
    ElapsedTime,
    Pause>
{ }

public class GameStatTag : NamedExclusiveGroup<GameStatTag> { }