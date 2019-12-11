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
        //Debug.LogError("GamePlay L1: " + _uiManager.GameOverCheck + " -- " + GamePlayLoop);
        while (GamePlayLoop)
        {
            //Debug.LogError("GamePlay L2: " + _uiManager.GameOverCheck + " -- " + GamePlayLoop);
            if (_uiManager.GameOverCheck)
            {
                _uiManager.GameIsOver();
                //Debug.LogError("GamePlay L3: " + _uiManager.GameOverCheck + " -- " + GamePlayLoop);s
            }
            yield return new WaitUntil(() => _uiManager.GameOverCheck);
        }
    }
}
