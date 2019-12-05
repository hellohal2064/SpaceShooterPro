using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FireDamage : MonoBehaviour
{
    [SerializeField]
    private bool _fireOn = false;

    //Not in Unity
    private Color32 _colorOff = new Color32(255, 255, 255, 0);
    private Color32 _colorOn = new Color32(255, 255, 255, 255);
    private Color32 _colorHold;
    //Ship Fire Loop
    private bool loopStartFire;
    private IEnumerator coroutineStartFire;

    // Start is called before the first frame update
    void Start()
    {
        _colorHold = GetComponent<SpriteRenderer>().color;
        if (_colorHold.Equals(_colorOn))
        {
            GetComponent<SpriteRenderer>().color = _colorOff;
        }
        // Create Thruster Objects
        loopStartFire = true;
        coroutineStartFire = StartFire();
        StartCoroutine(StartFire());
    }
    IEnumerator StartFire()
    {
        while (loopStartFire)
        {
            if (_fireOn)
            {
                GetComponent<SpriteRenderer>().color = _colorOn;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    public void FireON(bool _fireDamage)
    {
        if (!_fireOn && _fireDamage)
        {
            _fireOn = _fireDamage;

        }
    }
}
