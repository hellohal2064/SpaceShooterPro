using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ShieldBar : MonoBehaviour
{
    [SerializeField]
    private float _shieldBarX;
    [SerializeField]
    private float _shieldBarY;

    private float _barMax = 74f;
    private float _barMin = -25f;
    private float _changeAmount = 15f;
    private RectTransform _ShieldBar;
    private float _barHold;
    // Start is called before the first frame update
    void Start()
    {
        _shieldBarX = _barMin;
        _shieldBarY = 10f;
        _ShieldBar = GetComponent<RectTransform>();
        ChangeBar(shieldReset: true);
        _barHold = _barMax;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void ChangeBar([Optional] bool shieldHit, [Optional] bool shieldMax, [Optional] bool shieldReset)
    {
        //Debug.LogError(shieldHit + "-" + shieldMax + "-" + shieldReset);
        if (shieldHit)
        {
            //Debug.Log("Hit L1: " + _changeAmount + "-" + _shieldBarX);
            _barHold -=_changeAmount;
            _shieldBarX = _barHold;
            //Debug.Log("Hit L2: " + _shieldBarX + "-" + _barHold);
            Vector2 maxX = new Vector2(_shieldBarX, _shieldBarY);
            _ShieldBar.offsetMax = maxX;
        }
        else if (shieldMax)
        {
            //Debug.Log("Max L1: " + _shieldBarX);
            _shieldBarX = _barMax;
            _barHold = _barMax;
            //Debug.Log("Max L2: " + _shieldBarX);
            Vector2 maxX = new Vector2(_shieldBarX, _shieldBarY);
            _ShieldBar.offsetMax = maxX;
        }
        else if (shieldReset)
        {
            _shieldBarX = _barMin;
            _barHold = _barMax;
            Vector2 maxX = new Vector2(_shieldBarX, _shieldBarY);
            _ShieldBar.offsetMax = maxX;
        }
    }
    public void ShieldHit()
    {
        ChangeBar(shieldHit: true);
    }
    public void ShieldMax()
    {
        ChangeBar(shieldMax: true);
        
    }
    public void ShieldReset()
    {
        ChangeBar(shieldReset: true);
    }
}
