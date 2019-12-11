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
        new HDisplaySystem {name = "NewGame", displayName = "Game Coded by Sean Li hellohal2064@gmail.com", displayActive = false, scoreOn = false, effect = "PlainText"}
    };
    [SerializeField]
    private GameObject GamePanel = null;
    [SerializeField]
    private GameObject _continueButton = null;

    //Not in Unity
    private bool _gameover;
    private float _scoreLive;
    private IEnumerator coroutineHDDisplay;
    private TextMeshProUGUI textMeshProUGUI;

    //Default Colors
    Color startColor = new Color32(188, 0, 0, 0);
    Color endColor = new Color32(188, 0, 0, 255);
    Color baseColor = new Color32(255,255,255,255);


    // Start is called before the first frame update
    void Start()
    {
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
    void EffectBlink(string name, bool scoreon, TextMeshProUGUI displaytextcard, string displayname, float scoreLive)
    {
        displaytextcard.text = displayname;
        StartCoroutine(coroutineHDDisplay);
    }
    void ClearText(TextMeshProUGUI displaytextcard)
    {
        displaytextcard.text = "";
    }
    void UpdateHDSystem(List<HDisplaySystem> hddisplaySystem, string hdSystemName, bool scoreOn, bool displayActive, string displayEffect)
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
                            EffectPlainText(name: hddisplaySystem[i].name, scoreon: hddisplaySystem[i].scoreOn, displaytextcard: hddisplaySystem[i].displayTextCard, displayname: hddisplaySystem[i].displayName, scoreLive: _scoreLive);
                            break;
                        case "Blink":
                            EffectBlink(name: hddisplaySystem[i].name, scoreon: hddisplaySystem[i].scoreOn, displaytextcard: hddisplaySystem[i].displayTextCard, displayname: hddisplaySystem[i].displayName, scoreLive: _scoreLive);
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
   IEnumerator BlinkEffect()
    {
        yield return new WaitForSeconds(0.2f);
        float t = 0;

        while (_gameover)
        {
            yield return new WaitForSeconds(0.9f);
            while (t < 1)
            {
                _hDDisplaySystem[1].displayTextCard.color = Color.Lerp(startColor, endColor, t);
                t += Time.deltaTime / 15f;
            }
            yield return new WaitForSeconds(0.9f);
            t = 0;
            yield return new WaitForSeconds(0.9f);
            _hDDisplaySystem[1].displayTextCard.color = startColor;
        }
    }
    public void GameIsOver()
    {
        GamePanel.SetActive(true);
        _continueButton.SetActive(false);
        ChangeTextHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "GameOver", newText: "Game Over");
        UpdateHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "GameOver", scoreOn: false, displayActive: true, displayEffect: "Blink");
        UpdateHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "NewGame", scoreOn: false, displayActive: true, displayEffect: "PlainText");
    }
    public void GameStart()
    {
        _scoreLive = 0;
        GameOverCheck = false;
        GamePanel.SetActive(false);
        coroutineHDDisplay = BlinkEffect();
        UpdateHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "Score", scoreOn: true, displayActive: true, displayEffect: "PlainText");
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
        UpdateHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "Score", scoreOn: true, displayActive: true, displayEffect: "PlainText");
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
    public void GameRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void GamePause()
    {
        _continueButton.SetActive(true);
        ChangeTextHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "GameOver", newText: "Game Paused");
        UpdateHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "GameOver", scoreOn: false, displayActive: true, displayEffect: "Blink");
        ChangeTextHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "NewGame", newText: "Game Coded by Sean Li hellohal2064@gmail.com");
        UpdateHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "NewGame", scoreOn: false, displayActive: true, displayEffect: "PlainText");
        GamePanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void GameContinue()
    {
        Time.timeScale = 1;
        GamePanel.SetActive(false);
    }
    public void GameMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
