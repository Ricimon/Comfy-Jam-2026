using ECS;
using Svelto.ECS;

public class FlyawayObjectSystem : ISystem, IQueryingEntitiesEngine
{
    public EntitiesDB entitiesDB { get; set; }

    private readonly World world;
    private readonly ResourceManagers resourceManagers;

    public FlyawayObjectSystem(World world, ResourceManagers resourceManagers)
    {
        this.world = world;
        this.resourceManagers = resourceManagers;
    }

    public void Ready()
    {
    }

    public void Update()
    {
        // var groups = entitiesDB.FindGroups<FlyawayObject, RectTransformReference, RectPosition>();
        // entitiesDB.QueryEntities<FlyawayObject, RectTransformReference, RectPosition>(groups)
        //     .Each((uint id, ref FlyawayObject fo, ref RectTransformReference rtr, ref RectPosition rp) =>
        //     {
        //         if (!fo.ShouldRemove) { return; }

        //         if (fo.SlimeId.IsValid())
        //         {
        //             var slimeGor = entitiesDB.GetComponent<GameObjectReference>(fo.SlimeId);
        //             var slimeRp = entitiesDB.GetComponent<RectPosition>(fo.SlimeId);
        //             var slimeGo = gameObjectResourceManager[slimeGor.Id];

        //             var go = gameObjectResourceManager[gor.Id];

        //             // Remove from Slime
        //             fo.SlimeId = default;

        //             go.transform.SetParent(slimeGo.transform.parent, true);
        //             go.transform.SetSiblingIndex(slimeGo.transform.GetSiblingIndex() + 1);

        //             // Set randomized parameters
        //             var angle = Random.Range(30.0f, 150.0f) * Mathf.Deg2Rad;
        //             var speed = Random.Range(200.0f, 500.0f);
        //             fo.RemovalFlyVector = speed * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        //             fo.RemovalRotationSpeed = Random.Range(200.0f, 400.0f);
        //             fo.RemovalStartingPosition = slimeRp.Value;
        //         }

        //         var deltaTime = entitiesDB.GetSingletonComponent<UpdateDeltaTime>(UpdateDeltaTimeEntityDescriptor.Group).ValueSeconds;
        //         ref var time = ref fo.RemovalAnimatingTime;
        //         time += deltaTime;

        //         if (time > 5.0f)
        //         {
        //             world.RemoveEntity<DisguiseEntity>(id, DisguiseEntity.Group);
        //             return;
        //         }

        //         var x = fo.RemovalStartingPosition.x + fo.RemovalFlyVector.x * time;
        //         var y = fo.RemovalStartingPosition.y + fo.RemovalFlyVector.y * time + 0.5f * -1000.0f * time * time;
        //         rp.Value = new(x, y);

        //         var rt = rectTransformResourceManager[rtr.Id];
        //         rt.Rotate(0, 0, fo.RemovalRotationSpeed * deltaTime);
        //     });
    }
}