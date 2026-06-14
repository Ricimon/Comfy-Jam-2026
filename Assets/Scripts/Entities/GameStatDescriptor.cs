using Svelto.ECS;
using UnityEngine;

public class GameStatDescriptor : GenericEntityDescriptor<
    Score,
    Lives>
{ }

public class GameStatTag : NamedExclusiveGroup<GameStatTag> { }