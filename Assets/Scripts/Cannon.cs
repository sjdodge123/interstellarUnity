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
    
    public override void Build(float rotationAngle)
    {
        this.rotationAngle = rotationAngle;
        if (this.rotationAngle > 0)
        {
            portOrStar = -portOrStar;
        }
    }


    public void Awake()
    {
        lineController = gameObject.GetComponentInChildren<LineController>();
        munitionBody = munition.GetComponent<Rigidbody2D>();
        lineController.buildObject(munitionBody);
    }

    public override void Aim()
    {
        lineController.ToggleOn();
    }

    public override void Fire()
    {
        lineController.ToggleOff();
        var shot = Instantiate(munition, portOrStar * this.transform.parent.right * 3 + this.transform.parent.position, this.transform.parent.rotation * Quaternion.Euler(0, 0, rotationAngle)) as GameObject;
        var shotBody = shot.GetComponent<Rigidbody2D>();
        shotBody.velocity = portOrStar * this.transform.parent.right * fireVelocity;
        shotBody.velocity += this.transform.parent.GetComponent<Rigidbody2D>().velocity;

    }
}
