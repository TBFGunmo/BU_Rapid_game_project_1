using UnityEngine;
using TMPro;
using System.Collections;

public class Player : MonoBehaviour
{
    public float playerSpeed = 15f;

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
    public float pushForce = 10f;
    public float pushCooldown = 1.0f;
    private float lastPushTime;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (qteManager.isQTEActive)
            {
                bool isHit = qteManager.CheckHit();
                qteManager.StopQTE();

                if (isHit)
                {
                    rock.PushRockUp(autoWalkDir, pushForce);
                }
                else
                {
                    if (missedRoutine != null) StopCoroutine(missedRoutine);
                    missedRoutine = StartCoroutine(ShowMessage(missedText));
                }

                lastPushTime = Time.time;
                return;
            }

            if (Time.time >= lastPushTime + pushCooldown)
            {
                float distance = Physics2D.Distance(playerCol, rockCol).distance;

                if (distance <= catchDistance)
                {
                    rock.StartSlowdown(2f);
                    qteManager.StartQTE();
                }
                else
                {
                    if (outOfRangeRoutine != null) StopCoroutine(outOfRangeRoutine);
                    outOfRangeRoutine = StartCoroutine(ShowMessage(outOfRangeText));
                }

            }
            else
            {
                if (cdRoutine != null) StopCoroutine(cdRoutine);
                cdRoutine = StartCoroutine(ShowMessage(cooldownText));
            }

        }
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
