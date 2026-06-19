using System;
using Svelto.ECS;

public static class ECSUtils
{
    public static bool IsValid(this EGID egid)
    {
        return egid.entityID != 0 && !egid.groupID.isInvalid;
    }

    public static void RunOnFilteredComponents<T>(this EntityFilterCollection filterCollection, EntitiesDB entitiesDB, QueryEgidCallback<T> callback)
        where T : unmanaged, IEntityComponent
    {
        foreach (var (fis, group) in filterCollection)
        {
            var (t, entityIds, _) = entitiesDB.QueryEntities<T>(group);
            for (var i = 0; i < fis.count; i++)
            {
                var fi = fis[i];
                callback?.Invoke(new(entityIds[i], group), ref t[fi]);
            }
        }
    }

    public static void RunOnFilteredComponents<T1, T2>(this EntityFilterCollection filterCollection, EntitiesDB entitiesDB, QueryEgidCallback<T1, T2> callback)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
    {
        foreach (var (fis, group) in filterCollection)
        {
            var (t1, t2, entityIds, _) = entitiesDB.QueryEntities<T1, T2>(group);
            for (var i = 0; i < fis.count; i++)
            {
                var fi = fis[i];
                callback?.Invoke(new(entityIds[i], group), ref t1[fi], ref t2[fi]);
            }
        }
    }

    public static void RunOnFilteredComponents<T1, T2, T3>(this EntityFilterCollection filterCollection, EntitiesDB entitiesDB, QueryEgidCallback<T1, T2, T3> callback)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
        where T3 : unmanaged, IEntityComponent
    {
        foreach (var (fis, group) in filterCollection)
        {
            var (t1, t2, t3, entityIds, _) = entitiesDB.QueryEntities<T1, T2, T3>(group);
            for (var i = 0; i < fis.count; i++)
            {
                var fi = fis[i];
                callback?.Invoke(new(entityIds[i], group), ref t1[fi], ref t2[fi], ref t3[fi]);
            }
        }
    }

    public static void RunOnFilteredComponents<T1, T2, T3, T4>(this EntityFilterCollection filterCollection, EntitiesDB entitiesDB, QueryEgidCallback<T1, T2, T3, T4> callback)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
        where T3 : unmanaged, IEntityComponent
        where T4 : unmanaged, IEntityComponent
    {
        foreach (var (fis, group) in filterCollection)
        {
            var (t1, t2, t3, t4, entityIds, _) = entitiesDB.QueryEntities<T1, T2, T3, T4>(group);
            for (var i = 0; i < fis.count; i++)
            {
                var fi = fis[i];
                callback?.Invoke(new(entityIds[i], group), ref t1[fi], ref t2[fi], ref t3[fi], ref t4[fi]);
            }
        }
    }

    public static void RunOnFilteredComponents<T>(this EntityFilterCollection filterCollection, EntitiesDB entitiesDB, ActionRef<T> action)
        where T : unmanaged, IEntityComponent
    {
        filterCollection.RunOnFilteredComponents(entitiesDB,
            (EGID _, ref T t) => action?.Invoke(ref t));
    }

    public static void RunOnFilteredComponents<T1, T2>(this EntityFilterCollection filterCollection, EntitiesDB entitiesDB, ActionRef<T1, T2> action)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
    {
        filterCollection.RunOnFilteredComponents(entitiesDB,
            (EGID _, ref T1 t1, ref T2 t2) => action?.Invoke(ref t1, ref t2));
    }

    public static void RunOnFilteredComponents<T1, T2, T3>(this EntityFilterCollection filterCollection, EntitiesDB entitiesDB, ActionRef<T1, T2, T3> action)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
        where T3 : unmanaged, IEntityComponent
    {
        filterCollection.RunOnFilteredComponents(entitiesDB,
            (EGID _, ref T1 t1, ref T2 t2, ref T3 t3) => action?.Invoke(ref t1, ref t2, ref t3));
    }

    public static void RunOnFilteredComponents<T1, T2, T3, T4>(this EntityFilterCollection filterCollection, EntitiesDB entitiesDB, ActionRef<T1, T2, T3, T4> action)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
        where T3 : unmanaged, IEntityComponent
        where T4 : unmanaged, IEntityComponent
    {
        filterCollection.RunOnFilteredComponents(entitiesDB,
            (EGID _, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4) => action?.Invoke(ref t1, ref t2, ref t3, ref t4));
    }
}