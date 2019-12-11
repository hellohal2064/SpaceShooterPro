using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    [System.Serializable]
    public class GameShare
    {
        [HideInInspector]
        public string name;
        [SerializeField]
        public float gameVolume;
        [SerializeField]
        public float highScore;
        [SerializeField]
        public int highLevel;
    }
    [System.Serializable]
    public class LevelRequirments
    {
        [HideInInspector]
        public string name;
        [SerializeField]
        public int levelId;
        [SerializeField]
        public int scoreReguired;
        [SerializeField]
        public float spawnEnemyWaitTime;
        [SerializeField]
        public bool spawnAsteroid;
        [SerializeField]
        public float spawnAsteroidWaitTime;
    }

    static SystemManager instance;

    [SerializeField]
    public int _maxLevel = 6;
    [SerializeField]
    private List<GameShare> _gameShare = new List<GameShare>
    {
        new GameShare {name = "SharedInfo", gameVolume =1}
    };
    [SerializeField]
    private List<LevelRequirments> _levelRequirments = new List<LevelRequirments>
    {
        new LevelRequirments {name = "Level1", levelId = 1, scoreReguired = 0, spawnEnemyWaitTime = 10f,  spawnAsteroid = false, spawnAsteroidWaitTime =25f},
        new LevelRequirments {name = "Level2", levelId = 2, scoreReguired = 100, spawnEnemyWaitTime = 7f,  spawnAsteroid = false, spawnAsteroidWaitTime =25f},
        new LevelRequirments {name = "Level3", levelId = 3, scoreReguired = 200, spawnEnemyWaitTime = 5f,  spawnAsteroid = true, spawnAsteroidWaitTime =25f}
    };

    //Don't Destroy in Unity
    void Awake()
    {
        if (instance == null)
        {
            instance = this; // In first scene, make us the singleton.
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject); // On reload, singleton already set, so destroy duplicate.
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.LogError("High Score Get: " + _gameShare[0].highScore);
    }
    public float HighScoreIs
    {
        get
        {
            return _gameShare[0].highScore;
            //Debug.LogError("High Score Get: " + _gameShare[0].highScore);
        }
        set
        {
            if (value > _gameShare[0].highScore)
            {
                _gameShare[0].highScore = value;
                //Debug.LogError("High Score Set: " + _gameShare[0].highScore);
            }
        }
    }
}
