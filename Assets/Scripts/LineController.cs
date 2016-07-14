using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LineController : MonoBehaviour {

    
    public int intSteps;
    public Color startColor;
    public Color endColor;
    private Rigidbody2D shipBody;
    private LineRenderer lineRend;

    private float fadeRate = 3f;
    private float fadeTime = 0.0f;
    private float fadePercent = 0f;
    private Color lineStart;
    private Color lineEnd;
    private Color emptyColor = Color.black - new Color(0, 0, 0, .99f);
    

    // Use this for initialization
    void Start () {
        lineRend = GetComponent<LineRenderer>();
        shipBody = GetComponentInParent<Rigidbody2D>();
        lineStart = startColor;
        lineEnd = endColor - new Color(0, 0, 0, .95f);
    }

    // Update is called once per frame
    void FixedUpdate () {
        if(shipBody.velocity.sqrMagnitude > 0)
        {
            UpdateTrajectory(shipBody.transform.position, shipBody.velocity);
        }
	}

    void UpdateTrajectory(Vector3 initialPosition, Vector3 initialVelocity)
    {
        float timeDelta = Time.fixedDeltaTime;

        lineRend.SetVertexCount(intSteps);
        
        Vector3 position = initialPosition;
        Vector3 velocity = initialVelocity;
        
        for (int i = 0; i < intSteps; ++i)
        {
            Vector3 gravContr = Vector3.zero;
            lineRend.SetPosition(i, position);
 
            foreach (PlanetController planet in GameVars.Planets)
            {
                gravContr += GetPlanetGravity(planet, position);
            }
            var lastPosition = position;

            position += velocity * timeDelta + gravContr * timeDelta * timeDelta;
            RaycastHit2D cast = Physics2D.Linecast(lastPosition, position);
            if (cast.collider != null && cast.rigidbody.gameObject.CompareTag("Planet"))
            {
                ResetLine();
                lineRend.SetVertexCount(i);
                break;
            }
            velocity += gravContr * timeDelta;
            
        }
    }

    private Vector3 GetPlanetGravity(PlanetController planetController,Vector3 position)
    {
        Rigidbody2D planetBody = planetController.GetRigidBody();
        Vector3 distance = planetBody.transform.position - position;
        Vector3 gravContr = Vector3.zero;
        if (distance.magnitude > 0 && distance.magnitude < planetController.gravityRadius)
        {
            float accel = GameVars.GravityConstant * planetBody.mass / distance.sqrMagnitude;
            gravContr = distance.normalized * accel;
        }
        return gravContr;
    }

    internal void ResetLine()
    {
        fadeTime = Time.time + fadeRate;
        if (!lineRend.enabled)
        {
            lineRend.enabled = true;
        }
        fadePercent = 0f;
        lineRend.SetColors(lineStart, lineEnd);
    }

    internal void FadeLine()
    {
        fadePercent += .005f;
        lineRend.SetColors(Color.Lerp(lineStart, emptyColor, fadePercent), Color.Lerp(lineEnd, emptyColor, fadePercent));
        if (Time.time > fadeTime)
        {
            lineRend.enabled = false;
        }
    }
}
