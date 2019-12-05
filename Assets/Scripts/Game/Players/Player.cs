
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
[System.Serializable]
    public class LaserTypes
    {
        [HideInInspector]
        public string name;
        [SerializeField]
        public string laserName;
        [SerializeField]
        public float laserSpeed;
        [SerializeField]
        public float laserOffset;
        [SerializeField]
        public float laserMaxY;
        [HideInInspector]
        public float laserCooldown;
        [SerializeField]
        public float laserFirerate;
        [SerializeField]
        public GameObject laserPrefab;
        [SerializeField]
        public GameObject LaserContainer;
    }

    [System.Serializable]
    public class PowerupTypes
    {
        [HideInInspector]
        public string name;
        [SerializeField]
        public bool powerupIsActive;
        [SerializeField]
        public string powerupName;
        [SerializeField]
        public GameObject powerupContainer;
    }

    [System.Serializable]
    public class PlayerInfo
    {
        [HideInInspector]
        public string name;
        [SerializeField]
        public int playerLives;
        [SerializeField]
        public float playerSpeed;
        [SerializeField]
        public float boostSpeedX;
    }

    //lives Value Control
    [SerializeField]
    private List<PlayerInfo> _playerInfo = new List<PlayerInfo>
    {
        new PlayerInfo{name = "First Player", playerLives = 3, playerSpeed = 3.5f, boostSpeedX = 2f}
    };
    
    //Not in Unity
    private int _livesHold = 0;
    private  string[] _tagName = new string[]{ "Laser", "TripleShot", "Shields", "PowerUp" };
    // Player Object Control VAR
    // Screen player top .x boundry
    [SerializeField]
    private float _yTopBound = 0.0f;
    // Sreen player bottom .x boundry
    [SerializeField]
    private float _yBottomBound = -3.8f;
    // Screen player left .y boundry
    [SerializeField]
    private float _xLeftBound = -11.1f;
    // Screen player right .y boundry
    [SerializeField]
    private float _xRightBound = 11.1f;
    //Not in Unity
    private bool __ThrusterON;
    private Vector3 _movePlayer;
    private Vector3 _pHold;
    private float _fZero = 0.0f;
    private float _speedHold;
    private UIManager _uiManager;
    private FireDamage[] _fireDamage;
    private PlayerThruster _playerThruster;

    //PowerUp Var
    [SerializeField]
    private List<PowerupTypes> _powerupTypes = new List<PowerupTypes>()
    {
        new PowerupTypes{name = "Is TripleShot Active", powerupIsActive = false, powerupName = "TripleShot(Clone)"},
        new PowerupTypes{name = "Is SpeedBoost Active", powerupIsActive = false, powerupName = "Speed(Clone)"},
        new PowerupTypes{name = "Is Shield Active", powerupIsActive = false, powerupName = "Shield(Clone)"}
    };

    // Laser PreFab Object Control VAR
    [SerializeField]
    private List<LaserTypes> _laserTypes = new List<LaserTypes>()
    {
        new LaserTypes{name = "Single Laser", laserSpeed = 8f, laserOffset = 1.05f, laserMaxY = 7f, laserCooldown = -1f, laserFirerate = 0.2f, laserName = "Laser(Clone)"},
        new LaserTypes{name = "Triple Laser", laserSpeed = 8f, laserOffset = 1.05f, laserMaxY = 7f, laserCooldown = -1f, laserFirerate = 0.2f, laserName = "TripleLaser(Clone)"}
    };
    
    // Not in Unity
    private LivesCard _LivesCard;
    //Shields
    private int _shieldMax = 6;
    private int _shieldLive;
    private ShieldBar _ShieldBar;
    // Start is called before the first frame update
    void Start()
    {
        //Damage Object
        _fireDamage = GetComponentsInChildren<FireDamage>();
        //UNity Containers
        GameObject _PlayerContainer = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>().PlayerContainer;
        GameObject _LaserContainer = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>().LaserContainer;
        GameObject holdLaserContainer = GameObject.Find("Player Container");
        SetContainer(_PlayerContainer, _LaserContainer);
        //Thrusters
        _playerThruster = GetComponentInChildren<PlayerThruster>();
        _uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        //LivesCard
        _LivesCard = GameObject.FindGameObjectWithTag("LivesCard").GetComponent<LivesCard>();
        _LivesCard.RestSprite(true);
        //Default Shield OFF and Defaults
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        _shieldLive = _shieldMax;
        _ShieldBar = GameObject.FindGameObjectWithTag("ShieldBar").GetComponent<ShieldBar>();
        _ShieldBar.ShieldReset();
        //Set play start position
        _livesHold = _playerInfo[0].playerLives;
        _speedHold = _playerInfo[0].playerSpeed;

        if (holdLaserContainer != null)
        {
            for (int i = 0; i < _laserTypes.Count; i++)
            {
                _laserTypes[i].LaserContainer = GameObject.Find("Player Container");
            }
        }
        else
        {
            Debug.Log("Startup Bug LaserContainer");
        }

        GameObject holdPowerContainer = GameObject.Find("Powerup Container");
        if (holdPowerContainer != null)
        {
            for (int i = 0; i < _laserTypes.Count; i++)
            {
                _powerupTypes[i].powerupContainer = GameObject.Find("Powerup Container");
            }
        }
        else
        {
            Debug.Log("Startup Bug PowerContainer");
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        //FireLaser
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (_powerupTypes[0].powerupIsActive == true && Time.time > _laserTypes[1].laserCooldown)
            {
                FireLaser(_laserTypes, _powerupTypes, _laserTypes[1].laserMaxY);
            }
            else if (_powerupTypes[0].powerupIsActive == false && Time.time > _laserTypes[0].laserCooldown)
            {
                FireLaser(_laserTypes, _powerupTypes, _laserTypes[0].laserMaxY);
            }
        }
    }
    void MovePlayer()
    {
        // Player Motion Inputs
        //CrossPlatformInputManager.
        float _horizontalPlayer = Input.GetAxis("Horizontal");
        float _verticalPlayer = Input.GetAxis("Vertical");
        //Player Motion Inputs (Vertical Bounding)
        _pHold.Set(transform.position.x, Mathf.Clamp(transform.position.y, _yBottomBound, _yTopBound), _fZero);
        transform.position = _pHold;
        //Player Motion Inputs (Horizontal Bounding)
        if (transform.position.x > _xRightBound)
        {
            _pHold.Set(_xLeftBound, transform.position.y, _fZero);
            transform.position = _pHold;
        }
        else if (transform.position.x < _xLeftBound)
        {
            _pHold.Set(_xRightBound, transform.position.y, _fZero);
            transform.position = _pHold;
        }
        // Move Player
        _movePlayer.Set(_horizontalPlayer, _verticalPlayer, _fZero);
        if (_powerupTypes[1].powerupIsActive)
        {
            if (_speedHold == _playerInfo[0].playerSpeed)
            {
                _speedHold = _playerInfo[0].playerSpeed * _playerInfo[0].boostSpeedX;
                transform.Translate(_movePlayer * _speedHold * Time.deltaTime);
            }
            else
            {
                transform.Translate(_movePlayer * _speedHold * Time.deltaTime);
            }
        }
        else if (!_powerupTypes[1].powerupIsActive)
        {
            if (_speedHold != _playerInfo[0].playerSpeed)
            {
                _speedHold = _playerInfo[0].playerSpeed;
                transform.Translate(_movePlayer * _speedHold * Time.deltaTime);
            }
            else
            {
                transform.Translate(_movePlayer * _speedHold * Time.deltaTime);
            }
        }
        else
        {
            Debug.Log("Speed boost Bug");
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    { 
        bool[] collisionMaster = { false, false, false, false };
        bool[] collisionState = { false, false, false, false };
        int t = 0;

        foreach (var item in _tagName)
        {
            collisionState[t] = collision.CompareTag(item);
            if (collisionState[t])
            {
                break;
            }
            t++;
        }
        
        if (collisionMaster.SequenceEqual(collisionState))
        {
            var heading = collision.transform.position - transform.position;
            var distance = heading.magnitude;
            var direction = heading / distance;
            if (direction.x > 0)
            {
                _fireDamage[0].FireON(true);
            }
            else if (direction.x < 0)
            {
                _fireDamage[1].FireON(true);
            }
        }
    }
    void FireLaser(List<LaserTypes> laserList, List<PowerupTypes> powerupList, float laserBound)
    {
        if (powerupList[0].powerupIsActive)
        {
            laserList[1].laserCooldown = Time.time + laserList[1].laserFirerate;
            GameObject newLaser = Instantiate(laserList[1].laserPrefab, transform.position + new Vector3(0, laserList[1].laserOffset, 0), Quaternion.identity);
            foreach(Transform laserHold in newLaser.GetComponentInChildren<Transform>(false))
            {
                if (newLaser != null)
                {
                    laserHold.GetComponent<Laser>().LaserBound = laserBound;
                    laserHold.GetComponent<Laser>().TripleShotActive(powerupList[0].powerupIsActive);
                    laserHold.GetComponent<Rigidbody2D>().velocity = transform.up * laserList[1].laserSpeed;
                }
                else
                {
                    Debug.Log("TripleShot Laser is broken");
                }
            }
            if (laserList[1].LaserContainer != null)
            {
                newLaser.transform.parent = laserList[1].LaserContainer.transform;
            }
            else
            {
                Debug.Log("TripleShot Laser Container broken");
            }
        }
        else if (powerupList[0].powerupIsActive == false)
        {
            laserList[0].laserCooldown = Time.time + laserList[0].laserFirerate;
            GameObject newLaser = Instantiate(laserList[0].laserPrefab, transform.position + new Vector3(0, laserList[0].laserOffset, 0), Quaternion.identity);
            if (newLaser != null)
            {
                newLaser.GetComponent<Laser>().LaserBound = laserBound;
                newLaser.GetComponent<Laser>().TripleShotActive(_powerupTypes[0].powerupIsActive);
                newLaser.GetComponent<Rigidbody2D>().velocity = transform.up * laserList[0].laserSpeed;
            }
            else
            {
                Debug.Log("Sigle Laser is broken");
            }

            if (laserList[0].LaserContainer != null)
            {
                newLaser.transform.parent = laserList[0].LaserContainer.transform;
            }
            else
            {
                Debug.Log("Sigle Laser Container broken");
            }
        }
    }
    public bool ShieldControl([Optional]bool shieldactive, [Optional] bool scaleshields, [Optional] bool resetShields)
    {
        GameObject myChild = this.gameObject.transform.GetChild(0).gameObject;
        Vector3 scaleDefault = new Vector3(1.8f, 1.8f, 1.8f);
        if (shieldactive && !scaleshields && !resetShields)
        {
            _powerupTypes[2].powerupIsActive = shieldactive;
            myChild.SetActive(shieldactive);
            _ShieldBar.ShieldMax();
            return _powerupTypes[2].powerupIsActive;
        }
        else if (shieldactive && scaleshields && !resetShields)
        {
            Vector3 scaleHold = myChild.transform.localScale;
            scaleHold -= new Vector3(0.2f, 0.2f, 0.2f);
            if (scaleHold.x <= 0.9f)
            {
                myChild.SetActive(false);
                myChild.transform.localScale = scaleDefault;
                _ShieldBar.ShieldReset();
                _powerupTypes[2].powerupIsActive = false;
            }
            else
            {
                myChild.transform.localScale = scaleHold;
                _ShieldBar.ShieldHit();
                _powerupTypes[2].powerupIsActive = shieldactive;
            }
            return _powerupTypes[2].powerupIsActive;
        }
        else if (!shieldactive && !scaleshields && resetShields)
        {
            myChild.SetActive(shieldactive);
            myChild.transform.localScale = scaleDefault;
            _ShieldBar.ShieldReset();
            _powerupTypes[2].powerupIsActive = shieldactive;
            return _powerupTypes[2].powerupIsActive;
        }
        else
        {
            return _powerupTypes[2].powerupIsActive;
        }
    }
    void ShieldHitCount()
    {
        if (_shieldLive > 0)
        {
            _shieldLive--;
            //Debug.LogError(_shieldLive);
        }
    }
    public void ShieldReset()
    {
        _shieldLive = _shieldMax;
    }
    public void Damage(string DamageTo)
    {
        switch (DamageTo)
        {
            case "Shields":
                ShieldHitCount();
                ShieldControl(shieldactive: true, scaleshields: true);
                break;
            case "Player":
                _livesHold--;
                _LivesCard.ActivateSprite(_livesHold);
                if (_livesHold == 0)
                {
                    _livesHold = _playerInfo[0].playerLives;
                    _uiManager.GameOverCheck = true;
                    Destroy(this.gameObject);
                }
                break;
            default:
                break;
        }
    }
    public void SetContainer(GameObject playerContainer, GameObject laserContainer)
    {
        if (playerContainer != null)
        {
            this.gameObject.transform.parent = playerContainer.transform;
        }
        if (laserContainer != null)
        {
            for (int i = 0; i < _laserTypes.Count; i++)
            {
                _laserTypes[i].LaserContainer = laserContainer;
            }
        }
        else
        {
            Debug.Log("Startup Bug LaserContainer");
        }
    }
    public bool IsTripleShotActive
    {
        get
        {
            return _powerupTypes[0].powerupIsActive;
        }
        set
        {
            _powerupTypes[0].powerupIsActive = value;
        }
    }
    public bool IsSpeedActive
    {
        get
        {
            return _powerupTypes[1].powerupIsActive;
        }
        set
        {
            _powerupTypes[1].powerupIsActive = value;
        }
    }
    public int ShieldHit
    {
        get
        {
            return _shieldLive;
        }
        set
        {
           _shieldLive = value;
        }
    }
}
