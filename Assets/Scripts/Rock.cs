using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 oriVelocity;

    [Header("Rock Audio")]
    public AudioClip rockSound;
    [Range(0f, 1f)] public float rockVolume = 0.5f;
    private AudioSource audioSource;

    //public Player Player;

    private Coroutine slowCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (rockSound != null)
        {
            audioSource.clip = rockSound;
            audioSource.volume = rockVolume;
            audioSource.loop = true;
            audioSource.spatialBlend = 0f; 
            audioSource.Play();
        }

    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.player != null)
        {
            if (GameManager.Instance.player.gameEnd)
            {
                if (audioSource != null && audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
            else
            {
                if (audioSource != null && !audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }
    }

    private void OnValidate()
    {
        if (audioSource != null)
        {
            audioSource.volume = rockVolume;
        }
    }

    public void PushRockUp(Vector2 direction, float force)
    {
        if (slowCoroutine != null) StopCoroutine(slowCoroutine);

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

   /* public void StartSlowdown(float duration)
    {
        slowCoroutine = StartCoroutine(SlowdownRoutine(duration));
    }

    private IEnumerator SlowdownRoutine(float duration)
    {
        oriVelocity = rb.linearVelocity;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        yield return new WaitForSeconds(duration);
        rb.linearVelocity = oriVelocity;
    }

    public void ResumeRockNormal()
    {
        if (slowCoroutine != null) StopCoroutine(slowCoroutine);

        rb.linearVelocity = oriVelocity;
    }*/

}
