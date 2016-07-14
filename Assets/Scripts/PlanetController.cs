using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetController : MonoBehaviour
{

    public float gravityRadius;

    private Rigidbody2D myBody;

    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        GameVars.Planets.Add(this);
    }

    void FixedUpdate()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, gravityRadius);
        
        
        foreach (Collider2D coll in collisions)
        {
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
    }
    public Rigidbody2D GetRigidBody()
    {
        return myBody;
    }
}
