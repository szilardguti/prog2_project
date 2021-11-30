using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameInterface : MonoBehaviour
{
    public BattleSystem battleSystem;
    public BubblePopManager bubblePopManager;
    public static MinigameInterface minigameInterface;

    private void Awake()
    {
        minigameInterface = this;
    }

    public void BubblePopFinished()
    {
        battleSystem.ReturnGameplay();
    }
}
