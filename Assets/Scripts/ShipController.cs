using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ShipController : MonoBehaviour {

    public GameObject bullet;

    public float speed;
    public float rotateSpeed;
    public float pulseRadius;

    public float pulseRate = 1F;
    public float nextPulse = 0.0F;
    public float pulseStrength;

    public float haloTimer;
    private float haloTimeStamp;
    private bool haloBool = false;

    private LineController lineController;
    private Component halo;
    private Rigidbody2D body;

    // Use this for initialization
    void Awake () {
        body = GetComponent<Rigidbody2D>();
        halo = GetComponent("Halo");
        lineController= GetComponentInChildren<LineController>();
        GameVars.Ships.Add(this);
    }
	
	// Update is called once per frame
	void Update () {
        UsePlayerControls();
             
        //Pulse Cannon
        if (Input.GetButtonDown("Jump") && Time.time > nextPulse)
        {
            Pulse();
        }

        bool fire1Held = Input.GetButton("Fire1");
        bool fire2Held = Input.GetButton("Fire2");
        bool fire3Held = Input.GetButton("Fire1") && Input.GetButton("Fire2");

        
        bool fire1LetGo = !fire1Held && Input.GetButtonUp("Fire1");
        bool fire2LetGo = !fire2Held && Input.GetButtonUp("Fire2");
        bool fire3LetGo = !fire1Held && !fire2Held && (Input.GetButtonUp("Fire1") && Input.GetButtonUp("Fire2"));

        if (fire3Held)
        {
            //Debug.Log("Draw3");
        }
        else if (fire1Held)
        {
            //Debug.Log("Draw1");
        }
        else if (fire2Held)
        {
            //Debug.Log("Draw2");
        }

        if (fire3LetGo || (fire1LetGo && fire2LetGo))
        {
            var shot = (GameObject)Instantiate(bullet, transform.up * 5 + transform.position, transform.rotation);
            shot.GetComponent<Rigidbody2D>().velocity = transform.up * 200;
        }
        else if (fire1LetGo)
        {
            var shot = (GameObject)Instantiate(bullet, transform.right *3 + transform.position, transform.rotation * Quaternion.Euler(0,0,-90f));
            shot.GetComponent<Rigidbody2D>().velocity = transform.right * 50;
        }
        else if (fire2LetGo)
        {
            var shot = (GameObject)Instantiate(bullet, -transform.right * 3 + transform.position, transform.rotation * Quaternion.Euler(0, 0, 90f));
            shot.GetComponent<Rigidbody2D>().velocity = -transform.right * 50;
        }

        



    }

    private void Pulse()
    {
        halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
        haloBool = true;
        haloTimeStamp = Time.time;
        nextPulse = Time.time + pulseRate;
        GameEvents.GeneratePulse(body.position, pulseStrength, pulseRadius, body, false);
        if (haloBool == true && (Time.time - haloTimeStamp) > haloTimer)
        {
            halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
            haloBool = false;
        }
    }

    private void UsePlayerControls()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        if (horizontalMovement != 0f)
        {
            transform.Rotate(Vector3.forward * -horizontalMovement * rotateSpeed);
        }
        float verticalMovement = Input.GetAxis("Vertical");
        if (verticalMovement != 0f)
        {
            lineController.ResetLine();
            body.AddForce(transform.up * speed * verticalMovement);
        }
        else 
        {
            lineController.FadeLine();
        }
    }
}
