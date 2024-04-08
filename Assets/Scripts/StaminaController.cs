using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    public Slider staminaSlider;

    public void SetMaxStamina(float health)
    {
        staminaSlider.maxValue = health;
        staminaSlider.value = health;
    }

    public void SetStamina(float health)
    {
        staminaSlider.value = health;
    }
}
