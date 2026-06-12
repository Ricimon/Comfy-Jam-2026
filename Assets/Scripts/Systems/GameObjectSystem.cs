using ECS;
using Svelto.ECS;

public class GameObjectSystem : ISystem, IReactOnRemoveEx<GameObjectReference>
{
    public void Update()
    {
        
    }

    public void Remove((uint start, uint end) rangeOfEntities, in EntityCollection<GameObjectReference> entities, ExclusiveGroupStruct groupID)
    {
        throw new System.NotImplementedException();
    }
}