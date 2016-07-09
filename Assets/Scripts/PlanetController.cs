using UnityEngine;
using System.Collections;

public class PlanetController : MonoBehaviour
{
    public float gravityConstant;
    public float gravityRadius;
    private Rigidbody2D myBody;
    private CircleCollider2D gravityWell;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        gravityWell = gameObject.AddComponent<CircleCollider2D>();
        gravityWell.offset.Set(0, 0);
        gravityWell.isTrigger = true;
        gravityWell.enabled = true;
        gravityWell.radius = gravityRadius;
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D otherBody = other.GetComponent<Rigidbody2D>();
        Vector3 distance = other.transform.position - transform.position;
        if (distance.sqrMagnitude > 0)
        {
            float force = -gravityConstant * otherBody.mass * myBody.mass / distance.sqrMagnitude;
            otherBody.AddForce(distance.normalized * force);
        }
    }
}
