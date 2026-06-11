using System;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

public class ECSRunner : MonoBehaviour
{
    private void Start()
    {
        // can't do this because InputSystem needs to be ran in Update
        // RunWorldInEarlyUpdate();
    }

    private void Update()
    {
        GameContext.World.Progress();
    }

    private void OnDestroy()
    {
        GameContext.World.Dispose();
    }

    private void RunWorldInEarlyUpdate()
    {
        var defaultLoop = PlayerLoop.GetDefaultPlayerLoop();

        // Find the position of the early update in the default loop
        int earlyUpdateIndex = -1;
        for (int i = 0; i < defaultLoop.subSystemList.Length; i++)
        {
            if (defaultLoop.subSystemList[i].type == typeof(EarlyUpdate))
            {
                earlyUpdateIndex = i + 1;
                break;
            }
        }

        // Insert a custom update before the early update
        if (earlyUpdateIndex >= 0)
        {
            var newSubsystemList = new PlayerLoopSystem[defaultLoop.subSystemList.Length + 1];
            Array.Copy(defaultLoop.subSystemList, newSubsystemList, earlyUpdateIndex);
            newSubsystemList[earlyUpdateIndex] = new PlayerLoopSystem
            {
                type = typeof(ECSRunner),
                updateDelegate = Update
            };
            Array.Copy(
                defaultLoop.subSystemList, earlyUpdateIndex, newSubsystemList, earlyUpdateIndex + 1,
                defaultLoop.subSystemList.Length - earlyUpdateIndex);
                defaultLoop.subSystemList = newSubsystemList;
        }

        // Set the modified player loop
        PlayerLoop.SetPlayerLoop(defaultLoop);

        void Update()
        {
            if (GameContext.World.IsValid())
            {
                GameContext.World.Progress();
            }
        }
    }
}
