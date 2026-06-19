using ECS;
using UnityEngine;

public class GameContext
{
    public static World World;
    public static ResourceManagers ResourceManagers;
    public static GameObjectResourceManager GameObjectResourceManager;
    public static RectTransformResourceManager RectTransformResourceManager;
    public static AnimationCurveResourceManager AnimationCurveResourceManager;
    public static AudioClipResourceManager AudioClipResourceManager;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        World = new World();
        GameObjectResourceManager = new();
        RectTransformResourceManager = new();
        AnimationCurveResourceManager = new();
        ResourceManagers = new();
        AudioClipResourceManager = new();
        ResourceManagers.AddPrebuiltResourceManager<GameObject>(GameObjectResourceManager);
        ResourceManagers.AddPrebuiltResourceManager<RectTransform>(RectTransformResourceManager);
        ResourceManagers.AddPrebuiltResourceManager<AnimationCurve>(AnimationCurveResourceManager);
        ResourceManagers.AddPrebuiltResourceManager<AudioClip>(AudioClipResourceManager);

        //Entities
        var gameStatEntity = World.Entity<GameStatDescriptor>(GameStatTag.Group);
        gameStatEntity.Init(new Score
        {
            Value = 0
        });
        gameStatEntity.Init(new Lives
        {
            Value = 4
        });
        gameStatEntity.Init(new ElapsedTime { ValueSeconds = 0 });
        gameStatEntity.Init(new Pause { IsPaused = true });



        // Systems
        World.AddSystem(new GameObjectReferenceSystem(GameObjectResourceManager));
        World.AddSystem(new RectTransformReferenceSystem(RectTransformResourceManager));
        World.AddSystem(new UpdateDeltaTimeSystem());
        World.AddSystem(new ElapsedTimeSystem());
        World.AddSystem(new GameStatSystem());
        World.AddSystem(new RectPositionSystem(RectTransformResourceManager));
        World.AddSystem(new InputSystem(ResourceManagers));
        World.AddSystem(new SlimeSpawnerSystem(AnimationCurveResourceManager));
        World.AddSystem(new SlimeSystem(World, ResourceManagers));
        World.AddSystem(new SlimeWanderSystem());
        World.AddSystem(new SlimeFlightSystem(RectTransformResourceManager, AnimationCurveResourceManager));
        World.AddSystem(new SlimeDropletSystem(ResourceManagers));
        World.AddSystem(new DisguiseSystem(World, ResourceManagers));
        World.AddSystem(new FlyawayObjectSystem(World, ResourceManagers));
        
    }
}