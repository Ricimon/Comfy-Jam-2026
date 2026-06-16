using UnityEngine;
using ECS;
using Svelto.ECS;

public struct SlimeBrain : IEntityComponent
{
    public MovementState MovementState;
    public uint PenId;
    public bool RandomizePositionInPen;
}
