using Svelto.DataStructures.Experimental;
using Svelto.ECS;
using UnityEngine;

public struct GameObjectReference : IEntityComponent
{
    public ValueIndex Id;
}

public struct RectTransformReference : IEntityComponent
{
    public ResourceIndex<RectTransform> Id;
}