using System.Collections;
using UnityEngine;

public class floatingText : MonoBehaviour
{
    public float amplitude = 10f; 
    public float frequency = 2f;  

    private Vector3 startPosition;
    private Coroutine floatCoroutine;

    void Start()
    {
        startPosition = transform.localPosition;

        StartFloating();
    }

    public void StartFloating()
    {
        if (floatCoroutine != null) StopCoroutine(floatCoroutine);
        floatCoroutine = StartCoroutine(FloatRoutine());
    }

    private IEnumerator FloatRoutine()
    {
        while (true)
        {
            float newY = Mathf.Sin(Time.time * frequency) * amplitude;

            transform.localPosition = startPosition + new Vector3(0f, newY, 0f);

            yield return null;
        }
    }
}
