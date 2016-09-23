using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {

    public GameObject spawnObject;
    public int quantity;
    public int spawnX;
    public int spawnY;

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
            float spawnCushion = GameVars.MapWidth / (quantity * 2);
            var spawnLocation = generateSpawnLoc(i);

            for (var j = 0; j < players.Length; j++)
            {
                Vector3 distance = players[j].transform.position - spawnLocation;
                while (distance.magnitude < spawnCushion)
                {
                    spawnLocation = generateSpawnLoc(i);
                    distance = players[j].transform.position - spawnLocation;
                } 
            }
            Instantiate(spawnObject, spawnLocation, Quaternion.identity);
        }
    }
    private void spawnOther()
    {
        for (var i = 0; i < quantity; i++)
        {
            var spawnRange = new Vector2(Random.Range(spawnX, spawnX + GameVars.MapWidth), Random.Range(spawnY, spawnY - GameVars.MapHeight));
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
    private Vector3 generateSpawnLoc(int objInt)
    {
        float xLeftBound = spawnX + objInt * GameVars.MapWidth / quantity;
        float xRightBount = xLeftBound + (objInt + 1) * GameVars.MapWidth / quantity;

        float yUpBound = spawnY - objInt * GameVars.MapHeight / quantity;
        float yBotBound = yUpBound - (objInt + 1) * GameVars.MapHeight / quantity;

        var spawnLocation = new Vector3(Random.Range(xLeftBound, xRightBount), Random.Range(yBotBound, yUpBound), 0);

        return spawnLocation;
    }
}
