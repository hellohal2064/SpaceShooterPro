using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Coroutines
    private bool GamePlayLoop = true;
    private IEnumerator coroutineGamePlay;
    //Not In Unity
    private UIManager _uiManager;
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        // Create Game Play Loop
        coroutineGamePlay = GamePlay();
        StartCoroutine(coroutineGamePlay);
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
