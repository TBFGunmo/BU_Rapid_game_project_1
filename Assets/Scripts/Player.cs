using UnityEngine;
using TMPro;
using System.Collections;
using Unity.Mathematics;

public class Player : MonoBehaviour
{
    public float playerSpeed = 15f; //use
    public float RunSpeed = 25f;
    [SerializeField] private float currentSpeed = 0;

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

    public float CantcatchDistance = 1f;

    public float pushForce = 0f;
    public float knockBackForce = 0f;

    public float maxForce = 100f;
    public float minReboundForce = 5f;

    public float timeToMax = 5f;        
    public float timeToRebound = 2.5f;
    private float timer = 0f;

    private bool isCharging = false;
    private bool statusGoingDown = false;

    public bool isHoldRock = false;
    public bool alreadyKnockback = false;

    [SerializeField] private bool canHoldRock = false;

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

        currentSpeed = playerSpeed;
    }
    private void FixedUpdate()
    {
        rb.AddForce(autoWalkDir * currentSpeed);
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

        float distance = Physics2D.Distance(playerCol, rockCol).distance;

        if ((distance > CantcatchDistance)) // check for protect player press space when stone not in range for push or catch
        {
            canHoldRock = false;
        }
        else if (isHoldRock) // check is player holding a stone
        {
            currentSpeed = playerSpeed;

            if (Input.GetKeyDown(KeyCode.Space)) //start charging push
            {
                isCharging = true;
                pushForce = 0f;
                timer = 0f;
                statusGoingDown = false;
            }

            if (Input.GetKey(KeyCode.Space) && isCharging) // push force by time press
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

                //Debug.Log($" Force: {pushForce}");
            }

            if (Input.GetKeyUp(KeyCode.Space) && isCharging) // release space for push
            {
                //print("work");
                rock.PushRockUp(autoWalkDir, pushForce);
                alreadyKnockback = false;
                ResetCharge();
                
            }
        }
        else // after push
        {

            currentSpeed = RunSpeed; // set to run
            if (distance <= catchDistance)  // check stone in renge
            {
                canHoldRock = true;
            }

            if (canHoldRock && Input.GetKeyDown(KeyCode.Space)) // press for catch
            {
                isHoldRock = true;
            }
            
            
        }
    }

    private void OnCollisionStay(Collision collision) // not work      // if cant catch untill reach coli knockback player
    {
        print("check"); // print for check work but not work now
        if (!isHoldRock) // check of protect player from KnockBack when can catch stone
        {
            GameObject GO = collision.gameObject;
            if (GO != null)
            {
                if (GO.CompareTag("rock"))
                {
                    KnockBack();
                    isHoldRock = true;
                }
            }
        }
    }

    void ResetCharge()
    {
        isCharging = false;
        statusGoingDown = false;
        timer = 0f;
        pushForce = 0f;
    }

    private void KnockBack() 
    {
        print("back");
        alreadyKnockback = true;
        rb.AddForce(-autoWalkDir * knockBackForce, ForceMode2D.Impulse);
    }

    

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, catchDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, CantcatchDistance);
    }
        
    /*private IEnumerator ShowMessage(TMP_Text textUI)
    {
        textUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        textUI.gameObject.SetActive(false);
    }*/
}
