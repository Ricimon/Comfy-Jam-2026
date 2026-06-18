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

        var (pause, scoreCount) = entitiesDB.QueryEntities<Pause>(GameStatTag.Group);
        if (pause[0].IsPaused)
            return;

        entitiesDB.QueryEntities<ElapsedTime>(groups)
            .Each((ref ElapsedTime elapsedTime)=>
            {
                elapsedTime.ValueSeconds += Time.deltaTime;
            });
    }
}
