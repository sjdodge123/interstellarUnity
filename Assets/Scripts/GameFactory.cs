using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GameFactory : MonoBehaviour {

    public Text centerText;
    public GameObject menuStateMap;
    public GameObject playStateMap;

    public GameEndStateTimer timer;

    public GameFactory()
    {

    }

    public void Awake()
    {
        GameVars.GameFactory = this;
    }

    public MenuState CreateMenuState()
    {
        return new MenuState(centerText, menuStateMap);
    }

    public void CreateGameEndTimer()
    {
        Instantiate(timer);
    }
    
    public PlayState CreatePlayState()
    {
        return new PlayState(centerText,playStateMap);
    }

    public PauseState CreatePauseState()
    {
        return new PauseState();
    }

    public GameEndState CreateGameEndState()
    {
        return new GameEndState(centerText);
    }
}
