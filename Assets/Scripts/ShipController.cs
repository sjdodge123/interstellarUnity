using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ShipController : MonoBehaviour {

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

        if(haloBool == true && (Time.time - haloTimeStamp) > haloTimer)
        {
            halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
            haloBool = false;
        }
            
    }

    private void Pulse()
    {
        halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
        haloBool = true;
        haloTimeStamp = Time.time;
        nextPulse = Time.time + pulseRate;
        GameEvents.GeneratePulse(body.position, pulseStrength, pulseRadius, body, false);   
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
