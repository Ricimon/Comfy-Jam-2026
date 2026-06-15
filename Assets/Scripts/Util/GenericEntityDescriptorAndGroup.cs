using Svelto.ECS;
using Svelto.ECS.Internal;

public class GenericEntityDescriptorAndGroup<T> : NamedExclusiveGroup<T>, IEntityDescriptor where T : struct, _IInternalEntityComponent 
{
    private static readonly IComponentBuilder[] _componentBuilders;

    public IComponentBuilder[] componentsToBuild => _componentBuilders;

    static GenericEntityDescriptorAndGroup()
    {
        _componentBuilders = new IComponentBuilder[1]
        {
            new ComponentBuilder<T>()
        };
    }
}

public class GenericEntityDescriptorAndGroup<T1, T2> : NamedExclusiveGroup<T1>, IEntityDescriptor
    where T1 : struct, IEntityComponent
    where T2 : struct, IEntityComponent
{
    private static readonly IComponentBuilder[] _componentBuilders;

    public IComponentBuilder[] componentsToBuild => _componentBuilders;

    static GenericEntityDescriptorAndGroup()
    {
        _componentBuilders = new IComponentBuilder[]
        {
            new ComponentBuilder<T1>(),
            new ComponentBuilder<T2>(),
        };
    }
}