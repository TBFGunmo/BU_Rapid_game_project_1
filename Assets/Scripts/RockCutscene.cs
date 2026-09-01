using System.Collections;
using UnityEngine;

public class RockCutscene : MonoBehaviour
{

    [Header("Cutscene Targets")]
    public Transform[] bouncePoints;

    [Header("Bounce Settings")]
    public float jumpHeight = 3f;      
    public float timePerBounce = 0.8f; 
    public float rotationSpeed = 360f;

    private Rigidbody2D rb;

    public Camera winCamera;
    public Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    public void PlayWinCutscene()
    {
        mainCamera.enabled = false;
        winCamera.enabled = true;

        StartCoroutine(BounceRoutine2D());
    }

    private IEnumerator BounceRoutine2D()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        for (int i = 0; i < bouncePoints.Length; i++)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = bouncePoints[i].position;
            float timer = 0f;

            while (timer < timePerBounce)
            {
                timer += Time.deltaTime;
                float t = timer / timePerBounce;

                Vector3 currentPos = Vector3.Lerp(startPos, endPos, t);

                float heightOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;
                currentPos.y += heightOffset;

                transform.position = currentPos;

                transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);

                yield return null;
            }

            transform.position = endPos;

        }

        //Debug.Log("End");

        GameManager.Instance.WinUI();


    }

}
