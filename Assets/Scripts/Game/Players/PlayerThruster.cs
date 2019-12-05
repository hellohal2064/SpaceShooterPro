using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerThruster : MonoBehaviour
{
    [SerializeField]
    private bool _thrusterON;
    [SerializeField]
    private AudioClip _thrusterSound = null;
    //Not in Unity
    private Color32 _colorOff = new Color32(255, 255, 255, 0);
    private Color32 _colorOn = new Color32(255, 255, 255, 255);
    private IEnumerator coroutineStartThruster;
    //Audio Manger
    private AudioSource _playerAudio;
    // Start is called before the first frame update
    void Start()
    {
        _thrusterON = false;
        GetComponent<SpriteRenderer>().color = _colorOff;
        //Audio Manager
        _playerAudio = GetComponent<AudioSource>();
        _playerAudio.clip = _thrusterSound;
        // Create Thruster Objects
        coroutineStartThruster = StartThruster();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _thrusterON = true;
            ThrusterControl(_thrusterON);
        }
    }
    void ThrusterControl(bool _thrusters)
    {
        if (_thrusters)
        {
                StartCoroutine(StartThruster(_thrusters));
        }
    }
    IEnumerator StartThruster([Optional] bool _thrusters)
    {
        float t = 0;
        _playerAudio.Play();
        while (t < 1)
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(_colorOff, _colorOn, t);
            yield return new WaitForFixedUpdate();
            t += Time.deltaTime / 1f;
            ThrusterON = true;
        }
        float s = 0;
        _playerAudio.Stop();
        while (s < 1)
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(_colorOn, _colorOff, s);
            yield return new WaitForFixedUpdate();
            s += Time.deltaTime / 1f;
            ThrusterON = true;
        }
        ThrusterON = false;
    }
    public bool ThrusterON
    {
        get
        {
            return _thrusterON;
        }
        set
        {
            _thrusterON = value;
        }
    }
}
