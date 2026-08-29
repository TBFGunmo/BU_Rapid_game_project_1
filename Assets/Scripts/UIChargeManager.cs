using UnityEngine;
using UnityEngine.UI;

public class UIChargeManager : MonoBehaviour  
{
    public Slider chargeSlider; 

    void Start()
    {
        chargeSlider.value = 0f;
    }

    public void UpdateChargeBar(float currentForce, float maxForce)
    {
        chargeSlider.minValue = 0f;

        chargeSlider.maxValue = maxForce;

        chargeSlider.value = currentForce;
    }

    public void ResetChargeBar()
    {
        chargeSlider.value = 0f;
    }

}