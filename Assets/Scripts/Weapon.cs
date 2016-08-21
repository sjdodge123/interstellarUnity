using UnityEngine;
using System.Collections;
using System;

public abstract class Weapon : MonoBehaviour {

    public virtual void Build()
    {
        throw new NotImplementedException();
    }
    public virtual void Build(float rotationAngle, Color trackingColor)
    {
        throw new NotImplementedException();
    }
    public virtual void Aim()
    {
        throw new NotImplementedException();
    }
    public virtual void Fire()
    {
        throw new NotImplementedException();
    }
}
