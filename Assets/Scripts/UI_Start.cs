using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class UI_Start : MonoBehaviour
{
    public Canvas mainUI;

    public GameObject[] Comic;
    private bool endComic = false;

    private int currentA = 0;

    public GameObject[] tutorial;
    private bool endTutorial = false;

    public GameObject cutSceneUI;

    public float timeToSkip = 3f; // time to hold for skip set in inspector
    private float currentTime = 0f; // current holding time , reset when release space before reach timeToSkip

    public Image skipProgressBar;

    void Start()
    {
        mainUI.gameObject.SetActive(false);
        currentA = 0;

        Comic[currentA].SetActive(true);
        currentA++;

        cutSceneUI.SetActive(false);

        if (skipProgressBar != null)
        {
            skipProgressBar.fillAmount = 0f;
        }

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
                foreach (GameObject c in Comic)
                {
                    c.SetActive(false);
                }

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
                GameManager.Instance.player.gameEnd = false;

                cutSceneUI.SetActive(true);

                this.gameObject.SetActive(false);
                



                //print("end");
            }
        }
        
        if (Input.GetKey(KeyCode.Space))    // <----------------- hold to skip
        {
            currentTime += Time.deltaTime;

            if (skipProgressBar != null)
            {
                skipProgressBar.fillAmount = currentTime / timeToSkip;
            }

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

                currentTime = 0f;
                if (skipProgressBar != null)
                {
                    skipProgressBar.fillAmount = 0f;
                }
            }
            else if (currentTime >= timeToSkip && !endTutorial) 
            {
                foreach (GameObject c in tutorial)
                {
                    c.SetActive(false);
                }

                foreach (GameObject c in Comic)
                {
                    c.SetActive(false);
                }

                endTutorial = true;
                currentA = 0;

                if (skipProgressBar != null)
                {
                    skipProgressBar.fillAmount = 0f;
                }

                mainUI.gameObject.SetActive(true);
                GameManager.Instance.player.gameEnd = false;

                cutSceneUI.SetActive(true);

                this.gameObject.SetActive(false);
            }

        }

        if (Input.GetKeyUp(KeyCode.Space))  // reset
        {
            currentTime = 0;
            if (skipProgressBar != null)
            {
                skipProgressBar.fillAmount = 0f;
            }
        }
    }
}
