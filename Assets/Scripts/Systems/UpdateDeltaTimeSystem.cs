using ECS;
using Svelto.ECS;
using UnityEngine;

public class UpdateDeltaTimeSystem : ISystem, IQueryingEntitiesEngine
{
    public EntitiesDB entitiesDB { get; set; }

    public void Ready()
    {

    }

    public void Update()
    {
        var (pause, scoreCount) = entitiesDB.QueryEntities<Pause>(GameStatTag.Group);
        if (pause[0].IsPaused)
            return;

        var (udt, count) = entitiesDB.QueryEntities<UpdateDeltaTime>(UpdateDeltaTimeEntityDescriptor.Group);
        for (var i = 0; i < count; i++)
        {
            udt[i].ValueSeconds = Time.deltaTime;
        }
    }
}