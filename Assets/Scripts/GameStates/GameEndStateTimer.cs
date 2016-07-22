using UnityEngine;
using System.Collections;

public class GameEndStateTimer : MonoBehaviour {

    public float duration = 5f;
    private float currentTime = 0.0f;
    void Awake()
    {
        currentTime = Time.time;
    }

	// Update is called once per frame
	void Update () {
	    if(Time.time > currentTime + duration)
        {
            Destroy(gameObject);
            GameVars.GameController.MoveToMenuState();    
        }
	}
}
