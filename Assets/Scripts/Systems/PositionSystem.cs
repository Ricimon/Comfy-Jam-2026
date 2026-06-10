using ECS;
using Svelto.ECS;

public class PositionSystem : ISystem, IQueryingEntitiesEngine
{
    public EntitiesDB entitiesDB { get; set; }

    public void Ready()
    {

    }

    public void Update()
    {
        var (goRefs, positions, count) = entitiesDB.QueryEntities<GameObjectReference, Position>(World.DefaultGroup);
        for (var i = 0; i < count; i++)
        {
            var go = GameContext.GameObjectResourceManager[goRefs[i].Id];

            if (go != null)
            {
                go.transform.position = positions[i].Value;
            }
        }
    }
}
