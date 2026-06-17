using System.Threading.Tasks;
using Svelto.DataStructures.Experimental;
using Svelto.ObjectPool;
using UnityEngine;

public class GameObjectResourceManager : ResourceManager<GameObject>
{
    private readonly ThreadSafeObjectPool<GameObject> resourcePool = new();
}