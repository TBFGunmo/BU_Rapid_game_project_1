using System.Data;
using UnityEngine;

public class UI_Start : MonoBehaviour
{
    public Canvas mainUI;

    public GameObject[] Comic;
    private bool endComic = false;

    private int currentA = 0;

    public GameObject[] tutorial;
    private bool endTutorial = false;

    public float timeToSkip = 3f;
    private float currentTime = 0f;


    void Start()
    {
        mainUI.gameObject.SetActive(false);
        currentA = 0;

        Comic[currentA].SetActive(true);
        currentA++;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (!endComic)
            {
                if ((currentA + 1) <= Comic.Length)
                {
                    foreach (GameObject c in Comic)
                    {
                        c.SetActive(false);
                    }

                    Comic[currentA].SetActive(true);
                    currentA++;

                    if (!((currentA + 1) <= Comic.Length) )
                    {
                        endComic = true;
                        currentA = 0;
                    }

                }
                else
                {
                    endComic = true;
                    currentA = 0;
                }
            }
            else if (!endTutorial)
            {
                if ((currentA + 1) <= tutorial.Length)
                {
                    foreach (GameObject c in tutorial)
                    {
                        c.SetActive(false);
                    }

                    tutorial[currentA].SetActive(true);
                    currentA++;

                    if (!((currentA + 1) <= tutorial.Length))
                    {
                        endTutorial = true;
                        currentA = 0;
                    }
                }
                else
                {
                    endTutorial = true;
                    currentA = 0;
                }
            }
            else 
            {
                mainUI.gameObject.SetActive(true);
                this.gameObject.SetActive(false);
                GameManager.Instance.player.gameEnd = false;

                print("end");
            }
        }
        
        if (Input.GetKey(KeyCode.Space)) 
        {
            currentTime += Time.deltaTime;


            if (currentTime >= timeToSkip && !endComic)
            {
                foreach (GameObject c in Comic)
                {
                    c.SetActive(false);
                }

                endComic = true;
                currentA = 0;

                foreach (GameObject c in tutorial)
                {
                    c.SetActive(false);
                }

                tutorial[currentA].SetActive(true);
                currentA++;

            }

        }

        if (Input.GetKeyUp(KeyCode.Space)) 
        {
            currentTime = 0;
        }
    }
}
