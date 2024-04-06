using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class ScoreSaver : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput = null;
    [SerializeField] private float delayWhenSavingScore = 1.5f;

    private int score = 0;
    private int timeInSeconds = 0;

    private const string POST_URL = "https://rangwath.cz/eatgun/db-access-scripts/insert_score_v1.php";
    private const long PIN = 0; // Insert correct PIN, removed because of public repository

    public void SaveScore()
    {
        if (string.IsNullOrWhiteSpace(nameInput.text))
        {
            Debug.Log("Name empty, returning");
            return;
        }

        MenuManager menuManager = FindObjectOfType<MenuManager>();
        if (menuManager == null)
        {
            Debug.LogError("MenuManager is null, add MenuManager to the scene.");
            return;
        }

        menuManager.HideEndMenu();
        menuManager.DisplaySavingMenu();

        InsertScore();

        StartCoroutine(LoadLeaderboardMenuAfterDelay(menuManager));
    }

    private void InsertScore()
    {
        if (string.IsNullOrWhiteSpace(nameInput.text))
        {
            Debug.LogWarning("Empty name cannot be inserted into DB");
            return;
        }

        StartCoroutine(InsertScoreToWeb(nameInput.text.ToUpper(), score, timeInSeconds));
    }

    private IEnumerator InsertScoreToWeb(string name, int score, int seconds)
    {
        long checksum = CalculateCheckSum(score, seconds);
        Debug.Log("Request Checksum: " + checksum);

        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("score", score);
        form.AddField("seconds", seconds);
        form.AddField("checksum", checksum.ToString());

        using (UnityWebRequest webRequest = UnityWebRequest.Post(POST_URL, form))
        {
            yield return webRequest.SendWebRequest();
            Debug.Log("URL: " + webRequest.url);

            Debug.Log("Web Request Result to Insert Score Result: " + webRequest.result);

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Insert Score Script returned: " + webRequest.downloadHandler.text);
            }
            else
            {
                Debug.LogWarning("Insert Score Script returned: " + webRequest.downloadHandler.text);
            }
        }
    }

    public void SetScore(int scoreValue)
    {
        score = scoreValue;
    }

    public void SetTimeInSeconds(int secondsValue)
    {
        timeInSeconds = secondsValue;
    }

    private long CalculateCheckSum(int score, int seconds)
    {
        long firstPart = score * seconds * PIN;
        long secondPart = score + seconds + PIN;
        long checksum = (firstPart + secondPart) * 3;

        return checksum;
    }

    private IEnumerator LoadLeaderboardMenuAfterDelay(MenuManager menuManager)
    {
        yield return new WaitForSeconds(delayWhenSavingScore);

        menuManager.LoadLeaderboardMenu();
    }
}
