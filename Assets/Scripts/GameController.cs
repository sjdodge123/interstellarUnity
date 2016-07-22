using UnityEngine;
using System.Collections;
using System;

public class GameController : MonoBehaviour {

    private GameState currentGameState;
    public GameObject[] players;

    public void Awake()
    {
        GameVars.GameController = this;
    }

	// Use this for initialization
	void Start ()
    {
        currentGameState = GameVars.GameFactory.CreateMenuState().Activate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void MoveToMenuState()
    {
        currentGameState.Deactivate();
        currentGameState = GameVars.GameFactory.CreateMenuState().Activate();
    }

    internal void MoveToPlayState()
    {
        currentGameState.Deactivate();
        currentGameState = GameVars.GameFactory.CreatePlayState().Activate();
    }

    internal void MoveToEndGameState()
    {
        currentGameState.Deactivate();
        currentGameState = GameVars.GameFactory.CreateGameEndState().Activate();
    }

    public void SomethingDied(GameObject obj)
    {
        obj.SetActive(false);
        MoveToEndGameState();
    }
}
