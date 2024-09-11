using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("DevEnemy"); // PLACEHOLDER SCENE NAME!!
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
