using UnityEngine;

public class StartBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("check_1");
        if (collision.gameObject.CompareTag("Player"))
        {
            print("check_2");
            GameManager.Instance.StartGame();
        }
    }
}
