using Svelto.ECS;
using UnityEngine;

public struct FlyawayObject : IEntityComponent
{
    // Requires setting
    public bool IsActive;
    public bool RotationFollowsPath;

    public float AnimatingTIme;
    public Vector2 FlyVector;
    public Vector2 StartingPosition;
    
    public float RotationSpeed;
}