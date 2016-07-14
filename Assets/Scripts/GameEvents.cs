using UnityEngine;
using System.Collections;

public class GameEvents : MonoBehaviour {

    public static void GenerateExplosion(Vector3 location,float strength, float size, bool showArt = true)
    {
        var inRange = Physics2D.CircleCastAll(location, size, Vector2.zero);
        foreach (RaycastHit2D other in inRange)
        {
            Vector3 distance = other.transform.position - location;
            if (distance.magnitude > 0)
            {
                other.rigidbody.AddForce(distance.normalized * CalcForce(strength, other.rigidbody.mass, distance));
            }
        }
    }


    public static void GeneratePulse(Vector3 location, float strength, float size,Rigidbody2D shipBody, bool showArt = true)
    {
        var inRange = Physics2D.CircleCastAll(location, size, Vector2.zero);
        Vector3 distance;
        foreach (RaycastHit2D other in inRange)
        {
            distance = other.transform.position - location;
            if (other.rigidbody.gameObject.CompareTag("Planet"))
            {
                CircleCollider2D planetColl = (CircleCollider2D)other.collider;
                Vector3 coll = (other.transform.position + (planetColl.radius * -distance.normalized));
                distance = coll - location;
                if (distance.magnitude > 0)
                {
                    shipBody.AddForce(distance.normalized * CalcForce(strength, other.rigidbody.mass, distance, false));
                }
                continue;
            }
            if (distance.magnitude > 0)
            {
                other.rigidbody.AddForce(distance.normalized * CalcForce(strength, other.rigidbody.mass, distance));
            }
            
        }
    }
    private static float CalcForce(float strength, float mass, Vector3 distance, bool positive = true)
    {
        float force = strength * mass * mass / distance.sqrMagnitude;
        if (positive)
        {
            return force;
        }
        return -force;
    }
}
