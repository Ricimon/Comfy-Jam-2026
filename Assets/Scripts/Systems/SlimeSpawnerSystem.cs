using ECS;
using Svelto.ECS;
using UnityEngine;

public class SlimeSpawnerSystem : ISystem, IQueryingEntitiesEngine
{
    public EntitiesDB entitiesDB { get; set; }

    private readonly AnimationCurveResourceManager animationCurveResourceManager;
    private readonly DisguiseType[] possibleDisguises = new[]
    {
        DisguiseType.Default,
        DisguiseType.YellowHoodie,
        DisguiseType.BlueHoodie,
    };

    public SlimeSpawnerSystem(AnimationCurveResourceManager animationCurveResourceManager)
    {
        this.animationCurveResourceManager = animationCurveResourceManager;
    }

    public static void DisposeAllSlimes(World world)
    {
        world.RemoveEntitiesFromGroup(SlimeGroup.BuildGroup);
    }

    public static EntityInitializer SpawnSlime(World world, SlimeColor slimeColor, DisguiseType disguise = DisguiseType.None)
    {
        var slimeEntity = world.Entity<SlimeEntityDescriptor>(SlimeGroup.BuildGroup);

        EntityInitializer disguiseEntity = default;
        if (disguise != DisguiseType.None)
        {
            disguiseEntity = world.Entity<DisguiseEntity>(DisguiseEntity.Group);

            disguiseEntity.Init(new Disguise
            {
                Type = disguise,
                SlimeId = slimeEntity.EGID,
            });
        }

        slimeEntity.Init(new Slime
        {
            CanPickUp = true,
            SlimeColor = slimeColor,
            DisguiseId = disguiseEntity.EGID,
        });

        // Spawn in main pen
        EGID penId = default;
        world.EntitiesDB.QueryEntities<RectPosition>(MainPenGroup.Groups)
            .Each((EGID egid, ref RectPosition _) =>
            {
                penId = egid;
            });

        slimeEntity.Init(new SlimeBrain
        {
            MovementState = MovementState.Wander,
            PenId = penId,
            RandomizePositionInPen = true,
        });

        slimeEntity.Init(new Direction
        {
            Value = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized
        });

        return slimeEntity;
    }

    public void Ready()
    {
    }

    public void Update()
    {
        var (c1, count1) = entitiesDB.QueryEntities<SlimeSpawner>(SlimeSpawnerGroup.Group);
        var (c2, count2) = entitiesDB.QueryEntities<UpdateDeltaTime>(UpdateDeltaTimeEntity.Group);
        var deltaTime = count2 > 0 ? c2[0].ValueSeconds : 0;
        var (elapsedTime, count) = entitiesDB.QueryEntities<ElapsedTime>(GameStatTag.Group);



        for (var i = 0; i < count1; i++)
        {
            ref var spawner = ref c1[i];
            spawner.TimeUntilSpawn -= deltaTime;


            if (spawner.TimeUntilSpawn <= 0)
            {
                var spawnRateCurve = animationCurveResourceManager[spawner.SpawnRateCurveId];
                var slimeColor = Random.value < 0.5f ? SlimeColor.Blue : SlimeColor.Yellow;
                var hasDisguise = Random.value < 0.5f;
                var disguise = DisguiseType.None;
                if (hasDisguise)
                {
                    disguise = possibleDisguises[Random.Range(0, possibleDisguises.Length)];
                }
                SpawnSlime(GameContext.World, slimeColor, disguise);
                spawner.TimeUntilSpawn = spawnRateCurve.Evaluate(elapsedTime[0].ValueSeconds);
            }
        }
    }
}