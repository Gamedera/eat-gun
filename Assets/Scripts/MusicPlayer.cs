using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private float initialVolume = 1f;

    private AudioSource audioSource = null;

    private void Awake()
    {
        // Singleton pattern
        int numberMusicPlayers = FindObjectsOfType<MusicPlayer>().Length;
        if (numberMusicPlayers > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = initialVolume;
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}