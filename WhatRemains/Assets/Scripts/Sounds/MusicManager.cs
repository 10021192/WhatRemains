using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    // Three AudioClips assigned in the Inspector
    public AudioClip track1;
    public AudioClip track2;
    public AudioClip track3;
    public AudioClip track4;
    public AudioClip track5;

    private AudioSource audioSource;
    private AudioClip[] playlist;
    private int currentTrackIndex = 0;

    void Awake()
    {
        // Singleton pattern (optional, if you want a single manager across scenes)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persists between scene loads
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Set up the AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;   // We'll manually detect when a song finishes
        audioSource.playOnAwake = false;
        audioSource.volume = 0.65f;

        // Build a simple array for your 3 tracks
        playlist = new AudioClip[] { track1, track2, track3 };
    }

    void Start()
    {
        // Start playing the first track
        PlayRandomTrack();
    }

    void Update()
    {
        // If the audio finished, load up the next track
        if (!audioSource.isPlaying)
        {
            PlayRandomTrack();
        }
    }

    private void PlayRandomTrack()
    {
        int randomIndex = Random.Range(0, playlist.Length);
        AudioClip nextTrack = playlist[randomIndex];
        audioSource.clip = nextTrack;
        audioSource.Play();
    }
}