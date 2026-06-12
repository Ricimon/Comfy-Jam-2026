using Svelto.ECS;

public class SlimeSpawnerEntityDescriptor : GenericEntityDescriptor<
    SlimeSpawner>
{ }

public class SlimeSpawnerGroup : NamedExclusiveGroup<SlimeSpawnerGroup> { }