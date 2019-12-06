using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class PowerUp
    {
        [HideInInspector]
        public string name;
        [SerializeField]
        public string powerupName;
        [HideInInspector]
        public int location;
        [SerializeField]
        public float powerupTimer;
        [SerializeField]
        public Rigidbody2D PowerupPrefab;
    }

    [System.Serializable]
    public class GameContainers
    {
        [HideInInspector]
        public string name;
        [SerializeField]
        public GameObject container;
    }

    [SerializeField]
    private GameObject _player = null;
    [SerializeField]
    private Rigidbody2D _enemy = null;
    [SerializeField]
    private float _spawnEnemyWaitTime = 5f;
    [SerializeField]
    private GameObject _asteroid = null;
    [SerializeField]
    private float _spawnAsteroidWaitTime = 25f;
    [SerializeField]
    private List<GameContainers> _gameContainers = new List<GameContainers>()
    {
        new GameContainers{name = "Player Container",container = null},
        new GameContainers{name = "Laser Container",container = null},
        new GameContainers{name = "Powerup Container",container = null},
        new GameContainers{name = "Enemy Container",container = null},
        new GameContainers{name = "Asteroid Container",container = null}
    };
    [SerializeField]
    private List<PowerUp> _powerup = new List<PowerUp>()
    {
        new PowerUp{name="TripleShot", powerupName = "TripleShot(Clone)", location = 0, powerupTimer = 5},
        new PowerUp{name="Speed", powerupName = "Speed(Clone)", location = 1, powerupTimer = 5},
        new PowerUp{name="Shield", powerupName = "Shield(CLone)", location = 2, powerupTimer = 5}
    };

    [SerializeField]
    private bool _EnemysOff = false;
    //Coroutines
    private bool GamePlayLoop = true;
    private IEnumerator coroutineGamePlay;
    private bool spawnEnemyLoop = true;
    private IEnumerator coroutineSpawnEnemy;
    private bool spawnAsteroidLoop = true;
    private IEnumerator coroutineSpawnAsteroid;
    private bool spawnPowerUpLoop = true;
    private IEnumerator coroutineSpawnPowerUp;
    private bool spawnPlayerLoop = true;
    private IEnumerator coroutineSpawnPlayer;
    //Not In Unity
    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        _uiManager.GameOverCheck = false;
        // Create Game Play Loop
        coroutineGamePlay = GamePlay();
        StartCoroutine(coroutineGamePlay);
        // Create Player
        coroutineSpawnPlayer = SpawnPlayer();
        StartCoroutine(coroutineSpawnPlayer);
        // Create Enemy Objects
        coroutineSpawnEnemy = SpawnEnemy(_spawnEnemyWaitTime);
        StartCoroutine(coroutineSpawnEnemy);
        // Create Asteroid Objects
        coroutineSpawnAsteroid = SpawnAsteroid(_spawnAsteroidWaitTime);
        StartCoroutine(coroutineSpawnAsteroid);
        // Create PowerUp Objects
        coroutineSpawnPowerUp = SpawnPowerUp(_gameContainers[2].container);
        StartCoroutine(coroutineSpawnPowerUp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator GamePlay()
    {
        //Debug.LogError("GamePlay L1: " + _uiManager.GameOverCheck + " -- " + GamePlayLoop);
        while (GamePlayLoop)
        {
           //Debug.LogError("GamePlay L2: " + _uiManager.GameOverCheck + " -- " + GamePlayLoop);
            if (_uiManager.GameOverCheck)
            {
                _uiManager.GameIsOver();
                while (Input.GetKeyDown(KeyCode.R))
                {
                    _uiManager.GameStart();
                    yield return null;
                }
                while (Input.GetKeyDown(KeyCode.M))
                {
                    SceneManager.LoadScene(0);
                    yield return null;
                }
                //Debug.LogError("GamePlay L3: " + _uiManager.GameOverCheck + " -- " + GamePlayLoop);s
            }
            yield return new WaitUntil(() => _uiManager.GameOverCheck);
        }
    }
    IEnumerator SpawnPlayer()
    {
        while (spawnPlayerLoop)
        {
            //Debug.LogError("SpawnPlayer L2: "+ _uiManager.GameOverCheck);
            if (GameObject.FindGameObjectWithTag("Player") == null && !_uiManager.GameOverCheck)
            {
                _uiManager.GameStart();
                GameObject newPlayer = Instantiate(_player);
             }
            yield return new WaitUntil(() => _uiManager.GameOverCheck);
        }
    }
    IEnumerator SpawnEnemy(float waitTime)
    {
        while (spawnEnemyLoop)
        {
            if (!_uiManager.GameOverCheck)
            {
                if (!_EnemysOff)
                {
                    while (Instantiate(_enemy)) yield return new WaitForSeconds(waitTime);
                }
            }
            yield return new WaitUntil(() => _uiManager.GameOverCheck);
        }
    }
    IEnumerator SpawnAsteroid(float waitTime)
    {
        while (spawnAsteroidLoop)
        {
            if (!_uiManager.GameOverCheck)
            {
                if (!_EnemysOff)
                {
                    while (Instantiate(_asteroid)) yield return new WaitForSeconds(waitTime);
                }
            }
            yield return new WaitUntil(() => _uiManager.GameOverCheck);
        }
    }
    IEnumerator SpawnPowerUp(GameObject powerupContainer)
    {
        while (spawnPowerUpLoop)
        {
            if (GameObject.FindGameObjectWithTag("PowerUp") != true && !_uiManager.GameOverCheck)
            {
                for (int i = 0; i < _powerup.Count; i++)
                {
                    if (GameObject.Find(_powerup[i].powerupName) != true)
                    {
                        Rigidbody2D newPowerUp = Instantiate(_powerup[i].PowerupPrefab);
                        newPowerUp.GetComponent<Powerup>().SetStartup(powerupContainer, _powerup[i].location, _powerup[i].powerupTimer);
                    }
                }
            }
            yield return new WaitUntil(() => _uiManager.GameOverCheck);
        }
    }

    public GameObject PlayerContainer
    {
        get
        {
            return _gameContainers[0].container;
        }
        set
        {
            _gameContainers[0].container = value;
        }
    }
    public GameObject LaserContainer
    {
        get
        {
            return _gameContainers[1].container;
        }
        set
        {
            _gameContainers[1].container = value;
        }
    }
    public GameObject EnemyContainer
    {
        get
        {
            return _gameContainers[3].container;
        }
        set
        {
            _gameContainers[3].container = value;
        }
    }
        public GameObject AsteroidContainer
    {
        get
        {
            return _gameContainers[4].container;
        }
        set
        {
            _gameContainers[4].container = value;
        }
    }
}
