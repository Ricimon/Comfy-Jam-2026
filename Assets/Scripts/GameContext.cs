using ECS;
using UnityEngine;

public class GameContext
{
    public static World World;
    public static GameObjectResourceManager GameObjectResourceManager;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        World = new World();
        GameObjectResourceManager = new GameObjectResourceManager();

        // Systems
        World.AddSystem(new UpdateDeltaTimeSystem());
        World.AddSystem(new PositionSystem(GameObjectResourceManager));
        World.AddSystem(new InputSystem(GameObjectResourceManager));
        World.AddSystem(new SlimeSpawnerSystem());
        World.AddSystem(new SlimeWanderSystem());
    }
}