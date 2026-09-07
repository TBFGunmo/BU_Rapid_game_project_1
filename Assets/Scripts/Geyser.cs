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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = UnityEngine.Random.Range(0, timeToBlash);
        print(currentTime);
        timeToShowSmoke = timeToBlash - timeToPreBlash;
        timeBlashing = blashTime / 3;

        smoke.SetActive(false);
        blash.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= timeToShowSmoke && !showing) 
        {
            showing = true;
            smoke.SetActive(true);
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


    private IEnumerator Blashing() 
    {
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
        preBlash2.SetActive(false);

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
