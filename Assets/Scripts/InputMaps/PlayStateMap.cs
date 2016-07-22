﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayStateMap : MonoBehaviour {

    private GameObject[] players;
    private List<ShipController> controllers;
    // Use this for initialization
    void Start()
    {
        this.players = GameVars.GameController.players;
        this.controllers = new List<ShipController>();
        for (var i = 0; i < players.Length; i++)
        {
            controllers.Add(players[i].GetComponent<ShipController>());
        }
    }
	
	// Update is called once per frame
	void Update () {

        for (var i = 0; i < players.Length; i++)
        {
            var horizontal = Input.GetAxis(i + "Horizontal");
            if (horizontal != 0)
            {
                controllers[i].MoveHorizontal(horizontal);
            }
            var vertical = Input.GetAxis(i + "Vertical");
            if (vertical != 0)
            {
                controllers[i].MoveVertical(vertical);
            }
        }
	}
}