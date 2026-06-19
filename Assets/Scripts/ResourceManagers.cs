using System;
using System.Collections.Generic;
using DBC.Common;
using Svelto.DataStructures.Experimental;
using Svelto.ECS.ResourceManager;

public class ResourceManager<T> : ECSResourceManager<T> where T : class { }

public class ResourceManagers
{
    private readonly Dictionary<Type, object> resourceManagers = new();

    public void AddPrebuiltResourceManager<T>(object resourceManager) where T : class
    {
        resourceManagers[typeof(T)] = resourceManager;
    }

    public ValueIndex Add<T>(in T resource) where T : class
    {
        if (!resourceManagers.ContainsKey(typeof(T)))
        {
            var nrm = new ResourceManager<T>();
            resourceManagers.Add(typeof(T), nrm);
            // This needs to be here so that invalid ValueIndexes return a null GameObject on release builds
            nrm.Add(null);
        }
        var rm = resourceManagers[typeof(T)] as ResourceManager<T>;
        return rm.Add(resource);
    }

    public bool Has<T>(ValueIndex index) where T : class
    {
        if (resourceManagers.TryGetValue(typeof(T), out var rmo))
        {
            var rm = rmo as ResourceManager<T>;
            try
            {
                var resource = rm[index];
                if (resource == null)
                {
                    return false;
                }
                return true;
            }
            catch (PreconditionException) { }
        }
        return false;
    }

    public T Get<T>(ValueIndex index) where T : class
    {
        if (resourceManagers.TryGetValue(typeof(T), out var rmo))
        {
            var rm = rmo as ResourceManager<T>;
            return rm[index];
        }
        return null;
    }

    public bool TryGet<T>(ValueIndex index, out T resource) where T : class
    {
        if (resourceManagers.TryGetValue(typeof(T), out var rmo))
        {
            var rm = rmo as ResourceManager<T>;
            try
            {
                resource = rm[index];
                if (resource == null)
                {
                    return false;
                }
                return true;
            }
            catch (PreconditionException) { }
        }
        resource = default;
        return false;
    }

    public void Remove<T>(ValueIndex index) where T : class
    {
        if (resourceManagers.TryGetValue(typeof(T), out var rmo))
        {
            var rm = rmo as ResourceManager<T>;
            rm.Remove(index);
        }
    }

    public void Clear<T>() where T : class
    {
        if (resourceManagers.TryGetValue(typeof(T), out var rmo))
        {
            var rm = rmo as ResourceManager<T>;
            rm.Clear();
        }
    }
}