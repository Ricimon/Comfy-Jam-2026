using ECS;
using Svelto.ECS;
using UnityEngine;

public class SlimeWanderSystem : ISystem, IQueryingEntitiesEngine
{
    public EntitiesDB entitiesDB { get; set; }

    private const float WANDER_SPEED = 200f;
    private const float BOUNCE_OFFSET = .3f;

    private readonly ResourceManagers resourceManagers;

    public SlimeWanderSystem(ResourceManagers resourceManagers)
    {
        this.resourceManagers = resourceManagers;
    }
    public void Ready()
    {

    }

    public void Update()
    {
        var deltaTime = entitiesDB.GetSingletonComponent<UpdateDeltaTime>(UpdateDeltaTimeEntity.Group).ValueSeconds;
        var movementCurve = entitiesDB.GetSingletonComponent<SlimeConfig>(SlimeConfigEntity.Group)
            .MovementCurveId.ToObject(resourceManagers);

        EGID mainPenId = GetMainPenEGID();

        var slimeBrainGroup = entitiesDB.FindGroups<SlimeBrain, RectPosition, RectBoundary, Direction>();
        entitiesDB.QueryEntities<SlimeBrain, RectPosition, RectBoundary, Direction>(slimeBrainGroup)
            .Each((ref SlimeBrain brain, ref RectPosition position, ref RectBoundary slimeBoundary, ref Direction direction) =>
            {
                if (brain.MovementState != MovementState.Wander)
                    return;

                if (brain.PenId == default)
                    brain.PenId = mainPenId;

                var walkCycleDuration = 0.8f;
                var wanderSpeed = WANDER_SPEED;
                if (brain.IsSpeedUp)
                {
                    wanderSpeed *= 2.0f;
                    walkCycleDuration *= 0.5f;
                }

                brain.WalkCycleTime += deltaTime;
                while (brain.WalkCycleTime > walkCycleDuration && walkCycleDuration > 0)
                {
                    brain.WalkCycleTime -= walkCycleDuration;
                    brain.WanderSpeedMultiplierThisCycle = 0;
                }
                if (brain.WanderSpeedMultiplierThisCycle == 0)
                {
                    // brain.WanderSpeedMultiplierThisCycle = Random.Range(0.1f, 1.2f);
                    brain.WanderSpeedMultiplierThisCycle = 1.0f;
                }

                wanderSpeed *= brain.WanderSpeedMultiplierThisCycle * movementCurve.Evaluate(brain.WalkCycleTime / walkCycleDuration);
                Vector3 newPos = position.Value + direction.Value * deltaTime * wanderSpeed;

                var slimeHalfW = slimeBoundary.Width / 2f;
                var slimeHalfH = slimeBoundary.Height / 2f;

                var penQuery = entitiesDB.QueryEntities<RectBoundary, RectPosition>(PenGroupTag.Groups);
                var penBoundary = entitiesDB.QueryEntity<RectBoundary>(brain.PenId);
                var penPos = entitiesDB.QueryEntity<RectPosition>(brain.PenId);

                var localX = newPos.x - penPos.Value.x;
                var localY = newPos.y - penPos.Value.y;

                var penHalfW = penBoundary.Width / 2f;
                var penHalfH = penBoundary.Height / 2f;


                if (localX + slimeHalfW >= penHalfW || localX - slimeHalfW <= -penHalfW)
                {
                    var dir = direction.Value;
                    dir.x = -dir.x + Random.Range(0, BOUNCE_OFFSET);
                    direction.Value = dir.normalized;
                    newPos.x = penPos.Value.x + Mathf.Clamp(localX, -penHalfW + slimeHalfW, penHalfW - slimeHalfW);
                }
                if (localY + slimeHalfH >= penHalfH || localY - slimeHalfH <= -penHalfH)
                {
                    var dir = direction.Value;
                    dir.y = -dir.y + Random.Range(-BOUNCE_OFFSET, 0);
                    direction.Value = dir.normalized;
                    newPos.y = penPos.Value.y + Mathf.Clamp(localY, -penHalfH + slimeHalfH, penHalfH - slimeHalfH);
                }

                position.Value = newPos;
            });
    }

    private EGID GetMainPenEGID()
    {
        var mainPenQuery = entitiesDB.QueryEntities<RectBoundary>(MainPenGroupTag.Groups);
        foreach (var ((boundaries, ids, count), group) in mainPenQuery)
        {
            if (count > 0)
            {
                return new(ids[0], group);
            }
        }
        return default;
    }
}
