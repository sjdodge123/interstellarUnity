using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {

    public GameObject spawnObject;
    public int quantity;
    public int spawnX;
    public int spawnY;
    public int spawnWidth;
    public int spawnHeight;

    public int minVelocity;
    public int maxVelocity;

    // Use this for initialization
    void Start () {
        for(var i=0;i<quantity; i++)
        {
            var spawnRange = new Vector2(Random.Range(spawnX, spawnX + spawnWidth), Random.Range(spawnY, spawnY - spawnHeight));
            var velocity = Random.Range(minVelocity, maxVelocity);
            Vector2 unitVector = new Vector2(Random.Range(-1f,1f), (Random.Range(-1f, 1f)));
            if (unitVector.sqrMagnitude == 0)
            {
                unitVector = Vector2.zero;
            }
            else
            {
                unitVector = unitVector.normalized;
            }
            var spawned = (GameObject)Instantiate(spawnObject, spawnRange, Quaternion.identity);
            spawned.GetComponent<Rigidbody2D>().velocity = unitVector * velocity;
        }
	}
}
