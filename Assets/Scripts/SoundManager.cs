using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager Instance {get; set;}

    public AudioClip attackSound, encounterSound, enemyDeathSound, missSound, abilitySound, healSound;
    private AudioSource audioSource;

    void Awake() {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
        } 
        else 
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}