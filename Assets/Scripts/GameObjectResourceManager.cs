using System.Threading.Tasks;
using Svelto.DataStructures.Experimental;
using Svelto.ECS.ResourceManager;
using Svelto.ObjectPool;
using UnityEngine;

public class GameObjectResourceManager : ECSResourceManager<GameObject>
{
    private readonly ThreadSafeObjectPool<GameObject> resourcePool = new();
}