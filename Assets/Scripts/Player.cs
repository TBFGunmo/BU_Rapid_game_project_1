using UnityEngine;
using TMPro;
using System.Collections;
using Unity.Mathematics;

public class Player : MonoBehaviour
{
    public float playerSpeed = 15f; //use

    public Rock rock;
    public QTEManager qteManager;
    Collider2D playerCol;
    Collider2D rockCol;

    public TMP_Text cooldownText;
    public TMP_Text outOfRangeText;
    public TMP_Text missedText;
    private Rigidbody2D rb;

    public Vector2 autoWalkDir = new Vector2(-1f, 0.5f).normalized;

    public float catchDistance = 2f;
    //public float pushForce = 10f;
    public float pushCooldown = 1.0f;
    private float lastPushTime;

    //-----------------------------------------------------   Gun new value for push system

    public float pushForce = 0f;

    public float maxForce = 100f;
    public float minReboundForce = 5f;

    public float timeToMax = 5f;        
    public float timeToRebound = 2.5f;
    private float timer = 0f;

    private bool isCharging = false;
    private bool statusGoingDown = false;

    public bool isHoldRock = false;

    //-----------------------------------------------------


    private Coroutine cdRoutine;
    private Coroutine missedRoutine;
    private Coroutine outOfRangeRoutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<Collider2D>();
        rockCol = rock.GetComponent<Collider2D>();  
        lastPushTime = -pushCooldown;
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))  // push
        {
            if (qteManager.isQTEActive) // check cooldown
            {
                bool isHit = qteManager.CheckHit();
                qteManager.StopQTE();

                if (isHit)
                {
                    rock.PushRockUp(autoWalkDir, pushForce); // push
                }
                else // show text
                {
                    if (missedRoutine != null) 
                    { 
                        StopCoroutine(missedRoutine); 
                    }

                    missedRoutine = StartCoroutine(ShowMessage(missedText));
                }

                lastPushTime = Time.time; // collect start cooldown
                return;
            }

            if (Time.time >= lastPushTime + pushCooldown) // cooldown reach
            {
                float distance = Physics2D.Distance(playerCol, rockCol).distance;

                if (distance <= catchDistance)
                {
                    rock.StartSlowdown(2f);
                    qteManager.StartQTE();
                }
                else // show text
                {
                    if (outOfRangeRoutine != null)  
                    { 
                        StopCoroutine(outOfRangeRoutine); 
                    }
                    outOfRangeRoutine = StartCoroutine(ShowMessage(outOfRangeText));
                }

            }
            else // still cooldown
            {
                if (cdRoutine != null) 
                { 
                    StopCoroutine(cdRoutine); 
                }

                cdRoutine = StartCoroutine(ShowMessage(cooldownText));
            }

        }
        */


            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                isCharging = true;
                pushForce = 0f;
                timer = 0f;
                statusGoingDown = false;
            }

            if (Input.GetKey(KeyCode.Space) && isCharging)
            {
                timer += Time.deltaTime;

                if (!statusGoingDown)
                {
                    float t = Mathf.Clamp01(timer / timeToMax);
                    pushForce = Mathf.Lerp(0f, maxForce, t);

                    if (timer >= timeToMax)
                    {
                        statusGoingDown = true;
                        timer = 0f;
                    }
                }
                else
                {
                    float t = Mathf.Clamp01(timer / timeToRebound);
                    pushForce = Mathf.Lerp(maxForce, minReboundForce, t);
                }

                Debug.Log($" Force: {pushForce}");
            }

            if (Input.GetKeyUp(KeyCode.Space) && isCharging)
            {
                rock.PushRockUp(autoWalkDir, pushForce);
                ResetCharge();
            }
    }

    void ResetCharge()
    {
        isCharging = false;
        statusGoingDown = false;
        timer = 0f;
        pushForce = 0f;
    }

    private void FixedUpdate()
    {
        rb.AddForce(autoWalkDir* playerSpeed);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, catchDistance);
    }
        
    private IEnumerator ShowMessage(TMP_Text textUI)
    {
        textUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        textUI.gameObject.SetActive(false);
    }
}
