using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInfo : MonoBehaviour
{
    public void MainMenu()
    {
        GameObject.FindGameObjectWithTag("SystemManager").GetComponent<SystemManager>().WritePrefs();
        SceneManager.LoadScene(0);
    }
}
