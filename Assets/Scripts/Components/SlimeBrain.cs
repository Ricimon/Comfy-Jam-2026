using Svelto.ECS;
using UnityEngine;

public struct SlimeBrain : IEntityComponent
{
    public MovementState MovementState;
    public bool IsSpeedUp;
    public EGID PenId;
    public bool RandomizePositionInPen;

    // Flight animation parameters
    public float FlightAnimationTime;
    public Vector2 FlightStartingPosition;
    public Vector2 FlightTargetPosition;
    public int FlightRotationDirection;
}
