using UnityEngine;
using ECS;
using Svelto.ECS;

public struct SlimeBrain : IEntityComponent
{
    public MovementState MovementState;
    public uint PenId;
    public bool RandomizePositionInPen;

    // Flight animation parameters
    public float FlightAnimationTime;
    public Vector2 FlightStartingPosition;
    public Vector2 FlightTargetPosition;
    public float FlightRotationSpeed;
}
