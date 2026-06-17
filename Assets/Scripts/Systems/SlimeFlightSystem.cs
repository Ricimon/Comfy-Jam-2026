using ECS;
using Svelto.ECS;
using UnityEngine;

public class SlimeFlightSystem : ISystem, IQueryingEntitiesEngine
{
    public EntitiesDB entitiesDB { get; set; }

    private readonly RectTransformResourceManager rectTransformResourceManager;

    public SlimeFlightSystem(RectTransformResourceManager rectTransformResourceManager)
    {
        this.rectTransformResourceManager = rectTransformResourceManager;
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
                    Vector2 targetPosition = default;
                    entitiesDB.TryGetComponent(sb.PenId, PenGroupTag.Groups,
                        (ref RectPosition rp, ref RectBoundary rb) =>
                        {
                            var penExtents = 0.5f * new Vector2(rb.Width, rb.Height);
                            var xMin = rp.Value.x - penExtents.x + slimeExtents.x;
                            var xMax = rp.Value.x + penExtents.x - slimeExtents.x;
                            var yMin = rp.Value.y - penExtents.y + slimeExtents.y;
                            var yMax = rp.Value.y + penExtents.y - slimeExtents.y;
                            targetPosition = new(
                                Random.Range(xMin, xMax),
                                Random.Range(yMin, yMax)
                            );
                        });
                    sb.FlightTargetPosition = targetPosition;

                    sb.FlightRotationSpeed = Random.Range(200.0f, 400.0f);
                }

                var deltaTime = entitiesDB.GetSingletonComponent<UpdateDeltaTime>(UpdateDeltaTimeEntityDescriptor.Group).ValueSeconds;
                ref var time = ref sb.FlightAnimationTime;
                time += deltaTime;

                // Set flight time
                if (time > 2.0f)
                {
                    sb.MovementState = MovementState.Wander;
                    // Make slime angry
                    return;
                }

                var rt = rectTransformResourceManager[rtr.Id];
                rt.Rotate(0, 0, sb.FlightRotationSpeed * deltaTime);
            });
    }
}