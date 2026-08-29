using UnityEngine;

public class FinishedLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("rock"))
        {
            GameManager.Instance.GameWin();
        }
    }
}