using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Willpower : MonoBehaviour
{
    public Slider slider;

    public void SetMaxWillpower(int will)
    {
        slider.maxValue = will;
        slider.value = will;
    }

    public void SetWillpower(int will)
    {
        slider.value = will;
    }



}
