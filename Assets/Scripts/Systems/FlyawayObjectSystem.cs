using ECS;
using Svelto.ECS;
using UnityEngine;

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
        var groups = entitiesDB.FindGroups<FlyawayObject, RectTransformReference, RectPosition>();
        entitiesDB.QueryEntities<FlyawayObject, RectTransformReference, RectPosition>(groups)
            .Each((EGID egid, ref FlyawayObject fo, ref RectTransformReference rtr, ref RectPosition rp) =>
            {
                if (!fo.IsActive) { return; }

                if (fo.AnimatingTIme == 0)
                {
                    // Set randomized parameters
                    var angle = Random.Range(30.0f, 150.0f) * Mathf.Deg2Rad;
                    var speed = Random.Range(200.0f, 500.0f);
                    fo.FlyVector = speed * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                    fo.RotationSpeed = Random.Range(200.0f, 400.0f);
                    fo.StartingPosition = rp.Value;
                }

                var deltaTime = entitiesDB.GetSingletonComponent<UpdateDeltaTime>(UpdateDeltaTimeEntity.Group).ValueSeconds;
                ref var time = ref fo.AnimatingTIme;
                time += deltaTime;

                if (time > 5.0f)
                {
                    world.RemoveEntity<BaseEntityDescriptor>(egid);
                    return;
                }

                var gravity = -1000.0f;

                var x = fo.StartingPosition.x + fo.FlyVector.x * time;
                var y = fo.StartingPosition.y + fo.FlyVector.y * time + 0.5f * gravity * time * time;
                rp.Value = new(x, y);

                var rt = resourceManagers.Get<RectTransform>(rtr.Id);
                if (fo.RotationFollowsPath)
                {
                    var movementVector = new Vector2(
                        fo.FlyVector.x,
                        fo.FlyVector.y + gravity * time
                    );
                    var angle = Vector2.SignedAngle(new Vector2(0, -1), movementVector);
                    rt.localRotation = Quaternion.Euler(0, 0, angle);
                }
                else
                {
                    rt.Rotate(0, 0, fo.RotationSpeed * deltaTime);
                }
            });
    }
}