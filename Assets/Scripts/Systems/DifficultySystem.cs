using ECS;
using Svelto.ECS;
using UnityEngine;


public class DifficultySystem : ISystem, IQueryingEntitiesEngine
{
    public EntitiesDB entitiesDB { get; set; }

    public void Ready()
    {
        
    }

    public void Update()
    {
        var (c1, count1) = entitiesDB.QueryEntities<SlimeSpawner>(SlimeSpawnerGroup.Group);
        var (c2, count2) = entitiesDB.QueryEntities<UpdateDeltaTime>(UpdateDeltaTimeEntityDescriptor.Group);
        var deltaTime = count2 > 0 ? c2[0].ValueSeconds : 0;



        
    }
}
