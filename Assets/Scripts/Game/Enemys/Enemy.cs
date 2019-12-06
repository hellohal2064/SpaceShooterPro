using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _ySpeed = 3.5f;
    [SerializeField]
    private float _yTopBound = 7.5f;
    // Sreen player bottom .x boundry
    [SerializeField]
    private float _yBottomBound = -6f;
    // Screen player left .y boundry
    [SerializeField]
    private float _xLeftBound = -10f;
    // Screen player right .y boundry
    [SerializeField]
    private float _xRightBound = 10f;
    //Audio Manager
    [SerializeField]
    private AudioClip _explosionSound = null;
    //Not in Unity
    private bool _firstPass = true;
    private bool _firstScore = false;
    private GameObject _player;
    private UIManager _uiManager;
    private Animator _mainAnimator;
    //private Animation _currentClipInfo;
    private float _clipLenght;
    RuntimeAnimatorController _ac;
    //Audio Manger
    private AudioSource _playerAudio;
    private bool _playedAudio = false;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        //Unity Object Container
        SetContainer(GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>().EnemyContainer);
        //Animation Control
        _mainAnimator = GetComponent<Animator>();
        _ac = _mainAnimator.runtimeAnimatorController;
        _mainAnimator.ResetTrigger("OnEnemyDeath");
         //Audio Manager
        _playerAudio = GetComponent<AudioSource>();
        _playerAudio.clip = _explosionSound;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            if (transform.position.y < _yBottomBound && _firstPass == false)
            {
                transform.position = new Vector3(Random.Range(_xLeftBound, _xRightBound), _yTopBound + 1, 0);
            }
            else if (_firstPass == true)
            {
                transform.position = new Vector3(Random.Range(_xLeftBound, _xRightBound), _yTopBound + 1, 0);
                _firstPass = false;
            }
            else
            {
                transform.Translate(Vector3.down * _ySpeed * Time.deltaTime);
            }
        } 
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void DestroyAnimator([Optional] bool isPlayer)
    {
        _ySpeed = 0.8f;
        AnimationClip[] clips = _ac.animationClips;
        _clipLenght = clips[0].length;
        _mainAnimator.SetTrigger("OnEnemyDeath");
        if (!isPlayer && !_firstScore)
        {
            _uiManager.Score("SpaceShip");
            _firstScore = true;
        }
        if (!_playedAudio)
        {
            _playerAudio.Play();
            _playedAudio = true;
        }
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        Destroy(this.gameObject, _clipLenght);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser") || collision.CompareTag("TripleShot"))
        {
            Destroy(collision.gameObject);
            DestroyAnimator();
        }
        else if (collision.CompareTag("Shields"))
        {
            Player player = collision.GetComponentInParent <Player>();
            if (player != null)
            {
                player.Damage("Shields");
            }
            else
            {
                Debug.Log("Collider Shields Bug");
            }
            DestroyAnimator();
        }
        else if (collision.CompareTag("Player"))
        {
            Player player = collision.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage("Player");
            }
            else
            {
                Debug.Log("Collider Player Bug");
            }
            DestroyAnimator(isPlayer: true);
        }
    }
    public void SetContainer(GameObject enemyContainer)
    {
        if (enemyContainer != null)
        {
            this.gameObject.transform.parent = enemyContainer.transform;
        }
        else
        {
            Debug.Log("Startup Bug PowerContainer");
        }
    }
}