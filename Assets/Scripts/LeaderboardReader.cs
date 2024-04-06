using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class LeaderboardReader : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI leaderboardNames = null;
    [SerializeField] private TextMeshProUGUI leaderboardScores = null;
    [SerializeField] private TextMeshProUGUI leaderboardTimes = null;
    
    private const string GET_URL = "https://rangwath.cz/eatgun/db-access-scripts/get_leaderboard.php";

    private void Start() 
    {
        GetResult();
    }

    public void GetResult()
    {
        StartCoroutine(ReadLeaderboardFromWeb());
    }

    private IEnumerator ReadLeaderboardFromWeb()
    {
        string url = URLAntiCacheRandomizer(GET_URL);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            Debug.Log("URL: " + webRequest.url);

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Web Request to Read Leaderboard Result: " + webRequest.result);

                if (webRequest.downloadHandler.text == "0")
                {
                    Debug.Log("WebResult text is 0, nothing to parse, displaying empty table.");
                    leaderboardNames.text = "";
                    leaderboardScores.text = "";
                    leaderboardTimes.text = "";
                }
                else
                {
                    List<HighScoreRecord> highScoreList = ParseWebResultIntoHighScoreList(webRequest.downloadHandler.text);

                    leaderboardNames.text = GetHighScoreNames(highScoreList);
                    leaderboardScores.text = GetHighScoreScores(highScoreList);
                    leaderboardTimes.text = GetHighScoreTimes(highScoreList);
                }               
            }
            else
            {
                Debug.LogWarning("Web Request to Read Leaderboard Result: " + webRequest.result);
            }
        }      
    }  

    private List<HighScoreRecord> ParseWebResultIntoHighScoreList(string leaderboardText)
    {      
        string[] records = leaderboardText.Split(";");

        List<HighScoreRecord> highScoreList = new List<HighScoreRecord>();

        foreach (string record in records)
        {
            if (!string.IsNullOrWhiteSpace(record))
            {
                string[] fields = record.Split(",");

                HighScoreRecord highScoreRecord = new HighScoreRecord(Int32.Parse(fields[0]), fields[1], Int32.Parse(fields[2]), Int32.Parse(fields[3]));
                highScoreList.Add(highScoreRecord);
            }
        }

        Debug.Log("Records in High Score List: " + highScoreList.Count);

        return highScoreList;
    }

    private string GetHighScoreNames(List<HighScoreRecord> highScoreList)
    {
        string parsedList = "";

        foreach (HighScoreRecord record in highScoreList)
        {
            parsedList += record.GetName();
            parsedList += Environment.NewLine;
        }

        return parsedList;
    }

    private string GetHighScoreScores(List<HighScoreRecord> highScoreList)
    {
        string parsedList = "";

        foreach (HighScoreRecord record in highScoreList)
        {
            parsedList += record.GetScore();
            parsedList += Environment.NewLine;
        }

        return parsedList;
    }

    private string GetHighScoreTimes(List<HighScoreRecord> highScoreList)
    {
        string parsedList = "";

        foreach (HighScoreRecord record in highScoreList)
        {
            int time = record.GetTimeInSeconds();
            float minutes = Mathf.Floor(time / 60);
            float seconds = Mathf.RoundToInt(time % 60);
            string timerText = null;
            if (seconds < 10)
            {
                timerText = minutes.ToString() + ":0" + seconds.ToString();
            }
            else
            {
                timerText = minutes.ToString() + ":" + seconds.ToString();
            }
            parsedList += timerText;
            parsedList += Environment.NewLine;
        }

        return parsedList;
    }

    public static string URLAntiCacheRandomizer(string url)
    {
        string temp = "";
        temp += UnityEngine.Random.Range(1000000, 8000000).ToString();
        temp += UnityEngine.Random.Range(1000000, 8000000).ToString();
        string result = url + "?temp=" + temp;
        return result;
    }
}
