using System.Collections;
using UnityEngine;

public class VolcanoManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject rockPrefab;     
    public GameObject shadowPrefab;  
    public LayerMask groundLayer;     

    [Header("Spawn Settings")]
    public float spawnRadius = 6f;    
    public float fallHeight = 15f;    
    public float warningTime = 1.2f;  

    [Header("Timer & Difficulty")]
    public float maxTime = 60f;       
    public float currentTime = 60f;
    public float slowSpawnRate = 3f;  
    public float fastSpawnRate = 0.5f;

    [Header("Pre-Drop Shake Settings")]
    public float shakeDuration = 0.4f;  
    public float shakeIntensity = 0.25f;

    [Header("Audio")]
    public AudioClip earthquakeSound;
    public AudioClip meteorSound;
    [Range(0f, 1f)] public float maxMeteorVolume = 0.5f;

    public static VolcanoManager instant;

    private void Start()
    {
        instant = this;
    }

    public void StartSpawn() 
    {
        StartCoroutine(SpawnRoutine());
    }

    private void Update()
    {
        currentTime = GameManager.Instance.timeRemain;
    }

    private IEnumerator SpawnRoutine()
    {
        while (currentTime > 0 && !(GameManager.Instance.player.gameEnd))
        {
            SpawnHazard();

            float timeRatio = currentTime / GameManager.Instance.timeGame;
            float currentSpawnDelay = Mathf.Lerp(fastSpawnRate, slowSpawnRate, timeRatio);

            yield return new WaitForSeconds(currentSpawnDelay);
        }
    }

    private void SpawnHazard()
    {

        float randomX = player.position.x + Random.Range(-spawnRadius, spawnRadius);

        Vector2 rayStart = new Vector2(randomX, player.position.y + 20f);
        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, 50f, groundLayer);

        if (hit.collider != null)
        {
            StartCoroutine(WarningAndDrop(hit.point, hit.normal));
        }
    }

    private IEnumerator WarningAndDrop(Vector2 groundPoint, Vector2 hitNormal)
    {
        if (earthquakeSound != null)
        {
            GameObject eqObj = new GameObject("EarthquakeAudio");
            eqObj.transform.position = groundPoint;
            AudioSource eqSource = eqObj.AddComponent<AudioSource>();
            eqSource.clip = earthquakeSound;
            eqSource.volume = 0.7f;
            eqSource.Play();
            Destroy(eqObj, shakeDuration);
        }

        AudioSource audioSource = null;
        if (meteorSound != null)
        {
            GameObject audioObj = new GameObject("MeteorAudio");
            audioObj.transform.position = groundPoint;
            audioSource = audioObj.AddComponent<AudioSource>();
            audioSource.clip = meteorSound;
            audioSource.volume = 0f; 
            audioSource.Play();

            Destroy(audioObj, meteorSound.length + 1f);
        }

        float timer = 0f;
        Camera mainCam = Camera.main;
        FollowCamera followCam;

        if (mainCam != null)
        {
            followCam = mainCam.GetComponent<FollowCamera>();
        }
        else
        {
            followCam = null;
        }

        while (timer < shakeDuration)
        {
            timer += Time.deltaTime;

            if (audioSource != null)
            {
                audioSource.volume = Mathf.Lerp(0f, maxMeteorVolume / 2f, timer / shakeDuration);
            }

            if (followCam != null)
            {
                Vector2 shakeOffset = Random.insideUnitCircle * shakeIntensity;
                followCam.shakeOffset = new Vector3(shakeOffset.x, shakeOffset.y, 0f);
            }

            yield return null;
        }

        if (followCam != null) followCam.shakeOffset = Vector3.zero;

        GameObject shadow = null;
        SpriteRenderer shadowSr = null;
        if (shadowPrefab != null)
        {
            Quaternion shadowRotation = Quaternion.FromToRotation(Vector3.up, hitNormal);

            shadow = Instantiate(shadowPrefab, groundPoint, shadowRotation);
            shadowSr = shadow.GetComponent<SpriteRenderer>();

            if (shadowSr != null)
            {
                Color c = shadowSr.color;
                c.a = 0f;
                shadowSr.color = c;
            }
        }

        timer = 0f;
        while (timer < warningTime)
        {
            timer += Time.deltaTime;
            if (audioSource != null)
            {
                audioSource.volume = Mathf.Lerp(maxMeteorVolume / 2f, maxMeteorVolume, timer / warningTime);
            }

            if (shadowSr != null)
            {
                Color c = shadowSr.color;
                c.a = Mathf.Lerp(0f, 0.7f, timer / warningTime);
                shadowSr.color = c;
            }
            yield return null;
        }

        Vector2 spawnPos = new Vector2(groundPoint.x, groundPoint.y + fallHeight);
        GameObject rock = Instantiate(rockPrefab, spawnPos, Quaternion.identity);

        while (rock != null)
        {
            if (shadowSr != null)
            {
                float currentDist = rock.transform.position.y - groundPoint.y;
                float fallRatio = Mathf.Clamp01(1f - (currentDist / fallHeight));

                Color c = shadowSr.color;
                c.a = Mathf.Lerp(0.7f, 1f, fallRatio);
                shadowSr.color = c;
            }

            yield return null;
        }

        if (shadow != null)
        {
            Destroy(shadow);
        }
    }
}
