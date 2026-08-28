using UnityEngine;
using TMPro;
using System.Collections;

public class Player : MonoBehaviour
{
    public float playerSpeed = 15f;
    public Rock rock;

    public QTEManager qteManager;
    public TMP_Text cooldownText;
    public TMP_Text outOfRangeText;
    public TMP_Text missedText;

    public Vector2 autoWalkDir = new Vector2(-1f, 0.5f).normalized;
    private Rigidbody2D rb;

    public float catchDistance = 2f;
    public float pushForce = 10f;
    public float pushCooldown = 1.0f;
    private float lastPushTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
                    rock.ResumeRockNormal();
                    StartCoroutine(ShowMissedMessage());
                }

                lastPushTime = Time.time;
                return;
            }

            if (Time.time >= lastPushTime + pushCooldown)
            {
                Collider2D playerCol = GetComponent<Collider2D>();
                Collider2D rockCol = rock.GetComponent<Collider2D>();
                float distance = Physics2D.Distance(playerCol, rockCol).distance;

                if (distance <= catchDistance)
                {
                    rock.StartSlowdown(2f);
                    qteManager.StartQTE();
                }
                else
                {
                    StartCoroutine(ShowOutOfRangeMessage());
                }

            }
            else
            {
               
                print("cdcd");
                StartCoroutine(ShowCooldownMessage());
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

    private IEnumerator ShowCooldownMessage()
    {
        cooldownText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        cooldownText.gameObject.SetActive(false);
    }
  private IEnumerator ShowOutOfRangeMessage()
    {
        outOfRangeText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        outOfRangeText.gameObject.SetActive(false);
    }
  private IEnumerator ShowMissedMessage()
    {
        missedText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        missedText.gameObject.SetActive(false);
    }
}
