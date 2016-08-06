using UnityEngine;
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
            var ship = controllers[i];

            var horizontal = Input.GetAxis(i + "Horizontal");
            var controllerLeftHorizontal = Input.GetAxis("LeftJoystickHorzional");

            if (controllerLeftHorizontal != 0)
            {
                var controllerLeftVertical = Input.GetAxis("LeftJoystickVertical");
                horizontal = controllerLeftHorizontal;
            }
            
            if (horizontal != 0)
            {
                ship.MoveHorizontal(horizontal);
            }

            //Thrust control
            var vertical = Input.GetAxis(i + "Vertical");
            if (Input.GetButton("AButton"))
            {
                vertical = Input.GetAxis("AButton");
            }

            if (vertical != 0)
            {
                ship.MoveVertical(vertical);
            }

            if (Input.GetButtonDown(i+"Jump"))
            {
                ship.Pulse();
            }

            if (Input.GetButton(i + "Fire1") || Input.GetButton("RightBumper"))
            {
                ship.AimStarboard();
            }
            if (Input.GetButtonUp(i + "Fire1")|| Input.GetButtonUp("RightBumper"))
            {
                ship.FireStarboard();
            }

            if (Input.GetButton(i + "Fire2") || Input.GetButton("LeftBumper"))
            {
                ship.AimPort();
            }
            if (Input.GetButtonUp(i + "Fire2") || Input.GetButtonUp("LeftBumper"))
            {
                ship.FirePort();
            }
        }
	}
}
