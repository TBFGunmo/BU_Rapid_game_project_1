using System.Collections;
using UnityEngine;

public class VolcanoManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject rockPrefab;     
    public GameObject warningPrefab;  
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

            StartCoroutine(WarningAndDrop(hit.point));
        }
    }

    private IEnumerator WarningAndDrop(Vector2 groundPoint)
    {

        GameObject warning = Instantiate(warningPrefab, groundPoint, Quaternion.identity);

        yield return new WaitForSeconds(warningTime);


        Destroy(warning);
        Vector2 spawnPos = new Vector2(groundPoint.x, groundPoint.y + fallHeight);
        Instantiate(rockPrefab, spawnPos, Quaternion.identity);
    }
}
