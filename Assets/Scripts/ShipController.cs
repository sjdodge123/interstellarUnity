using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ShipController : MonoBehaviour
{
    public GameObject weapon1;
    public GameObject weapon2;

    public Color playerColor;


    private Weapon portWeaponController;
    private Weapon starWeaponController;
    private FuelController fuelController;

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

    public float haloTimer;
    private float haloTimeStamp;
    private bool haloBool = false;

    private LineController lineController;

    private Component halo;
    private Rigidbody2D body;
    private Vector3 spawnPosition;
    


    // Use this for initialization
    void Awake()
    {
        spawnPosition = gameObject.transform.position;

        body = GetComponent<Rigidbody2D>();
        halo = GetComponent("Halo");

        lineController = gameObject.GetComponentInChildren<LineController>();
        lineController.buildObject(body);

        weapon1 = Instantiate(weapon1);
        weapon1.transform.parent = transform;
        portWeaponController = weapon1.GetComponent<Weapon>();

        weapon2 = Instantiate(weapon2);
        weapon2.transform.parent = transform;
        starWeaponController = weapon2.GetComponent<Weapon>();

        portWeaponController.Build(90, playerColor);
        starWeaponController.Build(-90, playerColor);

        fuelController = gameObject.GetComponent<FuelController>();
        fuelController.setColor(playerColor);
    }
    public void Start()
    {
        lineController.setStartColor(playerColor);
        lineController.setEndColor(playerColor);
        
    }



    public void OnEnable()
    {
        GameVars.Camera.AddToCamera(gameObject);
        fuelController.resetFuel();
    }

    void FixedUpdate()
    {
        CheckCooldowns();
    }


    public void Pulse()
    {
        if (Time.time > nextPulse)
        {
            halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
            haloBool = true;
            haloTimeStamp = Time.time;
            nextPulse = Time.time + pulseRate;
            GameEvents.GeneratePulse(body.position, pulseStrength, pulseRadius, body, false);
        }

    }

    public void MoveHorizontal(float horizontalMovement)
    {
        transform.Rotate(Vector3.forward * -horizontalMovement * rotateSpeed);
    }

    public void MoveVertical(float vertical)
    {
        lineController.ToggleOn();
        fuelController.BurnFuel(vertical);
        if (fuelController.currentFuel > 0)
        {
            body.AddForce(transform.up * speed * vertical);
        }
       
    }

    public void RotateToAngle(float thisHorAxis, float thisVertAxis)
    {
        var angleDest = Mathf.Repeat(Mathf.Atan2(-thisHorAxis, -thisVertAxis) * Mathf.Rad2Deg, 360f);
        var angleStart = transform.eulerAngles.z;
        var angleDiff = Math.Abs(Mathf.DeltaAngle(angleStart, angleDest));
        if ( angleDiff > 0.5f)
        {
            var lerpTime =  Time.deltaTime * rotateSpeed ;
            var lerpAngle = Mathf.LerpAngle(angleStart, angleDest, lerpTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, lerpAngle);
        }
    }

    public void AimStarboard()
    {
        starWeaponController.Aim();
    }
    public void AimPort()
    {
        portWeaponController.Aim();
    }
    public void FireStarboard()
    {
        starWeaponController.Fire();
    }
    public void FirePort()
    {
        portWeaponController.Fire();
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Planet"))
        {
            IDied();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("GameBound"))
        {
            IDied();
        }
    }


    public Vector3 SpawnPosition
    {
        set { spawnPosition = value; }
        get { return spawnPosition; }
    }

    private void AntiGravity()
    {
        if (Time.time > nextAntiGrav)
        {
            nextAntiGrav = Time.time + antiGravRate;
            gameObject.layer = 9;
            antiGravDurationEnd = Time.time + antiGravDuration;
        }

    }

    private void CheckCooldowns()
    {
        if (Time.time > antiGravDurationEnd)
        {
            gameObject.layer = 0;
        }

        if (haloBool == true && (Time.time - haloTimeStamp) > haloTimer)
        {
            halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
            haloBool = false;
        }
    }

    private void IDied()
    {
        GameVars.GameController.SomethingDied(gameObject);
    }
}
