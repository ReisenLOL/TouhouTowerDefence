using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public GameObject mainMenuUI;
    public void ExitMenu(GameObject menu)
    {
        menu.SetActive(false);
        mainMenuUI.SetActive(true);
    }
    public void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);
        mainMenuUI.SetActive(false);
    }   
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        
    }
}