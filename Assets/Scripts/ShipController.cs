using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {

    public float speed;
    public float rotateSpeed;
    private Rigidbody2D body;
	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.forward * -Input.GetAxis("Horizontal") * rotateSpeed);  
    }

    void FixedUpdate()
    {
        body.AddForce(transform.up * speed * Input.GetAxis("Vertical"));
    }
}
