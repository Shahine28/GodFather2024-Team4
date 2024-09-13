
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    public void StartGame()
    {
        SceneManager.LoadScene(_sceneName); // PLACEHOLDER SCENE NAME!!
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
