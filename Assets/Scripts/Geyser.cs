using System.Collections;
using UnityEngine;
using UnityEngine.Assemblies;

public class Geyser : MonoBehaviour
{
    [Header("time setting")]
    public float timeToBlash = 10f;
    public float timeToPreBlash = 3f;
    public float blashTime = 3f;

    private float currentTime = 0f;
    private float timeToShowSmoke = 0f;
    private float timeBlashing = 0f;

    private bool blashing = false;
    private bool showing = false;

    [Header("Link GameOBJ")]
    public GameObject smoke;
    public GameObject blash;
    public GameObject preBlash1;
    public GameObject preBlash2;

    private Coroutine blashCoroutine;

    private bool isStopped = false;

    [Header("Geyser Audio")]
    public AudioClip smokeSound; 
    public AudioClip eruptSound; 
    private AudioSource audioSource;
    [Range(0f, 1f)] public float geyserVolume = 0.3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = UnityEngine.Random.Range(0, timeToBlash);
        print(currentTime);
        timeToShowSmoke = timeToBlash - timeToPreBlash;
        timeBlashing = blashTime / 3;

        smoke.SetActive(false);
        blash.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) 
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = geyserVolume;

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; 
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 10f;
        audioSource.maxDistance = 25f;

    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (GameManager.Instance != null && GameManager.Instance.player != null && GameManager.Instance.player.gameEnd)
        {
            //print("check2");
            if (!isStopped)
            {
                //print("check1");
                isStopped = true;

                if (blashCoroutine != null)
                {
                    StopCoroutine(blashCoroutine);
                }

                if (audioSource != null)
                {
                    print("check");
                    audioSource.Stop();
                }

                if (smoke != null) smoke.SetActive(false);
                if (blash != null) blash.SetActive(false);
                if (preBlash1 != null) preBlash1.SetActive(false);
                if (preBlash2 != null) preBlash2.SetActive(false);
            }
            return;
        }
        else
        {
            if (currentTime >= timeToShowSmoke && !showing)
            {
                showing = true;
                smoke.SetActive(true);

                if (smokeSound != null)
                {
                    audioSource.clip = smokeSound;
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }

            if (currentTime >= timeToBlash && !blashing)
            {
                blashing = true;

                if (blashCoroutine != null)
                {
                    StopCoroutine(blashCoroutine);
                }

                blashCoroutine = StartCoroutine(Blashing());

            }
        }

    }


    private IEnumerator Blashing() 
    {
        if (eruptSound != null)
        {
            audioSource.Stop();
            audioSource.clip = eruptSound;
            audioSource.loop = false;
            audioSource.Play();
        }

        smoke.SetActive(false);
        preBlash1.SetActive(true);

        yield return new WaitForSeconds(0.15f);

        preBlash1.SetActive(false);
        preBlash2.SetActive(true);

        yield return new WaitForSeconds(0.15f);

        preBlash2.SetActive(false);
        blash.SetActive(true);

        yield return new WaitForSeconds(blashTime);

        smoke.SetActive(false);
        blash.SetActive(false);
        preBlash2.SetActive(false);
        preBlash1.SetActive(false);

        if (audioSource != null)
        {
            audioSource.Stop();
        }

        blashing = false;
        showing = false;

        currentTime = 0f;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("cjsd");
        if (collision.gameObject.CompareTag("Player")) 
        {
            if (blashing && !GameManager.Instance.player.gameEnd) 
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (blashing && !GameManager.Instance.player.gameEnd)
            {
                GameManager.Instance.GameOver();
            }
        }
    }
}
