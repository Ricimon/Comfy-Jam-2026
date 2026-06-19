using DBC.Common;
using ECS;
using Svelto.ECS;
using UnityEngine;

public class GameObjectReferenceSystem : ISystem, IReactOnRemoveEx<GameObjectReference>
{
    private readonly ResourceManagers resourceManagers;

    public GameObjectReferenceSystem(ResourceManagers resourceManagers)
    {
        this.resourceManagers = resourceManagers;
    }

    public void Update()
    {
        
    }

    public void Remove((uint start, uint end) rangeOfEntities, in EntityCollection<GameObjectReference> entities, ExclusiveGroupStruct groupID)
    {
        var (gors, _) = entities;
        for (var i = rangeOfEntities.start; i < rangeOfEntities.end; i++)
        {
            ref var gor = ref gors[i];
            try
            {
                var go = gor.Id.ToObject(resourceManagers);
                resourceManagers.Remove<GameObject>(gor.Id);

                Object.Destroy(go);
            }
            catch (PreconditionException) { }
        }
    }
}