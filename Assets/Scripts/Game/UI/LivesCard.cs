using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesCard : MonoBehaviour
{
    [System.Serializable]
    public class HDLivesSystem
    {
        [HideInInspector]
        public string name;
        [SerializeField]
        public Sprite spriteContainer;
    }
    [SerializeField]
    private List<HDLivesSystem> _hDSpriteSystem = new List<HDLivesSystem>()
    {
        new HDLivesSystem {name = "No Lives - Default"},
        new HDLivesSystem {name = "One Lives"},
        new HDLivesSystem {name = "Two Lives"},
        new HDLivesSystem {name = "Three Lives"}
    };
    // Not in Unity
    private int _defaultSprite = 0;
    private int _startSprite = 3;
    private Color _defaultColor = new Color (255f, 255f, 255f, 255f );
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Image>().sprite = _hDSpriteSystem[_defaultSprite].spriteContainer;
        this.gameObject.GetComponent<Image>().color = _defaultColor;
        this.gameObject.GetComponent<Image>().preserveAspect = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // is Sprite Activation
    void SpriteActivate(int spritenumber)
    {
        switch (spritenumber)
        {
            case 0:
                this.gameObject.GetComponent<Image>().sprite = _hDSpriteSystem[_defaultSprite].spriteContainer;
                break;
            case 1:
                this.gameObject.GetComponent<Image>().sprite = _hDSpriteSystem[1].spriteContainer;
                break;
            case 2:
                this.gameObject.GetComponent<Image>().sprite = _hDSpriteSystem[2].spriteContainer;
                break;
            case 3:
                this.gameObject.GetComponent<Image>().sprite = _hDSpriteSystem[3].spriteContainer;
                break;
            default:
                this.gameObject.GetComponent<Image>().sprite = _hDSpriteSystem[_startSprite].spriteContainer;
                break;
        }
        return;
    }
    public void RestSprite(bool defaultsprite)
    {
        if (defaultsprite)
        {
            SpriteActivate(_startSprite);
        }
    }

    public void ActivateSprite(int activesprite)
    {
        SpriteActivate(activesprite);
    }
}
