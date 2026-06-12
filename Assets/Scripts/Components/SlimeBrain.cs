using UnityEngine;
using ECS;
using Svelto.ECS;

public struct SlimeBrain : IEntityComponent
{
    public MovementState MovementState;
    public SortingPenType SortingPen;
}
