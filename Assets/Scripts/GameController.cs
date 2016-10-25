using UnityEngine;
using System.Collections;
using System;

public class GameController : MonoBehaviour {

    private GameState currentGameState;
    private GameObject gameBounds;
    public GameObject[] players;
    
    public void Awake()
    {
        GameVars.GameController = this;
        
    }

	// Use this for initialization
	void Start ()
    {
        currentGameState = GameVars.GameFactory.CreateMenuState().Activate();
        BoxCollider2D gameBounds = GameObject.Find("GameBounds").GetComponent<BoxCollider2D>();
        gameBounds.size = new Vector2(GameVars.MapWidth, GameVars.MapHeight);
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
