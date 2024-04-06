using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText = null;

    private float startTime = 0;
    private int timer = 0;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        float time = Time.time - startTime;
        timer = Mathf.RoundToInt(time);
        float minutes = Mathf.Floor(time / 60);
        float seconds = Mathf.RoundToInt(time % 60);
        if (seconds < 10)
        {
            timerText.text = minutes.ToString() + ":0" + seconds.ToString();
        }
        else
        {
            timerText.text = minutes.ToString() + ":" + seconds.ToString();
        }
    }

    public int GetTimeInSeconds()
    {
        return timer;
    }
}
