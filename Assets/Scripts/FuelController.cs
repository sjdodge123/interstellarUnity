using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FuelController : MonoBehaviour {

    public Slider fuelSlider;
    public float totalFuel = 100;
    public float currentFuel = 100;

    public float burnRate;
    public float chargeRate;

    void Awake()
    {
        currentFuel = totalFuel;
    }
    // Use this for initialization
    void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        var expectedFuel = currentFuel + chargeRate * Time.deltaTime;
        if (expectedFuel < totalFuel)
        {
            currentFuel = expectedFuel;
        }
        else
        {
            currentFuel = totalFuel;
        }
	}
    void FixedUpdate()
    {
        UpdateSlider();
    }

    public void BurnFuel(float vertical)
    {
        var expectedFuel = currentFuel - burnRate * vertical * Time.deltaTime;
        if (expectedFuel >= 0)
        {
            currentFuel = expectedFuel;
        }
        else
        {
            currentFuel = 0;
        }
        
    }
    private void UpdateSlider()
    {
        fuelSlider.value = currentFuel;
    }
}
