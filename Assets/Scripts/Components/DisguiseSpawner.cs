using Svelto.DataStructures.Experimental;
using Svelto.ECS;

public struct DisguiseSpawner : IEntityComponent
{
    public ValueIndex DisguiseDefaultPrefabId;
}

public class DisguiseSpawnerEntity : GenericEntityDescriptorAndGroup<
    DisguiseSpawner,
    GameObjectReference>
{ }