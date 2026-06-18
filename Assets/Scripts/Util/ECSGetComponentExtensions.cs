using Svelto.DataStructures;
using Svelto.ECS;

public static class ECSGetComponentExtensions
{
    public static T GetSingletonComponent<T>(this EntitiesDB entitiesDB, ExclusiveGroupStruct groupStructId)
        where T : unmanaged, IEntityComponent
    {
        return entitiesDB.QueryUniqueEntity<T>(groupStructId);
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

    public static T GetComponent<T>(this EntitiesDB entitiesDB, uint id, ExclusiveGroupStruct group)
        where T : unmanaged, IEntityComponent
    {
        return entitiesDB.QueryEntity<T>(id, group);
    }

    public static T GetComponent<T>(this EntitiesDB entitiesDB, EGID egid)
        where T : unmanaged, IEntityComponent
    {
        return entitiesDB.QueryEntity<T>(egid);
    }

    //
    // TryGetComponent, EGID input
    //
    public static bool TryGetComponent<T>(this EntitiesDB entitiesDB, EGID egid, ActionRef<T> action)
        where T : unmanaged, IEntityComponent
    {
        if (entitiesDB.TryQueryEntitiesAndIndex(egid, out var i, out NB<T> t))
        {
            action?.Invoke(ref t[i]);
            return true;
        }
        return false;
    }

    public static bool TryGetComponent<T1, T2>(this EntitiesDB entitiesDB, EGID egid, ActionRef<T1, T2> action)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
    {
        bool foundEntity = false;
        entitiesDB.QueryEntities<T1, T2>(egid.groupID)
            .Each((uint i, ref T1 t1, ref T2 t2) =>
            {
                if (i == egid.entityID)
                {
                    action?.Invoke(ref t1, ref t2);
                    foundEntity = true;
                }
            });
        return foundEntity;
    }

    public static bool TryGetComponent<T1, T2, T3>(this EntitiesDB entitiesDB, EGID egid, ActionRef<T1, T2, T3> action)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
        where T3 : unmanaged, IEntityComponent
    {
        bool foundEntity = false;
        entitiesDB.QueryEntities<T1, T2, T3>(egid.groupID)
            .Each((uint i, ref T1 t1, ref T2 t2, ref T3 t3) =>
            {
                if (i == egid.entityID)
                {
                    action?.Invoke(ref t1, ref t2, ref t3);
                    foundEntity = true;
                }
            });
        return foundEntity;
    }

    //
    // TryGetComponent, id & ExclusiveGroup input
    //
    public static bool TryGetComponent<T>(this EntitiesDB entitiesDB, uint id, ExclusiveGroupStruct group, ActionRef<T> action)
        where T : unmanaged, IEntityComponent
    {
        return entitiesDB.TryGetComponent(new EGID(id, group), action);
    }

    public static bool TryGetComponent<T1, T2>(this EntitiesDB entitiesDB, uint id, ExclusiveGroupStruct group, ActionRef<T1, T2> action)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
    {
        return entitiesDB.TryGetComponent(new EGID(id, group), action);
    }

    public static bool TryGetComponent<T1, T2, T3>(this EntitiesDB entitiesDB, uint id, ExclusiveGroupStruct group, ActionRef<T1, T2, T3> action)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
        where T3 : unmanaged, IEntityComponent
    {
        return entitiesDB.TryGetComponent(new EGID(id, group), action);
    }

    //
    // TryGetComponent, Groups input
    //
    public static bool TryGetComponent<T>(this EntitiesDB entitiesDB, uint id, in LocalFasterReadOnlyList<ExclusiveGroupStruct> groups, ActionRef<T> action)
        where T : unmanaged, IEntityComponent
    {
        bool foundEntity = false;
        entitiesDB.QueryEntities<T>(groups)
            .Each((EGID egid, ref T t) =>
            {
                if (egid.entityID == id)
                {
                    action?.Invoke(ref t);
                    foundEntity = true;
                }
            });
        return foundEntity;
    }

    public static bool TryGetComponent<T1, T2>(this EntitiesDB entitiesDB, uint id, in LocalFasterReadOnlyList<ExclusiveGroupStruct> groups, ActionRef<T1, T2> action)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
    {
        bool foundEntity = false;
        entitiesDB.QueryEntities<T1, T2>(groups)
            .Each((EGID egid, ref T1 t1, ref T2 t2) =>
            {
                if (egid.entityID == id)
                {
                    action?.Invoke(ref t1, ref t2);
                    foundEntity = true;
                }
            });
        return foundEntity;
    }

    public static bool TryGetComponent<T1, T2, T3>(this EntitiesDB entitiesDB, uint id, in LocalFasterReadOnlyList<ExclusiveGroupStruct> groups, ActionRef<T1, T2, T3> action)
        where T1 : unmanaged, IEntityComponent
        where T2 : unmanaged, IEntityComponent
        where T3 : unmanaged, IEntityComponent
    {
        bool foundEntity = false;
        entitiesDB.QueryEntities<T1, T2, T3>(groups)
            .Each((EGID egid, ref T1 t1, ref T2 t2, ref T3 t3) =>
            {
                if (egid.entityID == id)
                {
                    action?.Invoke(ref t1, ref t2, ref t3);
                    foundEntity = true;
                }
            });
        return foundEntity;
    }
}