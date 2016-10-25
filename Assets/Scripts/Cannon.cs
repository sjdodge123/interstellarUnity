using UnityEngine;
using System.Collections;
using System;

public class Cannon : Weapon
{
    public GameObject munition;
    public float recoverTime = 4f;
    public float fireVelocity;

    private LineController lineController;
    private Rigidbody2D munitionBody;
    private float rotationAngle;
    private int portOrStar = 1;
    private Rigidbody2D parentBody;
    private Color trackingColor;
    
    private float cooldownTimer = 0.0f;

    public override void Build(float rotationAngle, Color trackingColor)
    {
        this.rotationAngle = rotationAngle;
        if (this.rotationAngle > 0)
        {
            portOrStar = -portOrStar;
        }

        this.trackingColor = trackingColor;
    }


    public void Awake()
    {
        lineController = gameObject.GetComponentInChildren<LineController>();
    }

    public void Start()
    {
        parentBody = this.transform.parent.GetComponent<Rigidbody2D>();
        SetTrackingColor(trackingColor, trackingColor);
    }

    public override void Aim()
    {
        if (munitionBody == null)
        {
            munitionBody = gameObject.AddComponent<Rigidbody2D>(); //empty rigid body. Doesn't have bullet properties
        }
        munitionBody.velocity = FindMunitionVelocity();
        munitionBody.position = FindMunitionPosition();
        munitionBody.tag = munition.tag;
        lineController.buildObject(munitionBody);
        lineController.ToggleOn();
    }

    public override void Fire()
    {
        //Hide Active Aimline
        Destroy(munitionBody);
        munitionBody = null;
        lineController.HideLine();
        if(Time.time > cooldownTimer)
        {
            cooldownTimer = Time.time + recoverTime;
            var shot = Instantiate(munition, FindMunitionPosition(), this.transform.parent.rotation * Quaternion.Euler(0, 0, rotationAngle)) as GameObject;
            shot.GetComponent<Rigidbody2D>().velocity = FindMunitionVelocity();
        }
    }

    private Vector2 FindMunitionVelocity()
    {
        Vector2 velocity;
        velocity = portOrStar * this.transform.parent.right * fireVelocity;
        velocity += parentBody.velocity;
        return velocity;
    }

    private Vector2 FindMunitionPosition()
    {
        return portOrStar * this.transform.parent.right * 5 + this.transform.parent.position;
    }
    public void SetTrackingColor(Color startColor, Color endColor)
    {
        lineController.setStartColor(startColor);
        lineController.setEndColor(endColor);
    }
}
