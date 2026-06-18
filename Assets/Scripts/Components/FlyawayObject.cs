using Svelto.ECS;
using UnityEngine;

public struct FlyawayObject : IEntityComponent
{
    // Requires setting
    public bool IsActive;
    public Vector2 StartingPosition;

    public float AnimatingTIme;
    public Vector2 FlyVector;
    
    public bool RotationFollowsPath;
    public float RotationSpeed;
}