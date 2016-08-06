using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LineController : MonoBehaviour
{


    public int intSteps;
    public Color startColor;
    public Color endColor;
    
    private LineRenderer lineRend;

    private float fadeDuration = 2f;
    private float fadeTimer = 0.0f;
    private float fadePercent = 0f;

    private float renderDuration = 3f;
    private float renderTimer = 0.0f;
    private Color emptyStart;
    private Color emptyEnd;

    private Rigidbody2D targetBody;

    private bool collisionTrajectory;
    private bool rendering;



    public void buildObject(Rigidbody2D targetBody)
    {
        this.targetBody = targetBody;
    }

    void Awake()
    {
        lineRend = GetComponent<LineRenderer>();
        lineRend.SetColors(startColor, endColor);

        emptyStart = new Color(startColor.r, startColor.g, startColor.b, 0f);
        emptyEnd = new Color(endColor.r, endColor.g, endColor.b, 0f);
    }

    public void FixedUpdate()
    {
        fadeTimer = Time.time + fadeDuration;
        if (rendering)
        {
            UpdateTrajectory(targetBody.transform.position, targetBody.velocity);
            if (Time.time > renderTimer && !collisionTrajectory)
            {
                ToggleOff();
            }
        }
        
        if (!collisionTrajectory)
        {
            FadeLine();
        }

    }

    public void ToggleOn()
    {
        ResetColor();
        rendering = true;
        renderTimer = Time.time + renderDuration;

    }
    public void ToggleOff()
    {
        rendering = false;
    }

    public void HideLine()
    {
        lineRend.enabled = false;
        ToggleOff();
    }

    public void UpdateTrajectory(Vector3 initialPosition, Vector3 initialVelocity)
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
                collisionTrajectory = true;
                lineRend.SetVertexCount(i);
                break;
            }
            else
            {
                collisionTrajectory = false;
            }
            velocity += gravContr * timeDelta;
        }
    }

    private Vector3 GetPlanetGravity(PlanetController planetController, Vector3 position)
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

  
    private void ResetColor()
    { 
        if (!lineRend.enabled)
        {
            lineRend.enabled = true;
        }
        fadePercent = 0f;
        lineRend.SetColors(startColor, endColor);
    }
    
    private void FadeLine()
    {
        fadePercent += .010f;
        lineRend.SetColors(Color.Lerp(startColor, emptyStart, fadePercent), Color.Lerp(endColor, emptyEnd, fadePercent));

        if (Time.time > fadeTimer)
            {
                lineRend.enabled = false;
            }
    }
}
