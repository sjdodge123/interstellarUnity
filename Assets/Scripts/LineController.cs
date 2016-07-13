using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LineController : MonoBehaviour {

    public GameObject ship;
    public PlanetController planetController;
    public GameObject planet;
    public int intSteps;
    private Rigidbody2D planetBody;
    private Rigidbody2D shipBody;
    private LineRenderer lineRend;

	// Use this for initialization
	void Start () {
        lineRend = GetComponent<LineRenderer>();
        planetBody = planet.GetComponent<Rigidbody2D>();
        shipBody = ship.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        if(shipBody.velocity.sqrMagnitude > 0)
        {
            UpdateTrajectory(ship.transform.position, shipBody.velocity);
        } 
	}

    void UpdateTrajectory(Vector3 initialPosition, Vector3 initialVelocity)
    {
        float timeDelta = Time.deltaTime;

        lineRend.SetVertexCount(intSteps);

        Vector3 position = initialPosition;
        Vector3 velocity = initialVelocity;
        for (int i = 0; i < intSteps; ++i)
        {
            lineRend.SetPosition(i, position);

            Vector3 distance = planet.transform.position - position;
            Vector3 gravContr = new Vector3();
            if (distance.sqrMagnitude > 0)
            {
                float force = planetController.gravityConstant * shipBody.mass * planetBody.mass / distance.sqrMagnitude;
                gravContr = distance.normalized * force;
            }

            position += velocity * timeDelta + 0.5f * gravContr * timeDelta * timeDelta;
            velocity += gravContr * timeDelta;
        }
    }
}
