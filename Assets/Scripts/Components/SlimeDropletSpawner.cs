using Svelto.DataStructures.Experimental;
using Svelto.ECS;

public struct SlimeDropletSpawner : IEntityComponent
{
    public ValueIndex DropletPrefabId;
}

public class SlimeDropletSpawnerEntity : GenericEntityDescriptorAndGroup<SlimeDropletSpawner> {}