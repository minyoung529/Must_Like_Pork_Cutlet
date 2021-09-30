using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();

        if(gameObject.name.Contains("BGM"))
        {
            slider.value = GameManager.Instance.CurrentUser.bgmVolume;
        }

        else
        {
            slider.value = GameManager.Instance.CurrentUser.effectSoundVolume;
        }
    }
}
