using UnityEngine;

public class SlimeFlightConfigEntityInitializer : MonoBehaviour
{
    public AnimationCurve slimeFlightAnimationCurve;
    public AnimationCurve slimeFlightRotationAnimationCurve;

    private void Start()
    {
        var flightCurveId = GameContext.AnimationCurveResourceManager.Add(slimeFlightAnimationCurve);
        var flightRotationCurveId = GameContext.AnimationCurveResourceManager.Add(slimeFlightRotationAnimationCurve);

        GameContext.World.RemoveEntitiesFromGroup(SlimeFlightConfigEntity.Group);

        var entity = GameContext.World.Entity<SlimeFlightConfigEntity>(SlimeFlightConfigEntity.Group);

        entity.Init(new SlimeFlightConfig
        {
            FlightCurveId = flightCurveId,
            FlightRotationCurveId = flightRotationCurveId,
        });
    }
}