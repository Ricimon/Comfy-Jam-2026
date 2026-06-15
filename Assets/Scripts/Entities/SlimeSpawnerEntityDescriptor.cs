using Svelto.ECS;

public class SlimeSpawnerEntityDescriptor : GenericEntityDescriptor<
    SlimeSpawner,
    GameObjectReference>
{ }

public class SlimeSpawnerGroup : NamedExclusiveGroup<SlimeSpawnerGroup> { }