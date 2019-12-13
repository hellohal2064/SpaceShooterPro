using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingSlider : MonoBehaviour
{
    private Slider _sliderBar;
    private SystemManager _systemManager;
    private float volumeSetting;
    // Start is called before the first frame update
    void Start()
    {
        _systemManager = GameObject.FindGameObjectWithTag("SystemManager").GetComponent<SystemManager>();
        _sliderBar = this.GetComponent<Slider>();
        if (_sliderBar.value != _systemManager.GameVolume)
        {
            _sliderBar.value = _systemManager.GameVolume;
        }
    }
    public void SetLevel()
    {
        _systemManager.GameVolume = _sliderBar.value;
    }
}
