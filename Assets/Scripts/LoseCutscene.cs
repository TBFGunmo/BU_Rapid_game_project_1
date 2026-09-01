using System.Collections;
using UnityEngine;

public class LoseCutscene : MonoBehaviour
{
    [Header("Cameras")]
    public Camera mainCamera;
    public Camera endCamera;

    [Header("Cutscene Targets")]
    public Transform volcanoPoint;
    public Transform skyPoint; 
    public Transform villagePoint;

    [Header("Pan Settings")]
    public float timeToVolcano = 1.5f;
    public float delayAtVolcano = 1.0f;
    public float timeToSky = 1.5f; 
    public float timeToVillage = 2.5f;  

    [Header("Zoom Settings")]
    public float zoomInSize = 3.0f;
    public float zoomInTime = 1.0f;

    [Header("Shake Settings")]
    public float shakeIntensity = 0.5f; 

    public GameObject RedRockPrefab; 
    public float rockRotationSpeed = 360f;

    public void PlayLoseCutscene()
    {
        StartCoroutine(CutsceneRoutine());
    }

    private IEnumerator CutsceneRoutine()
    {
        mainCamera.enabled = false;
        endCamera.enabled = true;

        endCamera.transform.position = mainCamera.transform.position;
        float originalSize = endCamera.orthographicSize;

        Vector3 startPos = endCamera.transform.position;
        Vector3 volcanoPos = new Vector3(volcanoPoint.position.x, volcanoPoint.position.y, startPos.z);

        float timer = 0f;
        while (timer < timeToVolcano)
        {
            timer += Time.deltaTime;
            endCamera.transform.position = Vector3.Lerp(startPos, volcanoPos, timer / timeToVolcano);
            yield return null;
        }
        endCamera.transform.position = volcanoPos;

        timer = 0f;
        while (timer < zoomInTime)
        {
            timer += Time.deltaTime;
            endCamera.orthographicSize = Mathf.Lerp(originalSize, zoomInSize, timer / zoomInTime);
            yield return null;
        }
        endCamera.orthographicSize = zoomInSize;

        float shakeTimer = 0f;
        while (shakeTimer < delayAtVolcano)
        {
            shakeTimer += Time.deltaTime;
            Vector2 shakeOffset = Random.insideUnitCircle * shakeIntensity;
            endCamera.transform.position = volcanoPos + new Vector3(shakeOffset.x, shakeOffset.y, 0f);
            yield return null;
        }
        endCamera.transform.position = volcanoPos;

        GameObject meteor = null;
        meteor = Instantiate(RedRockPrefab, volcanoPoint.position, Quaternion.identity);
       

        timer = 0f;
        Vector3 skyPos = new Vector3(skyPoint.position.x, skyPoint.position.y, startPos.z);
        while (timer < timeToSky)
        {
            timer += Time.deltaTime;
            float t = timer / timeToSky;

            endCamera.transform.position = Vector3.Lerp(volcanoPos, skyPos, t);

            if (meteor != null)
            {
                meteor.transform.position = Vector3.Lerp(volcanoPoint.position, skyPoint.position, t);
                meteor.transform.Rotate(0f, 0f, -rockRotationSpeed * Time.deltaTime);
            }

            yield return null;
        }
        endCamera.transform.position = skyPos;

        timer = 0f;
        Vector3 villagePos = new Vector3(villagePoint.position.x, villagePoint.position.y, startPos.z);
        while (timer < timeToVillage)
        {
            timer += Time.deltaTime;
            float t = timer / timeToVillage;

            endCamera.transform.position = Vector3.Lerp(skyPos, villagePos, t);
            endCamera.orthographicSize = Mathf.Lerp(zoomInSize, originalSize, t);

            if (meteor != null)
            {
                meteor.transform.position = Vector3.Lerp(skyPoint.position, villagePoint.position, t);
                meteor.transform.Rotate(0f, 0f, -rockRotationSpeed * Time.deltaTime);
            }

            yield return null;
        }
        endCamera.transform.position = villagePos;
        endCamera.orthographicSize = originalSize;

        if (meteor != null)
        {
            meteor.SetActive(false);
        }

        yield return new WaitForSeconds(3.0f);
        GameManager.Instance.LoseUI();
    }
}