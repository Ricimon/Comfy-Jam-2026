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