using System.Collections;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    [Header("Audio Components")]
    public AudioSource bgmSource;

    [Header("Music Tracks")]
    public AudioClip normalBGM; 
    public AudioClip phase3BGM; 

    [Header("Volume Controls (Inspector)")]
    [Range(0f, 1f)] public float masterVolume = 0.4f;
    [Range(0f, 1f)] public float fadeVolume = 0.2f;   
    public float fadeTime = 2.0f;

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayNormalBGM();
    }

    private void OnValidate()
    {
        if (bgmSource != null && fadeCoroutine == null)
        {
            bgmSource.volume = masterVolume;
        }
    }
    public void PlayNormalBGM()
    {
        if (bgmSource == null || normalBGM == null) return;

        if (bgmSource.clip == normalBGM && bgmSource.isPlaying)
        {
            return; 
        }

        bgmSource.clip = normalBGM;
        bgmSource.volume = masterVolume;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlayPhase3BGM()
    {
        if (bgmSource == null || phase3BGM == null) return;

        bgmSource.clip = phase3BGM;
        bgmSource.volume = masterVolume;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void FadeBGMDown()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        float startVol = bgmSource.volume;
        float timer = 0f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVol, fadeVolume, timer / fadeTime);
            yield return null;
        }

        bgmSource.volume = fadeVolume;
    }
}