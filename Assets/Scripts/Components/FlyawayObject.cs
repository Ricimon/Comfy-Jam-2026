using Svelto.ECS;
using UnityEngine;

public struct FlyawayObject : IEntityComponent
{
    public float AnimatingTIme;
    public Vector2 FlyVector;
    public Vector2 StartingPosition;
    
    public bool RotationFollowsPath;
    public float RotationSpeed;
}