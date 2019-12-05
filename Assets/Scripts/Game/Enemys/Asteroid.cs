using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Asteroid : MonoBehaviour
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
    private float _xLeftBound = -9.0f;
    // Screen player right .y boundry
    [SerializeField]
    private float _xRightBound = 9.0f;
    [SerializeField]
    private AudioClip _explosionSound = null; 
    //Not in Unity
    private bool _firstPass = true;
    private bool _firstScore = false;
    private GameObject _player;
    private UIManager _uiManager;
    private Animator _mainAnimator;
    private float _clipLenght;
    RuntimeAnimatorController _ac;
    //Audio
    private AudioSource _playerAudio;
    private bool _playedAudio = false;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        //Unity Object Container
        SetContainer(GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>().AsteroidContainer);
        //Animation Control
        _mainAnimator = GetComponentInChildren<Animator>(false).GetComponent<Animator>();
        _ac = _mainAnimator.runtimeAnimatorController;
        _mainAnimator.ResetTrigger("OnAsteroidDeath");
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
        float t = 0;

        _ySpeed = 0.8f;
        AnimationClip[] clips = _ac.animationClips;
        _clipLenght = clips[0].length;
        while (t < 1)
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(new Color32(255, 255, 255, 255), new Color32(255, 255, 255, 0), t);
            t += Time.deltaTime / 10f;
        }
        _mainAnimator.SetTrigger("OnAsteroidDeath");
        if (!isPlayer && !_firstScore)
        {
            _uiManager.Score("Asteroid");
            _firstScore = true;
        }
        if (!_playedAudio)
        {
            _playerAudio.Play();
            _playedAudio = true;
        }
        Destroy(this.gameObject, (_clipLenght));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();
        if (collision.CompareTag("Laser") || collision.CompareTag("TripleShot"))
        {
            Destroy(collision.gameObject);
            DestroyAnimator();
        }
        else if (collision.CompareTag("Shields"))
        {
            if (player != null)
            {
                player.Damage("Shields");
                DestroyAnimator();
            }
            else
            {
                Debug.Log("Collider Shields Bug");
            }
        }
        else if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.Damage("Player");
                DestroyAnimator(isPlayer: true);
            }
            else
            {
                Debug.Log("Collider Player Bug");
            }
        }
    }
    public void SetContainer(GameObject asteroidContainer)
    {
        if (asteroidContainer != null)
        {
            this.gameObject.transform.parent = asteroidContainer.transform;
        }
        else
        {
            Debug.Log("Startup Bug PowerContainer");
        }
    }
}
