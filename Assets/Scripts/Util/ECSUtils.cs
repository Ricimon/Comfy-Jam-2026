using System;
using Svelto.ECS;

public static class ECSUtils
{
    public static void RunOnFilteredComponents<T>(this EntityFilterCollection filterCollection, EntitiesDB entitiesDB, ActionRef<T> action) where T : unmanaged, IEntityComponent
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
}