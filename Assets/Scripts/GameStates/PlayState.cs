using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayState : GameState
{
    private Text centerText;
    private GameObject playStateMap;
    private GameObject[] players;
    public PlayState(Text centerText, GameObject playStateMap)
    {
        this.centerText = centerText;
        this.playStateMap = playStateMap;
        this.players = GameVars.GameController.players;
    }
    public override GameState Activate()
    {
        centerText.text = "";
        playStateMap.SetActive(true);

        for (var i = 0; i < players.Length; i++)
        {    
            players[i].SetActive(true);
        }

        return this;
    }

    public override void Deactivate()
    {
        playStateMap.SetActive(false);
        for (var i = 0; i < players.Length; i++)
        {
            players[i].SetActive(false);
        }
    }
}
