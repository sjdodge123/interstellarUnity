using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ShipController : MonoBehaviour
{

    public GameObject bullet;
    public GameObject aimTracker;
    public GameObject pathTracker;

    public float speed;
    public float rotateSpeed;
    public float pulseRadius;
    public int playerNumber;

    public float pulseRate = 1F;
    public float nextPulse = 0.0F;
    public float pulseStrength;

    private float antiGravRate = 1f;
    private float nextAntiGrav = 0.0f;
    private float antiGravDuration = 2f;
    private float antiGravDurationEnd = 0.0f;


    private float bulletSpeed = 50f;

    public float haloTimer;
    private float haloTimeStamp;
    private bool haloBool = false;

    private LineController lineController;
    private LineController aimController;

    private Component halo;
    private Rigidbody2D body;
   

    // Use this for initialization
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        halo = GetComponent("Halo");
        aimController = aimTracker.GetComponent<LineController>();
        aimTracker.SetActive(false);
        lineController = pathTracker.GetComponent<LineController>();
       
        GameVars.Ships.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        UsePlayerControls();
        //Pulse Cannon
        if (Input.GetButtonDown("Jump") && Time.time > nextPulse)
        {
            Pulse();
        }

        bool fire1Held = Input.GetButton(playerNumber + "Fire1");
        bool fire2Held = Input.GetButton(playerNumber + "Fire2");
        bool fire3Held = Input.GetButton(playerNumber + "Fire3");
        bool fire4Held = Input.GetButton(playerNumber + "Fire1") && Input.GetButton(playerNumber + "Fire2");



        bool fire1LetGo = !fire1Held && Input.GetButtonUp(playerNumber + "Fire1");
        bool fire2LetGo = !fire2Held && Input.GetButtonUp(playerNumber + "Fire2");
        bool fire3LetGo = !fire3Held && Input.GetButtonUp(playerNumber + "Fire3");
        bool fire4LetGo = !fire1Held && !fire2Held && (Input.GetButtonUp(playerNumber + "Fire1") && Input.GetButtonUp(playerNumber + "Fire2"));




        if (fire3LetGo)
        {
            AntiGravity();
        }


        if (fire4Held)
        {
            //Debug.Log("Draw3");
        }
        else if (fire1Held)
        {
            aimTracker.SetActive(true);
            aimController.UpdateTrajectory(transform.right * 3 + transform.position, transform.right * bulletSpeed);
        }
        else if (fire2Held)
        {
            //Debug.Log("Draw2");
        }

        if (fire4LetGo || (fire1LetGo && fire2LetGo))
        {
            var shot = Instantiate(bullet, transform.up * 5 + transform.position, transform.rotation) as GameObject;
            shot.GetComponent<Rigidbody2D>().velocity = transform.up * bulletSpeed;
        }
        else if (fire1LetGo)
        {
            aimTracker.SetActive(false);
            var shot = Instantiate(bullet, transform.right * 3 + transform.position, transform.rotation * Quaternion.Euler(0, 0, -90f)) as GameObject;
            shot.GetComponent<Rigidbody2D>().velocity = transform.right * bulletSpeed;
        }
        else if (fire2LetGo)
        {
            var shot = Instantiate(bullet, -transform.right * 3 + transform.position, transform.rotation * Quaternion.Euler(0, 0, 90f)) as GameObject;
            shot.GetComponent<Rigidbody2D>().velocity = -transform.right * bulletSpeed;
        }
    }

    void FixedUpdate()
    {
        CheckCooldowns();
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
        float horizontalMovement = Input.GetAxis(playerNumber + "Horizontal");
        if (horizontalMovement != 0f)
        {
            transform.Rotate(Vector3.forward * -horizontalMovement * rotateSpeed);
        }
        float verticalMovement = Input.GetAxis(playerNumber + "Vertical");
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

    private void AntiGravity()
    {
        if(Time.time > nextAntiGrav)
        {
            nextAntiGrav = Time.time + antiGravRate;
            gameObject.layer = 9;
            antiGravDurationEnd = Time.time + antiGravDuration;
        }
       
    }

    private void CheckCooldowns()
    {
        if(Time.time > antiGravDurationEnd)
        {
            gameObject.layer = 0;
        }
    }
}
