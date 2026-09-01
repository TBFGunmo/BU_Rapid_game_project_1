using UnityEngine;
using TMPro;
using System.Collections;
using Unity.Mathematics;

public class Player : MonoBehaviour
{

    public Sprite sprite_45degree;
    public Sprite sprite_90degree;

    private SpriteRenderer spriteRenderer;

    public float playerSpeed = 15f; //use
    public float RunSpeed = 25f; // use
    [SerializeField] private float currentSpeed = 0; // use

    public Rock rock;
    public UIChargeManager chargeManager;
    public GameObject pushIconUI;
    public GameObject missedIconUI;

    Collider2D playerCol; // use
    Collider2D rockCol; // use

    public TMP_Text cooldownText;
    public TMP_Text outOfRangeText;
    public TMP_Text missedText;
    private Rigidbody2D rb; // use

    public Vector2 autoWalkDir = new Vector2(-1f, 0.5f).normalized; // use

    public Vector2 saveWalkDir; // use
    public Vector2 prestartWalkDir = new Vector2(-1f, 0f).normalized; // use

    public float catchDistance = 2f; // use
    //public float pushForce = 10f;
    public float pushCooldown = 1.0f;
    private float lastPushTime;

    //-----------------------------------------------------   Gun new value for push system

    public bool isStart = false;

    public float CantcatchDistance = 1f;

    public float pushForce = 0f;
    

    public float maxForce = 100f;
    public float minReboundForce = 5f;

    public float timeToMax = 5f;        
    public float timeToRebound = 2.5f;
    private float timer = 0f;

    private bool isCharging = false;
    private bool statusGoingDown = false;

    public bool isHoldRock = true;
    public bool alreadyKnockback = false;

    [SerializeField] private bool canHoldRock = false;

    public float pushingRockForceBegin = 5f;
    public float pushingRockForceMain = 2f;

    // public float knockBackForce = 0f; // unuse
    public float knockBackMultiplier = 1.5f; 
    public float minKnockBackForce = 5f;   
    public float maxKnockBackForce = 50f;

    public float holdDistanceDuringKnockback = 1.2f;

    private bool changesprite = false;

    //-----------------------------------------------------

    private Coroutine missedRoutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<Collider2D>();
        rockCol = rock.GetComponent<Collider2D>();

        saveWalkDir = autoWalkDir;

        autoWalkDir = prestartWalkDir;

        lastPushTime = -pushCooldown;

        currentSpeed = RunSpeed;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite_90degree;

    }
    private void FixedUpdate()
    {
        rb.AddForce(autoWalkDir * currentSpeed);

        if (isHoldRock && !isStart)
        {
            rock.PushRockUp(autoWalkDir, pushingRockForceBegin);
        }
        else if (isHoldRock) 
        {
            rock.PushRockUp(autoWalkDir, pushingRockForceMain);
        }
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

        if (isStart)
        {
            if (!changesprite)
            {
                spriteRenderer.sprite = sprite_45degree;
                changesprite = true;
            }


            float distance = Physics2D.Distance(playerCol, rockCol).distance;

            if ((distance > CantcatchDistance)) // check for protect player press space when stone not in range for push or catch
            {
                canHoldRock = false;
                isHoldRock = false;
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

                    chargeManager.UpdateChargeBar(pushForce, maxForce);

                    //Debug.Log($" Force: {pushForce}");
                }

                if (Input.GetKeyUp(KeyCode.Space) && isCharging) // release space for push
                {
                    //print("work");
                    rock.PushRockUp(autoWalkDir, pushForce);
                    alreadyKnockback = false;
                    isHoldRock = false;
                    lastPushTime = Time.time;
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

                    missedIconUI.SetActive(false);
                }


            }
            if (pushIconUI != null)
            {
                bool showIcon = !isHoldRock && (distance <= catchDistance) && (Time.time > lastPushTime + 0.5f);
                pushIconUI.SetActive(showIcon);
            }
        }
    }


    public void StartGame() 
    {
        isStart = true;
        autoWalkDir = saveWalkDir;

        currentSpeed = RunSpeed;
   
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (!isHoldRock) // check of protect player from KnockBack when can catch stone
        {
            GameObject GO = collision.gameObject;
            if (GO != null)
            {
                if (GO.CompareTag("rock"))
                {
                    //print("check");

                    float impactSpeed = collision.relativeVelocity.magnitude;
                    float calculatedForce = impactSpeed * knockBackMultiplier;
                    calculatedForce = Mathf.Clamp(calculatedForce, minKnockBackForce, maxKnockBackForce);

                    KnockBack(calculatedForce);

                    StartCoroutine(ShowMissedUI());

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

        if (chargeManager != null)
        {
            chargeManager.ResetChargeBar();
        }
    }

    private void KnockBack(float force) 
    {
        if (!alreadyKnockback)
        {
            alreadyKnockback = true;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(-autoWalkDir * force, ForceMode2D.Impulse);

            //rock.PushRockUp(-autoWalkDir, force * 2);
        }
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

    private IEnumerator ShowMissedUI()
    {
        if (missedIconUI != null)
        {
            missedIconUI.SetActive(true);
            yield return new WaitForSeconds(1.0f); 
            missedIconUI.SetActive(false);
        }
    }
}
