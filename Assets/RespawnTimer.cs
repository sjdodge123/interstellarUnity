using UnityEngine;
using System.Collections;

public class RespawnTimer : MonoBehaviour
{

    public float duration = 3f;
    private float currentTime;
    private GameObject parentShip;
    void Awake()
    {
        currentTime = Time.time;
        
    }

    void Update()
    {
        if (Time.time > currentTime + duration)
        {
            Destroy(gameObject);
            parentShip.SetActive(true);
        }
    }
    public void setParent(GameObject parent)
    {
        parentShip = parent;
    }
}
