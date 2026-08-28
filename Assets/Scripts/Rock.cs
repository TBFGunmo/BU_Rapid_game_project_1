using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 oriVelocity;

    private Coroutine slowCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void PushRockUp(Vector2 direction, float force)
    {
        if (slowCoroutine != null) StopCoroutine(slowCoroutine);

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public void StartSlowdown(float duration)
    {
        StartCoroutine(SlowdownRoutine(duration));
    }

    private IEnumerator SlowdownRoutine(float duration)
    {
        oriVelocity = rb.linearVelocity;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        yield return new WaitForSeconds(duration);
        rb.linearVelocity = oriVelocity;
    }

    public void ResumeRockNormal()
    {
        if (slowCoroutine != null) StopCoroutine(slowCoroutine);

        rb.linearVelocity = oriVelocity;
    }

}
