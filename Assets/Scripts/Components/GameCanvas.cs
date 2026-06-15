using Svelto.DataStructures.Experimental;
using Svelto.ECS;

public struct GameCanvas : IEntityComponent
{
    public ValueIndex SlimesParentGoId;
    public ValueIndex GrabbedObjectGoId;
}