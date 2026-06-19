using Svelto.DataStructures.Experimental;
using UnityEngine;

public struct ResourceIndex<T> where T : class
{
    public ValueIndex ValueIndex;

    public ResourceIndex(ValueIndex valueIndex)
    {
        ValueIndex = valueIndex;
    }

    public readonly T ToObject(ResourceManagers resourceManagers)
    {
        return resourceManagers.Get<T>(ValueIndex);
    }

    public static implicit operator ValueIndex(ResourceIndex<T> ri) => ri.ValueIndex;
    public static implicit operator ResourceIndex<T>(ValueIndex vi) => new(vi);
}

public static class ResourceIndexExtensions
{
    public static ResourceIndex<T> ToResourceIndex<T>(this ValueIndex valueIndex) where T : Object
    {
        return new(valueIndex);
    }
}