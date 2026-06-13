using System;
using Svelto.DataStructures;
using Svelto.ECS;

public static class ECSUtils
{
    public static void RunOnFilteredComponents<T>(this EntityFilterCollection filterCollection, EntitiesDB entitiesDB, ActionRef<T> action)
        where T : unmanaged, IEntityComponent
    {
        foreach (var (fis, group) in filterCollection)
        {
            var (component, _) = entitiesDB.QueryEntities<T>(group);
            for (var i = 0; i < fis.count; i++)
            {
                var fi = fis[i];
                action?.Invoke(ref component[fi]);
            }
        }
    }

    public static void RunOnFilteredComponents<T1, T2>(this EntityFilterCollection filterCollection, EntitiesDB entitiesDB, ActionRef<T1, T2> action)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
    {
        foreach (var (fis, group) in filterCollection)
        {
            var (t1, t2, _) = entitiesDB.QueryEntities<T1, T2>(group);
            for (var i = 0; i < fis.count; i++)
            {
                var fi = fis[i];
                action?.Invoke(ref t1[fi], ref t2[fi]);
            }
        }
    }

    public static void RunOnFilteredComponents<T1, T2, T3>(this EntityFilterCollection filterCollection, EntitiesDB entitiesDB, ActionRef<T1, T2,T3> action)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
        where T3 : unmanaged, IEntityComponent
    {
        foreach (var (fis, group) in filterCollection)
        {
            var (t1, t2, t3,_) = entitiesDB.QueryEntities<T1, T2, T3>(group);
            for (var i = 0; i < fis.count; i++)
            {
                var fi = fis[i];
                action?.Invoke(ref t1[fi], ref t2[fi], ref t3[fi]);
            }
        }
    }

    public static bool TryGetSingletonComponent<T>(this EntitiesDB entitiesDB, ExclusiveGroupStruct groupStructId, out T component)
        where T : unmanaged, IEntityComponent
    {
        var (t, count) = entitiesDB.QueryEntities<T>(groupStructId);
        if (count == 0)
        {
            component = default;
            return false;
        }
        component = t[0];
        return true;
    }

    public static bool TryGetComponent<T>(this EntitiesDB entitiesDB, uint id, ExclusiveGroupStruct group, ActionRef<T> action)
        where T : unmanaged, IEntityComponent
    {
        if (entitiesDB.TryQueryEntitiesAndIndex(id, group, out var i, out NB<T> t))
        {
            action?.Invoke(ref t[i]);
            return true;
        }
        return false;
    }
}