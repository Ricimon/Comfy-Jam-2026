using DBC.Common;
using ECS;
using Svelto.ECS;
using UnityEngine;

public class SlimeSystem : ISystem, IQueryingEntitiesEngine, IReactOnAddEx<GameObjectReference>
{
    public EntitiesDB entitiesDB { get; set; }

    private readonly GameObjectResourceManager gameObjectResourceManager;
    private readonly RectTransformResourceManager rectTransformResourceManager;

    public SlimeSystem(
        GameObjectResourceManager gameObjectResourceManager,
        RectTransformResourceManager rectTransformResourceManager)
    {
        this.gameObjectResourceManager = gameObjectResourceManager;
        this.rectTransformResourceManager = rectTransformResourceManager;
    }

    public void Ready()
    {
    }

    public void Update()
    {
        var count = 0;
        entitiesDB.QueryEntities<GameObjectReference>(SlimeGroup.Groups)
            .Each((ref GameObjectReference gor) =>
            {
                count++;
            });
        Debug.Log($"Found {count} gor components");
    }

    public void Add((uint start, uint end) rangeOfEntities, in EntityCollection<GameObjectReference> entities, ExclusiveGroupStruct groupID)
    {
        if (!groupID.FoundIn(SlimeGroup.Groups)) { return; }

        var (gors, entityIds, _) = entities;
        for (var i = rangeOfEntities.start; i < rangeOfEntities.end; i++)
        {
            ref var gor = ref gors[i];

            try
            {
                _ = gameObjectResourceManager[gor.Id];
            }
            catch (PreconditionException)
            {
                // Create GameObject
                if (!entitiesDB.TryGetSingletonComponent<SlimeSpawner>(SlimeSpawnerGroup.Group, out var slimeSpawner))
                {
                    return;
                }
                if (!entitiesDB.TryGetSingletonComponent<GameObjectReference>(CanvasGroup.Group, out var canvasGor))
                {
                    return;
                }
                var prefab = gameObjectResourceManager[slimeSpawner.SlimePrefabId];
                var parent = gameObjectResourceManager[canvasGor.Id];
                var go = Object.Instantiate(prefab, parent.transform);
                var goId = gameObjectResourceManager.Add(go);
                var rtId = rectTransformResourceManager.Add(go.GetComponent<RectTransform>());
                gor.Id = goId;
                if (entitiesDB.TryGetEntity<RectTransformReference>(i, groupID, out var rtr))
                {
                    Debug.Log("Assign rtr");
                    rtr.Id = rtId;
                }
            }
        }
    }
}