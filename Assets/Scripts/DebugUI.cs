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
        if (GUILayout.Button("Spawn Yellow Slime"))
        {
            SlimeSpawnerSystem.SpawnSlime(GameContext.World, SlimeColor.Yellow);
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Spawn Blue Slime"))
        {
            SlimeSpawnerSystem.SpawnSlime(GameContext.World, SlimeColor.Blue);
        }
        GUILayout.FlexibleSpace();

        GUILayout.Label($"SCORE: {GameStatSystem.Score}");
        GUILayout.Label($"Lives: {GameStatSystem.Lives}");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Spawn Disguised Yellow Slime"))
        {
            SlimeSpawnerSystem.SpawnSlime(GameContext.World, SlimeColor.Yellow, DisguiseType.Default);
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Spawn Disguised Blue Slime"))
        {
            SlimeSpawnerSystem.SpawnSlime(GameContext.World, SlimeColor.Blue, DisguiseType.Default);
        }
        GUILayout.EndHorizontal();


        

        GUI.skin = null;
    }
}