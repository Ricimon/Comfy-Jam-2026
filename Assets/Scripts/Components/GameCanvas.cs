using Svelto.ECS;
using UnityEngine;

public struct GameCanvas : IEntityComponent
{
    public ResourceIndex<GameObject> SlimesParentGoId;
    public ResourceIndex<GameObject> GrabbedObjectGoId;
}