using UnityEngine;

public class ECSRunner : MonoBehaviour
{
    private void Start()
    {
        GameContext.World.Entity<UpdateDeltaTimeEntityDescriptor>(UpdateDeltaTimeEntityDescriptor.Group);
    }

    private void Update()
    {
        GameContext.World.Progress();
    }

    private void OnDestroy()
    {
        GameContext.World.Dispose();
    }
}
