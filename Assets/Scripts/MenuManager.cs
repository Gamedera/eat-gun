using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject endMenu = null;
    [SerializeField] private GameObject savingMenu = null;

    public void StartFirstLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("FinalLevel");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLeaderboardMenu()
    {
        SceneManager.LoadScene("LeaderboardMenu");
    }

    public void ReloadCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void DisplayEndMenu()
    {
        if (endMenu == null)
        {
            Debug.LogWarning("EndMenu not assigned!");
        }
        endMenu.SetActive(true);
    }

    public void HideEndMenu()
    {
        if (endMenu == null)
        {
            Debug.LogWarning("EndMenu not assigned!");
        }
        endMenu.SetActive(false);
    }

    public void DisplaySavingMenu()
    {
        if (savingMenu == null)
        {
            Debug.LogWarning("SavingMenu not assigned!");
        }
        savingMenu.SetActive(true);
    }
}
