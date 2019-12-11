using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainManager : MonoBehaviour
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
        public float scoreIncrement;
        [SerializeField]
        public bool scoreOn;
        [SerializeField]
        public string effect;
    }
    [SerializeField]
    private List<HDisplaySystem> _hDDisplaySystem = new List<HDisplaySystem>()
    {
        new HDisplaySystem {name = "Credits", displayName = "Game Coded by Sean Li hellohal2064@gmail.com", displayActive = false, scoreOn = false, effect = "Blink"},
    };
    //Not in Unity
    private bool _gameover;
    private float _scoreLive;
    private IEnumerator coroutineHDDisplay;
    private TextMeshProUGUI textMeshProUGUI;
    private TextMeshProUGUI blinkEffect;
    Color startColor = new Color32(255, 255, 255, 255);
    Color endColor = new Color32(255, 255, 255, 0);

    // Start is called before the first frame update
    void Start()
    {
        coroutineHDDisplay = BlinkEffect();
        UpdateHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "Credits", scoreOn: false, displayActive: true, displayEffect: "Blink");
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
    IEnumerator BlinkEffect()
    {
        yield return new WaitForSeconds(0.2f);
        float t = 0;

        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            while (t < 1)
            {
                _hDDisplaySystem[0].displayTextCard.color = Color.Lerp(startColor, endColor, t);
                t += Time.deltaTime / 10f;
            }
            yield return new WaitForSeconds(0.5f);
            _hDDisplaySystem[0].displayTextCard.color = startColor;
            t = 0;
            yield return new WaitForSeconds(0.2f);
        }
    }
    public void Score()
    {

        for (int i = 0; i < _hDDisplaySystem.Count; i++)
        {
            if (_hDDisplaySystem[i].scoreIncrement > 0)
            {
                _scoreLive += _hDDisplaySystem[i].scoreIncrement;
            }
        }
        UpdateHDSystem(hddisplaySystem: _hDDisplaySystem, hdSystemName: "Score", scoreOn: true, displayActive: true, displayEffect: "PlainText");
    }
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }
    public void GameInfo()
    {
        SceneManager.LoadScene(2);
    }
    public void Settings()
    {

    }
    public void Quit()
    {
        Application.Quit();
    }
}
