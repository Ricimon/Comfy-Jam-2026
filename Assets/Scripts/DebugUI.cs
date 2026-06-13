using UnityEngine;

public class DebugUI : MonoBehaviour
{
    public GUISkin customSkin;

    private void OnGUI()
    {
        GUI.skin = customSkin;

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Spawn Slime"))
        {
            SlimeSpawnerSystem.SpawnSlime(GameContext.World, SlimeColor.None);
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Spawn Red Slime"))
        {
            SlimeSpawnerSystem.SpawnSlime(GameContext.World, SlimeColor.Red);
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Spawn Blue Slime"))
        {
            SlimeSpawnerSystem.SpawnSlime(GameContext.World, SlimeColor.Blue);
        }
        GUILayout.EndHorizontal();

        GUI.skin = null;
    }
}