using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class FuelController : MonoBehaviour {

    public Slider fuelSlider;
    public float totalFuel = 100;
    public float currentFuel = 100;

    public float burnRate;
    public float chargeRate;
    private Image fill;
    private Image background;
    private Image[] images;

    void Awake()
    {
        currentFuel = totalFuel;
        //fill = fuelSlider.GetComponentInChildren<Image>();
        images = fuelSlider.GetComponentsInChildren<Image>();
        foreach ( Image i in images)
        {
            if (i.name == "Background")
            {
                background = i;
            }
            else if (i.name == "Fill")
            {
                fill = i;
            }
        }
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
        var expectedFuel = currentFuel - burnRate * Math.Abs(vertical) * Time.deltaTime;
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

    internal void setColor(Color playerColor)
    {
        fill.color = playerColor;
        playerColor = new Color(playerColor.r, playerColor.g, playerColor.b, 0.5f);
        background.color = playerColor;
    }
    public void resetFuel()
    {
        currentFuel = totalFuel;
    }
}
