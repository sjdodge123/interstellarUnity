using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetController : MonoBehaviour
{
    public float gravityConstant;
    public float gravityRadius;

    private Rigidbody2D myBody;
    private CircleCollider2D gravityWell;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
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

                    float force = -gravityConstant * otherBody.mass * myBody.mass / distance.sqrMagnitude;
                    otherBody.AddForce(distance.normalized * force);
                }
            }
        }
    }
}
