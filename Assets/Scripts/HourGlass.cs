using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HourGlass : MonoBehaviour
{
    public Slider slider;
    public bool flip = false;
    public float max_willpower = 100;
    public float current_willpower;

    public void SetMaxWillpower(float will)
    {
        if(flip)
        {
            slider.maxValue = will;
            slider.value = 0;
        }
        else
        {
            slider.maxValue = will;
            slider.value = will;
        }
    }
    public void Reduce(float amount)
    {
        if (!flip)
        {
            slider.value -= amount;
        }
        else
        {
            slider.value += amount;
        }
    }
}
