using ECS;
using Svelto.ECS;
using UnityEngine;
using UnityEngine.InputSystem;
using static ECSUtils;

public class SlimeWanderSystem : ISystem, IQueryingEntitiesEngine
{
    public EntitiesDB entitiesDB { get; set; }

    private const float WANDER_SPEED = 200f;
    private const float BOUNCE_OFFSET = .3f;
    public void Ready()
    {

    }

    public void Update()
    {
        var slimeBrainGroup = entitiesDB.FindGroups<SlimeBrain, RectPosition, RectBoundary, Direction>();
        var slimeQuery = entitiesDB.QueryEntities<SlimeBrain, RectPosition, RectBoundary, Direction>(slimeBrainGroup);
        var (c2, count2) = entitiesDB.QueryEntities<UpdateDeltaTime>(UpdateDeltaTimeEntityDescriptor.Group);
        var deltaTime = count2 > 0 ? c2[0].ValueSeconds : 0;

        var penQuery = entitiesDB.QueryEntities<RectBoundary, RectPosition>(PenGroupTag.Groups);

        uint mainPenId = GetMainPenEGID();

        foreach (var ((brain, position, slimeBoundary, direction, count), _ ) in slimeQuery)
        {
            for(var groupIdx = 0; groupIdx < count; groupIdx++)
            {

                if (brain[groupIdx].MovementState != MovementState.Wander)
                    continue;

                if(brain[groupIdx].PenId == default)
                    brain[groupIdx].PenId = mainPenId;

                Vector3 newPos = position[groupIdx].Value + direction[groupIdx].Value * deltaTime * WANDER_SPEED;

                var slimeHalfW = slimeBoundary[groupIdx].Width / 2f;
                var slimeHalfH = slimeBoundary[groupIdx].Height / 2f;

                foreach (var ((penBoundary, penPos, penIds, penCount), group) in penQuery)
                {
                    for (var penIdx = 0; penIdx < penCount; penIdx++)
                    {
                        if (brain[groupIdx].PenId != penIds[penIdx])
                            continue;

                        var localX = newPos.x - penPos[penIdx].Value.x;
                        var localY = newPos.y - penPos[penIdx].Value.y;

                        var penHalfW = penBoundary[penIdx].Width / 2f;
                        var penHalfH = penBoundary[penIdx].Height / 2f;


                        if (localX + slimeHalfW >= penHalfW || localX - slimeHalfW <= -penHalfW)
                        {
                            var dir = direction[groupIdx].Value;
                            dir.x = -dir.x + Random.Range(0, BOUNCE_OFFSET);
                            direction[groupIdx].Value = dir.normalized;
                            newPos.x = penPos[penIdx].Value.x + Mathf.Clamp(localX, -penHalfW + slimeHalfW, penHalfW - slimeHalfW);
                        }
                        if (localY + slimeHalfH >= penHalfH || localY - slimeHalfH <= -penHalfH)
                        {
                            var dir = direction[groupIdx].Value;
                            dir.y = -dir.y + Random.Range(-BOUNCE_OFFSET, 0);
                            direction[groupIdx].Value = dir.normalized;
                            newPos.y = penPos[penIdx].Value.y + Mathf.Clamp(localY, -penHalfH + slimeHalfH, penHalfH - slimeHalfH);
                        }

                    }
                }

                position[groupIdx].Value = newPos;
                
            }
        }
    }
    
    private uint GetMainPenEGID()
    {
        var mainPenQuery = entitiesDB.QueryEntities<RectBoundary>(MainPenGroupTag.Groups);
        foreach (var ((boundaries, ids, count), group) in mainPenQuery)
        {
            if (count > 0)
            {
                return ids[0];
            }
        }
        return 0;
    }

    
    
}
