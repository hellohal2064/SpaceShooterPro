using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToggleHS : MonoBehaviour
{
    private Toggle _resetHS;
    private SystemManager _systemManager;
    // Start is called before the first frame update
    void Start()
    {
        _systemManager = GameObject.FindGameObjectWithTag("SystemManager").GetComponent<SystemManager>();
        _resetHS = this.GetComponent<Toggle>();
    }
    public void SetToggle()
    {
        _systemManager.ResetHS();
    }
    public void ClearToggle()
    {
        _resetHS.isOn = false;
    }
}
