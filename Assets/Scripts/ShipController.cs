using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipController : MonoBehaviour {

    public float speed;
    public float rotateSpeed;
    public float weaponRadius;

    public float fireRate = 1F;
    public float nextFire = 0.0F;
    public float weaponStrength;// = 10000F;

    public float haloTimer;
    private float haloTimeStamp;
    private bool haloBool = false;

    private Component halo;
    private Rigidbody2D body;
    private RaycastHit2D[] inRange;

    private CircleCollider2D weaponHalo;

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();
        halo = GetComponent("Halo");
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.forward * -Input.GetAxis("Horizontal") * rotateSpeed);

        //Weapon firing
        if (Input.GetButtonDown("Jump") && Time.time > nextFire)
        {
            halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
            haloBool = true;
            haloTimeStamp = Time.time;
            inRange = Physics2D.CircleCastAll(body.position, weaponRadius, Vector2.zero);
            nextFire = Time.time + fireRate;
            foreach (RaycastHit2D other in inRange)
            {
                if (other.rigidbody.gameObject.CompareTag("Asteroid"))
                {
                    Vector3 distance = other.transform.position - transform.position;
                    if (distance.sqrMagnitude > 0)
                    {
                        float force = weaponStrength * other.rigidbody.mass * body.mass / distance.sqrMagnitude;
                        other.rigidbody.AddForce(distance.normalized * force);
                    }
                }
                else if(other.rigidbody.gameObject.CompareTag("Planet"))
                {
                    if (other.collider.isTrigger)
                    {
                        continue;
                    }
                    Vector3 distance = other.transform.position - transform.position;

                    CircleCollider2D planetColl = (CircleCollider2D)other.collider;

                    Vector3 coll = (other.transform.position + (planetColl.radius * -distance.normalized));
                    distance = coll - transform.position;
                    //distance = coll - transform.position;
                    Debug.Log(coll);
                    if (distance.sqrMagnitude > 0)
                    {
                        float force = -weaponStrength * body.mass * body.mass / distance.sqrMagnitude;
                        body.AddForce(distance.normalized * force);
                    }

                } 
            }


            //GameObject clone = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
            
        }

        if(haloBool == true && (Time.time - haloTimeStamp) > haloTimer)
        {
            halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
            haloBool = false;
        }


            
    }


    void FixedUpdate()
    {
        body.AddForce(transform.up * speed * Input.GetAxis("Vertical"));
    }
}
