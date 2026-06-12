using UnityEngine;
using ECS;
using Svelto.ECS;

public class SlimeWanderSystem : ISystem, IQueryingEntitiesEngine
{
    public EntitiesDB entitiesDB { get; set; }

    public void Ready()
    {

    }

    public void Update()
    {
        var slimeBrainGroup = entitiesDB.FindGroups<SlimeBrain, Position, RectBoundary>();
        var slimeQuery = entitiesDB.QueryEntities<SlimeBrain, Position, RectBoundary>(slimeBrainGroup);
        var (c2, count2) = entitiesDB.QueryEntities<UpdateDeltaTime>(UpdateDeltaTimeEntityDescriptor.Group);
        var deltaTime = count2 > 0 ? c2[0].ValueSeconds : 0;

        var penQuery = entitiesDB.QueryEntities<RectBoundary, Position>(PenGroupTag.Groups);

        foreach (var ((brain, position, slimeBoundary, count), _ ) in slimeQuery)
        {
            for(var groupIdx = 0; groupIdx < count; groupIdx++)
            {
                var currentBrain = brain[groupIdx];
                if (currentBrain.MovementState != MovementState.Wander)
                    continue;

                Vector3 newPos = position[groupIdx].Value + new Vector3(1, 0, 0) * deltaTime;
                var slimeHalfW = slimeBoundary[groupIdx].Width / 2f;
                var slimeHalfH = slimeBoundary[groupIdx].Height / 2f;


                foreach (var((penBoundary, penPos, penCount), _) in penQuery)
                {
                    
                    for(var penIdx = 0; penIdx < penCount; penIdx++)
                    {

                        var localX = newPos.x - penPos[penIdx].Value.x;
                        var localY = newPos.y - penPos[penIdx].Value.y;

                        var penHalfW = penBoundary[penIdx].Width / 2f;
                        var penHalfH = penBoundary[penIdx].Height / 2f;

                        
                        if (localX + slimeHalfW >= penHalfW || localX - slimeHalfW <= -penHalfW)
                        {
                            Debug.Log("HIT X");
                            newPos.x = penPos[penIdx].Value.x + Mathf.Clamp(localX, -penHalfW + slimeHalfW, penHalfW - slimeHalfW);
                        }
                        if (localY + slimeHalfH >= penHalfH || localY - slimeHalfH <= -penHalfH)
                        {
                            Debug.Log("HIT Y");
                            newPos.y = penPos[penIdx].Value.y + Mathf.Clamp(localY, -penHalfH + slimeHalfH, penHalfH - slimeHalfH);
                        }
                    }
                }

                //Check if the new position is going to conflict with pen boundary


                position[groupIdx].Value = newPos;
            }
        }
    }

    
    
}
