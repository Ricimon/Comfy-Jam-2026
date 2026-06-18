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

            SlimeDroplet sdCopy = sd;
            entitiesDB.TryGetComponent(id, groupID,
                (ref GameObjectReference gor, ref RectTransformReference rtr) =>
                {
                    if (!resourceManagers.TryGet<GameObject>(gor.Id, out var go))
                    {
                        var spawner = entitiesDB.GetSingletonComponent<SlimeDropletSpawner>(SlimeDropletSpawnerEntity.Group);
                        var gameCanvas = entitiesDB.GetSingletonComponent<GameCanvas>(CanvasGroup.Group);
                        var prefab = resourceManagers.Get<GameObject>(spawner.DropletPrefabId);
                        var parent = resourceManagers.Get<GameObject>(gameCanvas.SlimesParentGoId);
                        go = Object.Instantiate(prefab, parent.transform);
                        go.transform.SetSiblingIndex(sdCopy.TransformSiblingIndex);
                        gor.Id = resourceManagers.Add(go);
                        rtr.Id = resourceManagers.Add(go.GetComponent<RectTransform>());
                    }
                    if (go.TryGetComponent(out Image image))
                    {
                        image.color = sdCopy.Color;
                    }
                });

            entitiesDB.TryGetComponent(id, groupID,
                (ref FlyawayObject fo, ref RectPosition rp) =>
                {
                    fo.IsActive = true;
                    fo.RotationFollowsPath = true;
                    fo.StartingPosition = rp.Value;
                });
        }
    }
}