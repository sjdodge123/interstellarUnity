using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlanetController : MonoBehaviour
{

    public float gravityRadius;

    private Rigidbody2D myBody;

    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        GameVars.Planets.Add(this);
    }

    /*
    void OnEnable()
    {
        GameVars.Camera.AddToCamera(gameObject);
    }
    */
    void FixedUpdate()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, gravityRadius);
        foreach (Collider2D coll in collisions)
        {
            if(coll.gameObject.layer == 9) //ignore gravity layer
            {
                continue;
            }
            Rigidbody2D otherBody = coll.attachedRigidbody;

            if (otherBody != null && otherBody != myBody)
            {
                Vector3 distance = otherBody.transform.position - transform.position;
                if (distance.sqrMagnitude > 0)
                {
                    float force = -GameVars.GravityConstant * otherBody.mass * myBody.mass / distance.sqrMagnitude;
                    otherBody.AddForce(distance.normalized * force);
                }
            }
            
        }
        CheckShipProximity(collisions);
    }

    private void CheckShipProximity(Collider2D[] collisions)
    {
        var hasShip = false;
        foreach (Collider2D coll in collisions)
        {
            Rigidbody2D otherBody = coll.attachedRigidbody;

            if (otherBody.gameObject.CompareTag("Player"))
            {
                hasShip = true;
                GameVars.Camera.AddToCamera(gameObject);
            }
        }
        if (!hasShip)
        {
            GameVars.Camera.RemoveFromCamera(gameObject);
        }
    }

    public Rigidbody2D GetRigidBody()
    {
        return myBody;
    }
}
