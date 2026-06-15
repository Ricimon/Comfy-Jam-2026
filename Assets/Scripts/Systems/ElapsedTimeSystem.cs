using ECS;
using Svelto.ECS;
using UnityEngine;

public class ElapsedTimeSystem : ISystem, IQueryingEntitiesEngine
{
    public EntitiesDB entitiesDB { get; set; }

    public void Ready()
    {
    }

    public void Update()
    {
        var groups = entitiesDB.FindGroups<ElapsedTime>();
        entitiesDB.QueryEntities<ElapsedTime>(groups)
            .Each((ref ElapsedTime elapsedTime)=>
            {
                elapsedTime.ValueSeconds += Time.deltaTime;
            });
        var (score, count) = GameContext.World.EntitiesDB.QueryEntities<Score>(GameStatTag.Group);
    }
}
