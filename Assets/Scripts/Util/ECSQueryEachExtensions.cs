using Svelto.ECS;

public static class ECSQueryEachExtensions
{
    public delegate void QueryIndexCallback<T>(uint i, ref T t);
    public delegate void QueryIndexCallback<T1, T2>(uint i, ref T1 t1, ref T2 t2);
    public delegate void QueryIndexCallback<T1, T2, T3>(uint i, ref T1 t1, ref T2 t2, ref T3 t3);
    public delegate void QueryIndexCallback<T1, T2, T3, T4>(uint i, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4);

    //
    // Entity Collection
    //
    public static void Each<T>(this EntityCollection<T> entities, QueryIndexCallback<T> callback)
        where T : unmanaged, IEntityComponent
    {
        var (t1, id, count) = entities;
        for (var i = 0; i < count; i++)
        {
            callback?.Invoke(id[i], ref t1[i]);
        }
    }

    public static void Each<T1, T2>(this EntityCollection<T1, T2> entities, QueryIndexCallback<T1, T2> callback)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
    {
        var (t1, t2, id, count) = entities;
        for (var i = 0; i < count; i++)
        {
            callback?.Invoke(id[i], ref t1[i], ref t2[i]);
        }
    }

    public static void Each<T1, T2, T3>(this EntityCollection<T1, T2, T3> entities, QueryIndexCallback<T1, T2, T3> callback)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
        where T3 : unmanaged, IEntityComponent
    {
        var (t1, t2, t3, id, count) = entities;
        for (var i = 0; i < count; i++)
        {
            callback?.Invoke(id[i], ref t1[i], ref t2[i], ref t3[i]);
        }
    }

    public static void Each<T1, T2, T3, T4>(this EntityCollection<T1, T2, T3, T4> entities, QueryIndexCallback<T1, T2, T3, T4> callback)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
        where T3 : unmanaged, IEntityComponent
        where T4 : unmanaged, IEntityComponent
    {
        var (t1, t2, t3, t4, id, count) = entities;
        for (var i = 0; i < count; i++)
        {
            callback?.Invoke(id[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i]);
        }
    }

    public static void Each<T>(this EntityCollection<T> entities, ActionRef<T> action)
        where T : unmanaged, IEntityComponent
    {
        entities.Each((uint _, ref T t) => action?.Invoke(ref t));
    }

    public static void Each<T1, T2>(this EntityCollection<T1, T2> entities, ActionRef<T1, T2> action)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
    {
        entities.Each((uint _, ref T1 t1, ref T2 t2) => action?.Invoke(ref t1, ref t2));
    }

    public static void Each<T1, T2, T3>(this EntityCollection<T1, T2, T3> entities, ActionRef<T1, T2, T3> action)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
        where T3 : unmanaged, IEntityComponent
    {
        entities.Each((uint _, ref T1 t1, ref T2 t2, ref T3 t3) => action?.Invoke(ref t1, ref t2, ref t3));
    }

    public static void Each<T1, T2, T3, T4>(this EntityCollection<T1, T2, T3, T4> entities, ActionRef<T1, T2, T3, T4> action)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
        where T3 : unmanaged, IEntityComponent
        where T4 : unmanaged, IEntityComponent
    {
        entities.Each((uint _, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4) => action?.Invoke(ref t1, ref t2, ref t3, ref t4));
    }

    //
    // GroupsEnumerable
    //
    public static void Each<T>(this GroupsEnumerable<T> entities, QueryIndexCallback<T> callback)
        where T : unmanaged, IEntityComponent
    {
        foreach (var ((t, id, count), _) in entities)
        {
            for (var i = 0; i < count; i++)
            {
                callback?.Invoke(id[i], ref t[i]);
            }
        }
    }

    public static void Each<T1, T2>(this GroupsEnumerable<T1, T2> entities, QueryIndexCallback<T1, T2> callback)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
    {
        foreach (var ((t1, t2, id, count), _) in entities)
        {
            for (var i = 0; i < count; i++)
            {
                callback?.Invoke(id[i], ref t1[i], ref t2[i]);
            }
        }
    }

    public static void Each<T1, T2, T3>(this GroupsEnumerable<T1, T2, T3> entities, QueryIndexCallback<T1, T2, T3> callback)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
        where T3 : unmanaged, IEntityComponent
    {
        foreach (var ((t1, t2, t3, id, count), _) in entities)
        {
            for (var i = 0; i < count; i++)
            {
                callback?.Invoke(id[i], ref t1[i], ref t2[i], ref t3[i]);
            }
        }
    }

    public static void Each<T1, T2, T3, T4>(this GroupsEnumerable<T1, T2, T3, T4> entities, QueryIndexCallback<T1, T2, T3, T4> callback)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
        where T3 : unmanaged, IEntityComponent
        where T4 : unmanaged, IEntityComponent
    {
        foreach (var ((t1, t2, t3, t4, id, count), _) in entities)
        {
            for (var i = 0; i < count; i++)
            {
                callback?.Invoke(id[i], ref t1[i], ref t2[i], ref t3[i], ref t4[i]);
            }
        }
    }

    public static void Each<T>(this GroupsEnumerable<T> entities, ActionRef<T> action)
        where T : unmanaged, IEntityComponent
    {
        entities.Each((uint _, ref T t) => action?.Invoke(ref t));
    }

    public static void Each<T1, T2>(this GroupsEnumerable<T1, T2> entities, ActionRef<T1, T2> action)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
    {
        entities.Each((uint _, ref T1 t1, ref T2 t2) => action?.Invoke(ref t1, ref t2));
    }

    public static void Each<T1, T2, T3>(this GroupsEnumerable<T1, T2, T3> entities, ActionRef<T1, T2, T3> action)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
        where T3 : unmanaged, IEntityComponent
    {
        entities.Each((uint _, ref T1 t1, ref T2 t2, ref T3 t3) => action?.Invoke(ref t1, ref t2, ref t3));
    }

    public static void Each<T1, T2, T3, T4>(this GroupsEnumerable<T1, T2, T3, T4> entities, ActionRef<T1, T2, T3, T4> action)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
        where T3 : unmanaged, IEntityComponent
        where T4 : unmanaged, IEntityComponent
    {
        entities.Each((uint _, ref T1 t1, ref T2 t2, ref T3 t3, ref T4 t4) => action?.Invoke(ref t1, ref t2, ref t3, ref t4));
    }
}