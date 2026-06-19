using Svelto.ECS;
using UnityEngine;

public struct DisguiseSpawner : IEntityComponent
{
    public ResourceIndex<GameObject> DisguiseDefaultPrefabId;
    public ResourceIndex<GameObject> DisguiseYellowHoodiePrefabId;
    public ResourceIndex<GameObject> DisguiseBlueHoodiePrefabId;
}

public class DisguiseSpawnerEntity : GenericEntityDescriptorAndGroup<
    DisguiseSpawner,
    GameObjectReference>
{ }