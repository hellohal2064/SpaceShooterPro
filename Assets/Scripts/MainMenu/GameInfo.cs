using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInfo : MonoBehaviour
{
    public void MainMenu()
    {
        //GameObject.FindGameObjectWithTag("SystemManager").GetComponent<SystemManager>().WritePrefs();
        //GameObject.FindGameObjectWithTag("ResetHS").GetComponent<ToggleHS>().ClearToggle();
        SceneManager.LoadScene(0);
    }
}
