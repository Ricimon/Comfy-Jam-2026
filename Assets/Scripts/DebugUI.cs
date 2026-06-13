using UnityEngine;

public class DebugUI : MonoBehaviour
{
    public GUISkin customSkin;

    private void OnGUI()
    {
        GUI.skin = customSkin;

        if (GUILayout.Button("Spawn Slime"))
        {
            SlimeSpawnerSystem.SpawnSlime(GameContext.World);
        }

        GUI.skin = null;
    }
}