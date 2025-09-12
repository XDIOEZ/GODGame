using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopDataUi : MonoBehaviour
{
    public Text fuelText;
    public Text maxFuelText;

    public ParameterFuel mianParameterFuel ;


    private void FixedUpdate()
    {
        maxFuelText.text = $"{mianParameterFuel.maxFuel}";
        FixFuelShow();
    }

    private void FixFuelShow()
    {
        fuelText.text = $"{(double)mianParameterFuel.fuel:F2}";
        if (mianParameterFuel.fuel > mianParameterFuel.maxFuel)
        {
            fuelText.text = maxFuelText.text;
        }
    }
}
