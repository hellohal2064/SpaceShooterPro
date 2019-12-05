using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private  bool tripleShotActive = false;
    [SerializeField]
    private float laserBound = 7;
    //Audio
    [SerializeField]
    private AudioClip _laserSound = null;
    //Not in Unity
    private GameObject _player = null;
    private string _laserName = "Laser(Clone)";
    //Audio Manger
    private AudioSource _playerAudio;
    private bool _playedAudio = false;

    void Start()
    {
        //Audio Manager
        _playerAudio = GetComponent<AudioSource>();
        _playerAudio.clip = _laserSound;
        if (GameObject.FindWithTag("Player") != null)
        {
            _player = GameObject.FindWithTag("Player");
        }
        else
        {
            Debug.Log("Laser Start - This is a bug");
        }
        //Audio Play
        if (!_playedAudio)
        {
            _playerAudio.Play();
            _playedAudio = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
       if (transform.position.y >= laserBound)
        {
            KillLaser(_laserName);
        }
       else if (_player == null)
        {
            KillLaser(_laserName);
        }
    }
    public void KillLaser(string laserName)
    {
        if (laserName == "TripleLaser(Clone)")
        { 
            GameObject killHold = GameObject.Find(laserName);
            if (killHold != null)
            {
                Destroy(killHold);
            }
            else
            {
                Debug.Log("This is a bug");
            }
        }
        else if (laserName == "Laser(Clone)")
        {
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("Kill Laser bug");
        }
    }

    public bool TripleShotActive(bool active)
    {
        if (active)
        {
            tripleShotActive = active;
            _laserName = "TripleLaser(Clone)";
        }
        else if (!active)
        {
            tripleShotActive = active;
            _laserName = "Laser(Clone)";
        }
        else
        {
            Debug.Log("TripleLaser Bug");
        }
        return tripleShotActive;
    }
    public float LaserBound
    {
        get
        {
            return laserBound;
        }
        set
        {
            laserBound = value;
        }
    }
}