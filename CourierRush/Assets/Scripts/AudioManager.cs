using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }


    public AudioSource musicSource;

    public AudioSource sfxSource;
    public AudioClip pickupClip;
    public AudioClip deliveryClip;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (musicSource != null && !musicSource.isPlaying)
            musicSource.Play();
    }

    // Called by GameManager.OnPackagePickedUp event
    public void PlayPickupSFX()
    {
        PlayClip(pickupClip);
    }

    // Called by GameManager.OnDeliveryCompleted event
    public void PlayDeliverySFX()
    {
        PlayClip(deliveryClip);
    }

    void PlayClip(AudioClip clip)
    {
        if (sfxSource == null || clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

}
