using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameVars : MonoBehaviour {

    public const float GravityConstant = 20000;
    public static List<ShipController> Ships = new List<ShipController>();
    public static List<PlanetController> Planets = new List<PlanetController>();
}
