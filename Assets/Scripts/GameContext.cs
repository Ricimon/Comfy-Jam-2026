using ECS;
using UnityEngine;

public class GameContext
{
    public static World World;
    public static GameObjectResourceManager GameObjectResourceManager;
    public static RectTransformResourceManager RectTransformResourceManager;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        World = new World();
        GameObjectResourceManager = new ();
        RectTransformResourceManager = new();

        //Entities
        var gameStatEntity = World.Entity<GameStatDescriptor>(GameStatTag.Group);
        gameStatEntity.Init(new Score
        {
            Value = 0
        });
        gameStatEntity.Init(new Lives
        {
            Value = 3
        });


        // Systems
        World.AddSystem(new GameObjectReferenceSystem(GameObjectResourceManager));
        World.AddSystem(new RectTransformReferenceSystem(RectTransformResourceManager));
        World.AddSystem(new UpdateDeltaTimeSystem());
        World.AddSystem(new GameStatSystem());
        World.AddSystem(new RectPositionSystem(RectTransformResourceManager));
        World.AddSystem(new InputSystem(GameObjectResourceManager));
        World.AddSystem(new SlimeSpawnerSystem());
        World.AddSystem(new SlimeSystem(GameObjectResourceManager, RectTransformResourceManager));
        World.AddSystem(new SlimeWanderSystem());
        World.AddSystem(new DisguiseSystem(World, GameObjectResourceManager, RectTransformResourceManager));
    }
}