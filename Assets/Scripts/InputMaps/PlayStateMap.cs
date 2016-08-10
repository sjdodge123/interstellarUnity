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

            //Controller-based rotation
            var thisHorAxis = Input.GetAxis(i + "LeftJoystickHorzional");
            var thisVertAxis = Input.GetAxis(i + "LeftJoystickVertical");
            
            if (thisHorAxis != 0 || thisVertAxis != 0)
            {
                ship.RotateToAngle(thisHorAxis, thisVertAxis);
            }
            
            //Keyboard-based rotation
            var horizontal = Input.GetAxis(i + "Horizontal");

            if (horizontal != 0)
            {
                ship.MoveHorizontal(horizontal);
            }

            //Thrust control
            var vertical = Input.GetAxis(i + "Vertical");
            if (Input.GetButton(i + "AButton"))
            {
                vertical = Input.GetAxis(i + "AButton");
            }

            if (vertical != 0)
            {
                ship.MoveVertical(vertical);
            }

            if (Input.GetButtonDown(i+"Jump"))
            {
                ship.Pulse();
            }

            if (Input.GetButton(i + "Fire1") || Input.GetButton(i + "RightBumper"))
            {
                ship.AimStarboard();
            }
            if (Input.GetButtonUp(i + "Fire1")|| Input.GetButtonUp(i + "RightBumper"))
            {
                ship.FireStarboard();
            }

            if (Input.GetButton(i + "Fire2") || Input.GetButton(i + "LeftBumper"))
            {
                ship.AimPort();
            }
            if (Input.GetButtonUp(i + "Fire2") || Input.GetButtonUp(i + "LeftBumper"))
            {
                ship.FirePort();
            }
        }
	}
}
