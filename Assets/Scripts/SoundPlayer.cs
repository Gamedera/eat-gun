using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private float initialVolume = 1f;
    [SerializeField] private AudioClip eatSound = null;
    [SerializeField] private float eatSoundVolume = 1f;
    [SerializeField] private AudioClip evolveSound = null;
    [SerializeField] private float evolveSoundVolume = 1f;
    [SerializeField] private AudioClip laserSound = null;
    [SerializeField] private float laserSoundVolume = 1f;
    [SerializeField] private AudioClip deathSound = null;
    [SerializeField] private float deathSoundVolume = 1f;

    private AudioSource audioSource = null;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = initialVolume;
    }

    public void PlayEatSound()
    {
        audioSource.PlayOneShot(eatSound, eatSoundVolume);
    }

    public void PlayEvolveSound()
    {
        audioSource.PlayOneShot(evolveSound, evolveSoundVolume);
    }

    public void PlayLaserSound()
    {
        audioSource.PlayOneShot(laserSound, laserSoundVolume);
    }

    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathSound, deathSoundVolume);
    }
}
