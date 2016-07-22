using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuState : GameState {

    private Text centerText;
    private GameObject menuStateMap;


    public MenuState(Text centerText,GameObject menuStateMap)
    {
        this.centerText = centerText;
        this.menuStateMap = menuStateMap;
    }

    public override GameState Activate()
    {
        centerText.text = "Press any key to begin!";
        menuStateMap.SetActive(true);
        return this;
    }

    public override void Deactivate()
    {
        menuStateMap.SetActive(false);
    }
}
