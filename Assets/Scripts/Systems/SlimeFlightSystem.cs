using ECS;
using Svelto.ECS;
using UnityEngine;

public class SlimeFlightSystem : ISystem, IQueryingEntitiesEngine
{
    public EntitiesDB entitiesDB { get; set; }

    private const float FlightTime = 1.0f;

    private readonly RectTransformResourceManager rectTransformResourceManager;
    private readonly AnimationCurveResourceManager animationCurveResourceManager;

    public SlimeFlightSystem(RectTransformResourceManager rectTransformResourceManager, AnimationCurveResourceManager animationCurveResourceManager)
    {
        this.rectTransformResourceManager = rectTransformResourceManager;
        this.animationCurveResourceManager = animationCurveResourceManager;
    }

    public void Ready() { }

    public void Update()
    {
        entitiesDB.QueryEntities<SlimeBrain, RectTransformReference, RectPosition, RectBoundary>(SlimeGroup.Groups)
            .Each((ref SlimeBrain sb, ref RectTransformReference rtr, ref RectPosition rp, ref RectBoundary rb) =>
            {
                if (sb.MovementState != MovementState.Flying) { return; }

                if (sb.FlightAnimationTime == 0)
                {
                    // Set flight parameters
                    sb.FlightStartingPosition = rp.Value;

                    Vector2 slimeExtents = 0.5f * new Vector2(rb.Width, rb.Height);
                    Vector2 startPosition = sb.FlightStartingPosition;
                    Vector2 targetPosition = default;
                    entitiesDB.TryGetComponent(sb.PenId,
                        (ref RectPosition rp, ref RectBoundary rb) =>
                        {
                            var penExtents = 0.5f * new Vector2(rb.Width, rb.Height);
                            var xMin = rp.Value.x - penExtents.x + slimeExtents.x;
                            var xMax = rp.Value.x + penExtents.x - slimeExtents.x;
                            var yMin = rp.Value.y - penExtents.y + slimeExtents.y;
                            var yMax = rp.Value.y + penExtents.y - slimeExtents.y;

                            var minDistance = 4.0f * slimeExtents.x;
                            for (var i = 0; i < 10; i++)
                            {
                                targetPosition = new(
                                    Random.Range(xMin, xMax),
                                    Random.Range(yMin, yMax)
                                );
                                if (Vector2.Distance(startPosition, targetPosition) > minDistance)
                                {
                                    break;
                                }
                            }
                        });
                    sb.FlightTargetPosition = targetPosition;

                    sb.FlightRotationDirection = Random.value < 0.5f ? -1 : 1;
                }

                var deltaTime = entitiesDB.GetSingletonComponent<UpdateDeltaTime>(UpdateDeltaTimeEntity.Group).ValueSeconds;
                ref var time = ref sb.FlightAnimationTime;
                time += deltaTime;

                // Set flight time
                if (time > FlightTime)
                {
                    sb.MovementState = MovementState.Wander;
                    // Make slime angry
                    sb.IsSpeedUp = true;
                    return;
                }

                if (entitiesDB.TryGetSingletonComponent(SlimeFlightConfigEntity.Group, out SlimeFlightConfig flightConfig))
                {
                    var flightCurve = animationCurveResourceManager[flightConfig.FlightCurveId];
                    var t = flightCurve.Evaluate(time / FlightTime);
                    rp.Value = Vector2.Lerp(sb.FlightStartingPosition, sb.FlightTargetPosition, t);

                    flightCurve = animationCurveResourceManager[flightConfig.FlightRotationCurveId];
                    t = flightCurve.Evaluate(time / FlightTime);
                    var numRotations = 3 * sb.FlightRotationDirection;
                    var rt = rectTransformResourceManager[rtr.Id];
                    rt.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, numRotations * 360.0f, t));
                }
            });
    }
}