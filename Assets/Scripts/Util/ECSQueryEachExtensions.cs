using Svelto.ECS;

public static class ECSQueryEachExtensions
{
    public delegate void QueryIndexCallback<T>(uint i, ref T t);
    public delegate void QueryIndexCallback<T1, T2>(uint i, ref T1 t1, ref T2 t2);
    public delegate void QueryIndexCallback<T1, T2, T3>(uint i, ref T1 t1, ref T2 t2, ref T3 t3);

    public static void Each<T>(this GroupsEnumerable<T> entities, ActionRef<T> action)
        where T : unmanaged, IEntityComponent
    {
        foreach (var ((t, count), _) in entities)
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
        foreach (var ((t1, t2, count), _) in entities)
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
        foreach (var ((t1, t2, t3, count), _) in entities)
        {
            for (var i = 0; i < count; i++)
            {
                action?.Invoke(ref t1[i], ref t2[i], ref t3[i]);
            }
        }
    }

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
}