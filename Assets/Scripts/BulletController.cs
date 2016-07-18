using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{
    public float explosionStrength;
    public float explosionSize;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameEvents.GenerateExplosion(transform.position, explosionStrength, explosionSize);
        Destroy(gameObject);
    }
}
