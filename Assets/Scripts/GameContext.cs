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

        // Systems
        World.AddSystem(new UpdateDeltaTimeSystem());
        World.AddSystem(new RectPositionSystem(RectTransformResourceManager));
        World.AddSystem(new InputSystem(GameObjectResourceManager));
        World.AddSystem(new SlimeSpawnerSystem());
        World.AddSystem(new SlimeSystem(GameObjectResourceManager, RectTransformResourceManager));
    }
}