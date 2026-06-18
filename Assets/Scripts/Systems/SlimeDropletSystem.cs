using ECS;
using Svelto.ECS;
using UnityEngine;
using UnityEngine.UI;

public class SlimeDropletSystem : ISystem, IQueryingEntitiesEngine, IReactOnAddEx<SlimeDroplet>
{
    public EntitiesDB entitiesDB { get; set; }

    private readonly ResourceManagers resourceManagers;

    public SlimeDropletSystem(ResourceManagers resourceManagers)
    {
        this.resourceManagers = resourceManagers;
    }

    public void Ready()
    {
    }

    public void Update()
    {
    }

    public void Add((uint start, uint end) rangeOfEntities, in EntityCollection<SlimeDroplet> entities,
        ExclusiveGroupStruct groupID)
    {
        var (slimeDroplets, entityIDs, _) = entities;
        for (var i = rangeOfEntities.start; i < rangeOfEntities.end; i++)
        {
            var id = entityIDs[i];
            ref var sd = ref slimeDroplets[i];

            var color = sd.Color;
            entitiesDB.TryGetComponent(id, groupID,
                (ref GameObjectReference gor) =>
                {
                    var go = resourceManagers.Get<GameObject>(gor.Id);
                    if (go.TryGetComponent(out Image image))
                    {
                        image.color = color;
                    }
                });

            entitiesDB.TryGetComponent(id, groupID,
                (ref FlyawayObject fo) =>
                {
                    fo.RotationFollowsPath = true;
                });
        }
    }
}