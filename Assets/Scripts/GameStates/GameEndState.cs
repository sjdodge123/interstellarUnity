using UnityEngine;
using UnityEngine.UI;
using System;

public class GameEndState : GameState
{
    private Text centerText;
    public GameEndState(Text centerText)
    {
        this.centerText = centerText;
    }
    public override GameState Activate()
    {
        centerText.text = "GameOver!";
        GameVars.GameFactory.CreateGameEndTimer();
        return this;
    }

    public override void Deactivate()
    {
        //DO stuff
    }
}
