using UnityEngine;
using System.Collections;

public class MenuStateMap : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown)
        {
            GameVars.GameController.MoveToPlayState();
        }
	}
}
