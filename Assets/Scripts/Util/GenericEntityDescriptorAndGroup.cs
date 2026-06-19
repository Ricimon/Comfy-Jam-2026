using ECS;
using Svelto.ECS;
using Svelto.ECS.Internal;

public class GenericEntityDescriptorAndGroup<T> : ExtendibleEntityDescriptor<BaseEntityDescriptor>
    where T : struct, _IInternalEntityComponent 
{
    public static ExclusiveGroup Group = new();
    public GenericEntityDescriptorAndGroup()
    {
        ExtendWith(new IComponentBuilder[]
        {
            new ComponentBuilder<T>()
        });
    }
}

public class GenericEntityDescriptorAndGroup<T1, T2> : ExtendibleEntityDescriptor<BaseEntityDescriptor>
    where T1 : struct, IEntityComponent
    where T2 : struct, IEntityComponent
{
    public static ExclusiveGroup Group = new();
    public GenericEntityDescriptorAndGroup()
    {
        ExtendWith(new IComponentBuilder[]
        {
            new ComponentBuilder<T1>(),
            new ComponentBuilder<T2>(),
        });
    }
}

public class GenericEntityDescriptorAndGroup<T1, T2, T3> : ExtendibleEntityDescriptor<BaseEntityDescriptor>
    where T1 : struct, IEntityComponent
    where T2 : struct, IEntityComponent
    where T3 : struct, IEntityComponent
{
    public static ExclusiveGroup Group = new();
    public GenericEntityDescriptorAndGroup()
    {
        ExtendWith(new IComponentBuilder[]
        {
            new ComponentBuilder<T1>(),
            new ComponentBuilder<T2>(),
            new ComponentBuilder<T3>(),
        });
    }
}

public class GenericEntityDescriptorAndGroup<T1, T2, T3, T4> : ExtendibleEntityDescriptor<BaseEntityDescriptor>
    where T1 : struct, IEntityComponent
    where T2 : struct, IEntityComponent
    where T3 : struct, IEntityComponent
    where T4 : struct, IEntityComponent
{
    public static ExclusiveGroup Group = new();
    public GenericEntityDescriptorAndGroup()
    {
        ExtendWith(new IComponentBuilder[]
        {
            new ComponentBuilder<T1>(),
            new ComponentBuilder<T2>(),
            new ComponentBuilder<T3>(),
            new ComponentBuilder<T4>(),
        });
    }
}

public class GenericEntityDescriptorAndGroup<T1, T2, T3, T4, T5> : ExtendibleEntityDescriptor<BaseEntityDescriptor>
    where T1 : struct, IEntityComponent
    where T2 : struct, IEntityComponent
    where T3 : struct, IEntityComponent
    where T4 : struct, IEntityComponent
    where T5 : struct, IEntityComponent
{
    public static ExclusiveGroup Group = new();
    public GenericEntityDescriptorAndGroup()
    {
        ExtendWith(new IComponentBuilder[]
        {
            new ComponentBuilder<T1>(),
            new ComponentBuilder<T2>(),
            new ComponentBuilder<T3>(),
            new ComponentBuilder<T4>(),
            new ComponentBuilder<T5>(),
        });
    }
}