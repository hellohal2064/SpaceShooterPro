using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Powerup : MonoBehaviour
{
    [System.Serializable]
    public class PowerupRanges
    {
        [HideInInspector]
        public string name;
        [SerializeField]
        public string powerupName;
        [SerializeField]
        public float start;
        [SerializeField]
        public float end;
        [SerializeField]
        public bool pausePower;
    }

    [SerializeField]
    private float _ySpeed = 3.5f;
    [SerializeField]
    private float _yTopBound = 7.5f;
    // Sreen player bottom .x boundry
    [SerializeField]
    private float _yBottomBound = -6f;
    // Screen player left .y boundry
    [SerializeField]
    private float _xLeftBound = -9.0f;
    // Screen player right .y boundry
    [SerializeField]
    private float _xRightBound = 9.0f;
    [SerializeField]
    private List<PowerupRanges> _powerupTypes = new List<PowerupRanges>()
    {
        new PowerupRanges{name = "TripleShot Range", powerupName = "TripleShot(Clone)", start=-10f, end=5f, pausePower = true},
        new PowerupRanges{name = "Speed Range", powerupName = "Speed(Clone)", start=-10, end=10f, pausePower = true},
        new PowerupRanges{name = "Shield Range", powerupName = "Shield(CLone)", start=-10f, end=10f, pausePower = true}
    };
    //Audio Manager
    [SerializeField]
    private AudioClip _powerupSound = null;
    //Not in Unity
    private bool powerupLoop = true;
    private IEnumerator coroutinePowerUp;
    private float _timePass = 0;
    private int _powerNumber;
    private Player _player;
    //Audio Manger
    private AudioSource _playerAudio;
    // Start is called before the first frame update
    void Start()
    {
        //Audio Manager
        _playerAudio = GetComponent<AudioSource>();
        _playerAudio.clip = _powerupSound;
        //Power Start
        this.transform.position = new Vector3(Random.Range(_xLeftBound, _xRightBound), _yTopBound + 1, 0);
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            if (transform.position.y < _yBottomBound)
            {
                transform.position = new Vector3(Random.Range(_xLeftBound, _xRightBound), _yTopBound + 1, 0);
                _powerupTypes[_powerNumber].pausePower = true;
            }
            else if (_powerupTypes[_powerNumber].pausePower == false)
            {
                transform.Translate(Vector3.down * _ySpeed * Time.deltaTime);
            }           
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (_player != null)
            {
                switch (_powerNumber)
                {
                    case 0:
                        _player.IsTripleShotActive = true;
                        break;
                    case 1:
                        _player.IsSpeedActive = true;
                        break;
                    case 2:
                        _player.ShieldControl(true);
                        break;
                    default:
                        Debug.Log("Bug in Switch Trigger");
                        break;
                }
            }
            transform.position = new Vector3(Random.Range(_xLeftBound, _xRightBound), _yTopBound + 1, 0);
            _powerupTypes[_powerNumber].pausePower = true;
            _playerAudio.Play();
        }
        else if (collision.tag == "PowerUp")
        {
            transform.position = new Vector3(Random.Range(_xLeftBound, _xRightBound), _yTopBound + 1, 0);
        }
    }
    IEnumerator PowerUpFlow(float powerupTimer)
    {
        while (powerupLoop)
        {
            if (_player != null)
            {
                switch (_powerNumber)
                {
                    case 0:
                        if (_player.IsTripleShotActive)
                        {
                            float _randomHold = Random.Range(_powerupTypes[_powerNumber].start, _powerupTypes[_powerNumber].end);
                            if (_randomHold > -2 && _randomHold < 2)
                            {
                                _player.IsTripleShotActive = false;
                            }
                        }
                        else
                        {
                            float _randomHold = Random.Range(_powerupTypes[_powerNumber].start, _powerupTypes[_powerNumber].end);
                            if (_randomHold > -2 && _randomHold < 2)
                            {
                                _powerupTypes[_powerNumber].pausePower = false;
                            }
                        }
                        break;
                    case 1:
                        if (_player.IsSpeedActive)
                        {
                            float _randomHold = Random.Range(_powerupTypes[_powerNumber].start, _powerupTypes[_powerNumber].end);
                            if (_randomHold > -2 && _randomHold < 2)
                            {
                                _player.IsSpeedActive = false;
                            }
                        }
                        else
                        {
                            float _randomHold = Random.Range(_powerupTypes[_powerNumber].start, _powerupTypes[_powerNumber].end);
                            if (_randomHold > -2 && _randomHold < 2)
                            {
                                _powerupTypes[_powerNumber].pausePower = false;
                            }
                        }
                        break;
                    case 2:
                        if (_player.ShieldControl())
                        {
                            if (_player.ShieldHit == 0) //_randomHold > -2 && _randomHold < 2
                            {
                                _player.ShieldControl(shieldactive:false, resetShields:true);
                            }
                        }
                        else if (!_player.ShieldControl())
                        {
                            float _randomHold = Random.Range(_powerupTypes[_powerNumber].start, _powerupTypes[_powerNumber].end);
                            if (_randomHold > -4 && _randomHold < 4) // _randomHold > -4 && _randomHold < 4
                            {
                                _powerupTypes[_powerNumber].pausePower = false;
                                _player.ShieldReset();
                            }
                        }
                        break;
                    default:
                        Debug.Log("Bug in Switch Trigger OFF");
                        break;
                }
            }
            yield return new WaitForSeconds(powerupTimer);
        }
    }
    public void SetStartup(GameObject powerupContainer, int location, float powerupTimer)
    {
        _powerNumber = location;
        _timePass = powerupTimer;
        if (powerupContainer != null)
        {
            this.gameObject.transform.parent = powerupContainer.transform;
        }
        else
        {
            Debug.Log("Startup Bug PowerContainer");
        }
        coroutinePowerUp = PowerUpFlow(_timePass);
        StartCoroutine(coroutinePowerUp);
    }
    public bool PausePower
    {
        get
        {
            return _powerupTypes[_powerNumber].pausePower;
        }
        set
        {
            _powerupTypes[_powerNumber].pausePower = value;
        }
    }
}
