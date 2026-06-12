using ECS;
using Svelto.ECS;

public class PositionSystem : ISystem, IQueryingEntitiesEngine
{
    public EntitiesDB entitiesDB { get; set; }

    private readonly GameObjectResourceManager gameObjectResourceManager;

    public PositionSystem(GameObjectResourceManager gameObjectResourceManager)
    {
        this.gameObjectResourceManager = gameObjectResourceManager;
    }

    public void Ready()
    {

    }

    public void Update()
    {
        var groups = entitiesDB.FindGroups<GameObjectReference, Position>();

        var g = entitiesDB.QueryEntities<GameObjectReference, Position>(groups);
        foreach(var ((goRefs, positions, count), _) in g)
        {
            for (var i = 0; i < count; i++)
            {
                var go = gameObjectResourceManager[goRefs[i].Id];

                if (go != null)
                {
                    go.transform.position = positions[i].Value;
                }
            }
        }
    }
}
