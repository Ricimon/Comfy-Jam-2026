using System;
using Svelto.ECS;

public static class ECSUtils
{
    public static void RunOnFilteredComponents<T>(this EntityFilterCollection filterCollection, EntitiesDB entitiesDB, ActionRef<T> action) 
        where T : unmanaged, IEntityComponent
    {
        foreach(var (fis, group) in filterCollection)
        {
            var (component, _) = entitiesDB.QueryEntities<T>(group);
            for (var i = 0; i < fis.count; i++)
            {
                var fi = fis[i];
                action?.Invoke(ref component[fi]);
            }
        }
    }

    public static void Each<T>(this GroupsEnumerable<T> entities, ActionRef<T> action)
        where T : unmanaged, IEntityComponent
    {
        foreach(var ((t, count), _) in entities)
        {
            for (var i = 0; i < count; i++)
            {
                action?.Invoke(ref t[i]);
            }
        }
    }

    public static void Each<T1, T2>(this GroupsEnumerable<T1, T2> entities, ActionRef<T1, T2> action)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
    {
        foreach(var ((t1, t2, count), _) in entities)
        {
            for (var i = 0; i < count; i++)
            {
                action?.Invoke(ref t1[i], ref t2[i]);
            }
        }
    }

    public static void Each<T1, T2, T3>(this GroupsEnumerable<T1, T2, T3> entities, ActionRef<T1, T2, T3> action)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
        where T3 : unmanaged, IEntityComponent
    {
        foreach(var ((t1, t2, t3, count), _) in entities)
        {
            for (var i = 0; i < count; i++)
            {
                action?.Invoke(ref t1[i], ref t2[i], ref t3[i]);
            }
        }
    }
}