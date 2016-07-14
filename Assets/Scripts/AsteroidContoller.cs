using UnityEngine;
using System.Collections;

public class AsteroidContoller : MonoBehaviour
{
    public float explosionStrength;
    public float explosionSize;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Planet"))
        {
            GameEvents.GenerateExplosion(transform.position, explosionStrength, explosionSize);
            Destroy(gameObject);
        }
    }
}
