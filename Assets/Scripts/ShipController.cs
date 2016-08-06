using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ShipController : MonoBehaviour
{

    public Weapon portWeapon;
    public Weapon starWeapon;

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
        portWeapon.Build(transform,-90);
        starWeapon.Build(transform, 90);
    }

    public void OnEnable()
    {
        GameVars.Camera.AddToCamera(gameObject);
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
        body.AddForce(transform.up * speed * vertical);
    }

    public void AimStarBoard()
    {
        starWeapon.Aim();
    }
    public void AimPort()
    {
        portWeapon.Aim();
    }
    public void FireStarBoard()
    {
        starWeapon.Fire();
    }
    public void FirePort()
    {
        portWeapon.Fire();
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Planet"))
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
