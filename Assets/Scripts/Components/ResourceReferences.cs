using Svelto.ECS;
using UnityEngine;

public struct GameObjectReference : IEntityComponent
{
    public ResourceIndex<GameObject> Id;
}

public struct RectTransformReference : IEntityComponent
{
    public ResourceIndex<RectTransform> Id;
}