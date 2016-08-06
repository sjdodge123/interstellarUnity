using UnityEngine;
using System.Collections;
using System;

public class Cannon : Weapon
{
    public GameObject munition;
    public float fireVelocity;

    private LineController lineController;
    private Rigidbody2D munitionBody;
    private Transform shipTransform;
    private float rotationAngle;
    private int portOrStar = 1;
    

    public void Awake()
    {
        lineController = gameObject.GetComponentInChildren<LineController>();
        munitionBody = munition.GetComponent<Rigidbody2D>();
        lineController.buildObject(munitionBody);
    }

    public override void Build(Transform shipTransform, float rotationAngle)
    {
        this.shipTransform = shipTransform;
        this.rotationAngle = rotationAngle;
        if(this.rotationAngle > 0)
        {
            portOrStar = -portOrStar;
        }
    }

    public override void Aim()
    {
        lineController.ToggleOn();
    }

    public override void Fire()
    {
        lineController.ToggleOff();
        var shot = Instantiate(munition, portOrStar * shipTransform.right * 3 + shipTransform.position, shipTransform.rotation * Quaternion.Euler(0, 0, rotationAngle)) as GameObject;
        shot.GetComponent<Rigidbody2D>().velocity = portOrStar * shipTransform.right * fireVelocity;
    }
}
