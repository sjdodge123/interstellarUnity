using UnityEngine;
using System.Collections;
using System;

public class Cannon : Weapon
{
    public GameObject munition;
    public float fireVelocity;

    private LineController lineController;
    private Rigidbody2D munitionBody;
    private float rotationAngle;
    private int portOrStar = 1;
    private Rigidbody2D parentBody;
    private Color trackingColor;

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
            munitionBody = gameObject.AddComponent<Rigidbody2D>();
        }

        munitionBody.velocity = FindMunitionVelocity();
        munitionBody.position = FindMunitionPosition();

        lineController.buildObject(munitionBody);
        lineController.ToggleOn();
    }

    public override void Fire()
    {
        Destroy(munitionBody);
        munitionBody = null;
        lineController.HideLine();
        var shot = Instantiate(munition, FindMunitionPosition(), this.transform.parent.rotation * Quaternion.Euler(0, 0, rotationAngle)) as GameObject;
        shot.GetComponent<Rigidbody2D>().velocity = FindMunitionVelocity();
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
        return portOrStar * this.transform.parent.right * 3 + this.transform.parent.position;
    }
    public void SetTrackingColor(Color startColor, Color endColor)
    {
        lineController.setStartColor(startColor);
        lineController.setEndColor(endColor);
    }
}
