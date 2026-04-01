using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    public Slider audioSlider;
    private void Start()
    {
        
        audioSlider.value=AudioListener.volume;//初始滑块值等于全局音量
        audioSlider.onValueChanged.AddListener(OnValueChanged);
    }
    private void OnValueChanged(float value)
    {
        AudioListener.volume=value;
    }
}
