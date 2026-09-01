using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Pre_game : MonoBehaviour
{
    public TMP_Text textStart;

    public float blinkSpeed = 2f;      
    public float minAlpha = 0.2f;     
    public float maxAlpha = 1.0f;

    private Coroutine blinkCoroutine;

    //----------------------------------------
    

    void Start()
    {
        StartBlinking();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            SceneManager.LoadScene("Main_Game");
        }
    }


    public void StartBlinking()
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(BlinkRoutine());
    }

    public void StopBlinking()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            Color color = textStart.color;
            color.a = maxAlpha;
            textStart.color = color;
        }
    }

    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            float pingPongValue = Mathf.PingPong(Time.time * blinkSpeed, 1f);

            float currentAlpha = Mathf.Lerp(minAlpha, maxAlpha, pingPongValue);

            Color color = textStart.color;
            color.a = currentAlpha;
            textStart.color = color;

            yield return null;
        }
    }



}
