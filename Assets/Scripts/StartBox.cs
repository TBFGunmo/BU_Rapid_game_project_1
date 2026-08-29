using UnityEngine;

public class StartBox : MonoBehaviour
{
    public Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("check_1");
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!(player.isStart))
            {
                print("check 2");
                player.StartGame();
            }
        }
    }
}
