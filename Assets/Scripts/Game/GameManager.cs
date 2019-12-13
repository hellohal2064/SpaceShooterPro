using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mainMixer = null;
    //Coroutines
    private bool GamePlayLoop = true;
    private IEnumerator coroutineGamePlay;
    //Not In Unity
    private UIManager _uiManager;
    private SystemManager _systemManager;
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        _systemManager = GameObject.FindGameObjectWithTag("SystemManager").GetComponent<SystemManager>();
        _systemManager.ReadPrefs();
        SetVolume(_systemManager.GameVolume);
        // Create Game Play Loop
        coroutineGamePlay = GamePlay();
        StartCoroutine(coroutineGamePlay);
    }
    void Update()
    {
        SetVolume(_systemManager.GameVolume);
    }
    void SetVolume(float volume)
    {
        float test = Mathf.Log10(volume) * 20;
        mainMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
    }
    IEnumerator GamePlay()
    {
        while (GamePlayLoop)
        {
            if (_uiManager.GameOverCheck)
            {
                _uiManager.GameIsOver();
            }
            yield return new WaitUntil(() => _uiManager.GameOverCheck);
        }
    }
}
