using UnityEngine;

public class SlimeConfigInitializer : MonoBehaviour
{
    public AnimationCurve slimeMovementCurve;

    private void Start()
    {
        GameContext.World.RemoveEntitiesFromGroup(SlimeConfigEntity.Group);
        var e = GameContext.World.Entity<SlimeConfigEntity>(SlimeConfigEntity.Group);
        e.Init(new SlimeConfig
        {
            MovementCurveId = GameContext.ResourceManagers.Add(slimeMovementCurve),
        });
    }
}