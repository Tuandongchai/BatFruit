using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;



public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private int defaultPoolSize = 10;
    [SerializeField] private int maxPoolSize = 30;
    [SerializeField] private AudioSO audioSO;
    private AudioSource musicSource;
    private ObjectPool<AudioSource> audioPool;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        musicSource = gameObject.AddComponent<AudioSource>();
        

        DontDestroyOnLoad(gameObject);

        audioPool = new ObjectPool<AudioSource>(
            CreateAudioSource,
            OnGetAudioSource,
            OnReleaseAudioSource,
            DestroyAudioSource,
            false,
            defaultPoolSize,
            maxPoolSize
        );
    }
    private void Start()
    {
        PlayMusicLoop(audioSO.InGameMusicBG, 0.75f);
    }
    private AudioSource CreateAudioSource()
    {
        GameObject go = new GameObject("PooledAudioSource");
        go.transform.parent = transform; 

        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f; 
        audioSource.playOnAwake = false;

        go.AddComponent<AudioContext>(); 
        return audioSource;
    }

    private void OnGetAudioSource(AudioSource source)
    {
        source.gameObject.SetActive(true);
    }

    private void OnReleaseAudioSource(AudioSource source)
    {
        source.gameObject.SetActive(false);
    }

    private void DestroyAudioSource(AudioSource source)
    {
        Destroy(source.gameObject);
    }

    // phat tai vi tri cu the
    public void PlayAt(Vector3 worldPosition, AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        AudioSource src = audioPool.Get();
        src.clip = clip;
        src.volume = volume;
        src.transform.position = worldPosition;
        src.PlayOneShot(clip);

        StartCoroutine(ReleaseAfter(src, clip.length));
    }

    // phat toam man hinh
    public void Play2D(AudioClip clip, float volume = 1f)
    {
        if(PlayerPrefs.GetInt("SFX")==0) return;
        PlayAt(Vector3.zero, clip, volume); 
    }

    private IEnumerator ReleaseAfter(AudioSource src, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioPool.Release(src);
    }

    //music bg
    public void PlayMusicLoop(AudioClip clip, float volume = 0.5f)
    {
        if (clip == null || PlayerPrefs.GetInt("Music") == 0) return;

        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.spatialBlend = 0f;
        musicSource.volume = 0.5f;
        musicSource.Play();
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void PlayMusic()
    {
        PlayMusicLoop(audioSO.InGameMusicBG, 0.75f);
    }
}
