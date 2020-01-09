using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [System.Serializable]
    public class HDisplaySystem
    {
        [HideInInspector]
        public string name;
        [SerializeField]
        public TextMeshProUGUI displayTextCard;
        [SerializeField]
        public bool displayActive;
        [SerializeField]
        public string displayName;
        [SerializeField]
        public bool scoreOn;
        [SerializeField]
        public string effect;
    }
    [SerializeField]
    private List<HDisplaySystem> _hDDisplaySystem = new List<HDisplaySystem>()
    {
        new HDisplaySystem {name = "Score", displayName = "Score:", displayActive = true , scoreOn = true, effect = "PlainText"},
        new HDisplaySystem {name = "GameOver", displayName = "Game Over", displayActive = false, scoreOn = false, effect = "Blink"},
        new HDisplaySystem {name = "HighScore", displayName = "High Score:", displayActive = true , scoreOn = true, effect = "PlainText"},
        new HDisplaySystem {name = "NewGame", displayName = "Game Coded by Sean Li hellohal2064@gmail.com", displayActive = false, scoreOn = false, effect = "PlainText"}
    };
    [SerializeField]
    private GameObject _GamePanel = null;
    [SerializeField]
    private GameObject _continueButton = null;

    //Not in Unity
    private SystemManager _systemManager;
    private SystemManager hsControl;
    private bool _gameover;
    private float _scoreLive;
    private IEnumerator coroutineBlinkDisplay;
    private TextMeshProUGUI textMeshProUGUI;

    //Default Colors
    Color startColor = new Color32(188, 0, 0, 0);
    Color endColor = new Color32(188, 0, 0, 255);
    Color baseColor = new Color32(255,255,255,255);

    // Start is called before the first frame update
    void Start()
    {
        _systemManager = GameObject.FindGameObjectWithTag("SystemManager").GetComponent<SystemManager>();
        GameStart();
    }
    // Update is called once per frame
    void Update()
    {
 
    }
    void EffectPlainText(string name, bool scoreon, TextMeshProUGUI displaytextcard, string displayname, float scoreLive)
    {
        if (scoreon)
        {
            displaytextcard.text = displayname + " " + scoreLive.ToString();
        }
        else if (!scoreon)
        {
            displaytextcard.text = displayname;
        }
    }
    void EffectBlink(string name, bool scoreon, TextMeshProUGUI displaytextcard, string displayname)
    {
        displaytextcard.text = displayname;
        coroutineBlinkDisplay = BlinkEffect(displaytextcard);
        StartCoroutine(coroutineBlinkDisplay);
    }
    void ClearText(TextMeshProUGUI displaytextcard)
    {
        displaytextcard.text = "";
    }
    void UpdateHDSystem(List<HDisplaySystem> hddisplaySystem, string hdSystemName, bool scoreOn, bool displayActive, string displayEffect, [Optional] float liveScore)
    {
        for (int i = 0; i < hddisplaySystem.Count; i++)
        {
            if (hddisplaySystem[i].name == hdSystemName)
            {
                hddisplaySystem[i].effect = displayEffect;
                hddisplaySystem[i].scoreOn = scoreOn;
                hddisplaySystem[i].displayActive = displayActive;
                if (displayActive)
                {
                    switch (hddisplaySystem[i].effect)
                    {
                        case "PlainText":
                            EffectPlainText(name: hddisplaySystem[i].name, scoreon: hddisplaySystem[i].scoreOn, displaytextcard: hddisplaySystem[i].displayTextCard, displayname: hddisplaySystem[i].displayName, scoreLive: liveScore);
                            break;
                        case "Blink":
                            EffectBlink(name: hddisplaySystem[i].name, scoreon: hddisplaySystem[i].scoreOn, displaytextcard: hddisplaySystem[i].displayTextCard, displayname: hddisplaySystem[i].displayName);
                            break;
                        default:
                            break;
                    }
                }
                else if (!displayActive)
                {
                    ClearText(hddisplaySystem[i].displayTextCard);
                }
            }
        }
    }
    void ChangeTextHDSystem(List<HDisplaySystem> hddisplaySystem, string hdSystemName, string newText)
    {
        for (int i = 0; i < hddisplaySystem.Count; i++)
        {
            if (hddisplaySystem[i].name == hdSystemName)
            {
                hddisplaySystem[i].displayName = newText;
            }
        }
    }
    float ReadHighScore()
    {
        hsControl = GameObject.Find("SystemManager").GetComponent<SystemManager>();
        return hsControl.HighScoreIs;
    }
    void WriteHighScore(float hsScore) 
    {
        hsControl = GameObject.Find("SystemManager").GetComponent<SystemManager>();
        hsControl.HighScoreIs = hsScore;
    }

    IEnumerator BlinkEffect(TextMeshProUGUI displaytextcard)
    {
        yield return new WaitForSeconds(0.2f);
        float t = 0;

        while (_gameover)
        {
            yield return new WaitForSeconds(0.2f);
            while (t < 1)
            {
                displaytextcard.color = Color.Lerp(startColor, endColor, t);
                t += Time.deltaTime / 15f;
            }
            yield return new WaitForSeconds(0.2f);
            t = 0;
            yield return new WaitForSeconds(0.2f);
            displaytextcard.color = startColor;
        }
    }
    public void GameIsOver()
    {
        Button gpButton = _continueButton.GetComponent<Button>();
        _systemManager.WritePrefs();
        WriteHighScore(_scoreLive);
        _GamePanel.SetActive(true);
        gpButton.interactable = false;
        gpButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        ChangeTextHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "GameOver", newText: "Game Over");
        StopCoroutine("BlinkEffect");
        UpdateHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "HighScore", scoreOn: true, displayActive: true, displayEffect: "PlainText", liveScore: ReadHighScore());
        UpdateHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "GameOver", scoreOn: false, displayActive: true, displayEffect: "Blink");
        UpdateHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "NewGame", scoreOn: false, displayActive: true, displayEffect: "PlainText");
    }
    public void GameStart()
    {
        _scoreLive = 0;
        GameOverCheck = false;
        _GamePanel.SetActive(false);
        UpdateHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "HighScore", scoreOn: true, displayActive: true, displayEffect: "PlainText", liveScore: ReadHighScore());
        UpdateHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "Score", scoreOn: true, displayActive: true, displayEffect: "PlainText", liveScore: _scoreLive);
        UpdateHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "NewGame", scoreOn: false, displayActive: false, displayEffect: "PlainText");
    }
    public void Score(string enemyType)
    {
        string[] MyEnemy = new string[] { "SpaceShip", "Asteroid" };

        if (MyEnemy[0] == enemyType)
        {
            _scoreLive += 5f;
        }
        else if (MyEnemy[1] == enemyType)
        {
            _scoreLive += 10f;
        }
        UpdateHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "Score", scoreOn: true, displayActive: true, displayEffect: "PlainText", liveScore: _scoreLive);
    }
    public void GameRestart()
    {
        Time.timeScale = 1;
        _systemManager.WritePrefs();
        SceneManager.LoadScene(1);
    }
    public void GamePause()
    {
        _systemManager.WritePrefs();
        WriteHighScore(_scoreLive);
        _continueButton.SetActive(true);
        ChangeTextHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "GameOver", newText: "Game Paused");
        StopCoroutine("BlinkEffect");
        UpdateHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "GameOver", scoreOn: false, displayActive: true, displayEffect: "Blink");
        ChangeTextHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "NewGame", newText: "Game Coded by Sean Li hellohal2064@gmail.com");
        UpdateHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "NewGame", scoreOn: false, displayActive: true, displayEffect: "PlainText");
        _GamePanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void GameContinue()
    {
        Time.timeScale = 1;
        _systemManager.WritePrefs();
        _GamePanel.SetActive(false);
    }
    public void GameMainMenu()
    {
        Time.timeScale = 1;
        _systemManager.WritePrefs();
        WriteHighScore(_scoreLive);
        SceneManager.LoadScene(0);
    }
    public bool GameOverCheck
    {
        get
        {
            return _gameover;
        }
        set
        {
            _gameover = value;
        }
    }
}
