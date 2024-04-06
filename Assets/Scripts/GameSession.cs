using System;
using UnityEngine;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText = null;
    [SerializeField] private TextMeshProUGUI timerText = null;
    [SerializeField] private TextMeshProUGUI endScoreText = null;
    [SerializeField] private TextMeshProUGUI endTimerText = null;
    [SerializeField] private TextMeshProUGUI endMessageText = null;
    //[SerializeField] private TMP_InputField nameInput = null;

    private const string WIN_MESSAGE = "YOU WON!";
    private const string LOSE_MESSAGE = "YOU DIED!";

    private void Awake()
    {
        // Singleton pattern
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numberGameSessions > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() 
    {
        scoreText.text = score.ToString();
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }

    public void Win()
    {
        endMessageText.text = WIN_MESSAGE;
        PreventPlayerMovement();
        DisplayEndMenu();
        ResetGameSession();
    }

    public void ProcessPlayerDeath()
    {
        endMessageText.text = LOSE_MESSAGE;
        DisplayEndMenu();
        PlayDeathSound();     
        ResetGameSession();
    }

    public void ResetGameSession()
    {
        // GameSession cleanup
        Destroy(gameObject);
    }

    private void PlayDeathSound()
    {
        SoundPlayer soundPlayer = FindObjectOfType<SoundPlayer>();
        if (soundPlayer == null)
        {
            Debug.LogWarning("SoundPlayer is null, sound will not work, add SoundPlayer to the scene.");
            return;
        }
        else
        {
            soundPlayer.PlayDeathSound();
        }
    }

    private void DisplayEndMenu()
    {        
        MenuManager menuManager = FindObjectOfType<MenuManager>();
        if (menuManager == null)
        {
            Debug.LogError("MenuManager is null, add MenuManager to the scene.");
            return;
        }
        ScoreSaver scoreSaver = FindObjectOfType<ScoreSaver>();
        if (scoreSaver == null)
        {
            Debug.LogWarning("ScoreSaver is null, add ScoreSaver to the scene or the score won't be saved.");
            return;
        }
        LevelTimer levelTimer = FindObjectOfType<LevelTimer>();
        if (levelTimer == null)
        {
            Debug.LogWarning("LevelTimer is null, add LevelTimer to the scene or the score won't be saved since timer cannot be retrieved.");
            return;
        }

        // Commented out because controller cannot get out of it
        //nameInput.Select();
        //nameInput.ActivateInputField();

        endScoreText.text = scoreText.text;
        endTimerText.text = timerText.text;

        scoreSaver.SetScore(Int32.Parse(endScoreText.text));
        scoreSaver.SetTimeInSeconds(levelTimer.GetTimeInSeconds());

        menuManager.DisplayEndMenu();
    }

    private void PreventPlayerMovement()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.Log("PlayerMovement is null, is player already dead?");
            return;
        }

        playerMovement.PreventPlayerMovement();
    }
}
