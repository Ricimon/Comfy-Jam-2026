using Svelto.DataStructures.Experimental;
using Svelto.ECS;

public struct GameObjectReference : IEntityComponent
{
    public ValueIndex Id;
}