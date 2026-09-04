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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = Random.Range(0, timeToBlash);
        timeToShowSmoke = timeToBlash - timeToPreBlash;
        timeBlashing = timeToBlash + blashTime;

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
            smoke.SetActive(false);
            blash.SetActive(true);
        }

        if (currentTime > timeBlashing && blashing)
        {
            blashing = false;
            showing = false;
            smoke.SetActive(false);
            blash.SetActive(false);
            currentTime = 0f;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("cjsd");
        if (collision.gameObject.CompareTag("Player")) 
        {
            if (blashing) 
            {
                GameManager.Instance.GameOver();
            }
        }
    }
}
