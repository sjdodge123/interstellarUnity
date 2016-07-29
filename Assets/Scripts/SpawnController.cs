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
        if (spawnObject.CompareTag("Planet"))
        {
            spawnPlanet();
        }
        else
        {
            spawnOther();
        }
        

	}
    private void spawnPlanet()
    {
        GameObject[] players = GameVars.GameController.players;

       
        //PlanetController script = spawnObject.GetComponent<PlanetController>();
        //float gravityRadius = script.gravityRadius;

        for (var i = 0; i < quantity; i++)
        {
            float xLeftBound  = spawnX + i * spawnWidth / quantity;
            float xRightBount = xLeftBound + (i+1) * spawnWidth / quantity;

            float yUpBound  = spawnY - i * spawnHeight / quantity;
            float yBotBound = yUpBound - (i + 1) * spawnHeight / quantity;

            var spawnLocation = new Vector3(Random.Range(xLeftBound, xRightBount), Random.Range(yBotBound, yUpBound), 0);
            
            for (var j = 0; j < players.Length; j++)
            {
                Vector3 distance = players[j].transform.position - spawnLocation;
                while (distance.magnitude < spawnWidth/(quantity*2))
                {
                    xLeftBound = spawnX + i * spawnWidth / quantity;
                    xRightBount = xLeftBound + (i + 1) * spawnWidth / quantity;

                    yUpBound = spawnY - i * spawnHeight / quantity;
                    yBotBound = yUpBound - (i + 1) * spawnHeight / quantity;

                    spawnLocation = new Vector3(Random.Range(xLeftBound, xRightBount), Random.Range(yUpBound, yBotBound), 0);
                    distance = players[j].transform.position - spawnLocation;
                } 
            }
            var spawned = Instantiate(spawnObject, spawnLocation, Quaternion.identity) as GameObject;
        }
    }
    private void spawnOther()
    {
        for (var i = 0; i < quantity; i++)
        {
            var spawnRange = new Vector2(Random.Range(spawnX, spawnX + spawnWidth), Random.Range(spawnY, spawnY - spawnHeight));
            var velocity = Random.Range(minVelocity, maxVelocity);
            Vector2 unitVector = new Vector2(Random.Range(-1f, 1f), (Random.Range(-1f, 1f)));
            if (unitVector.sqrMagnitude == 0)
            {
                unitVector = Vector2.zero;
            }
            else
            {
                unitVector = unitVector.normalized;
            }
            var spawned =Instantiate(spawnObject, spawnRange, Quaternion.identity) as GameObject;

            spawned.GetComponent<Rigidbody2D>().velocity = unitVector * velocity;
        }
    }
}
