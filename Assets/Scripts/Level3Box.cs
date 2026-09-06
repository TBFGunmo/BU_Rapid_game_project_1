using UnityEngine;

public class Level3Box : MonoBehaviour
{
    private bool isTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTrigger)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isTrigger = true;
                //VolcanoManager.instant.StartSpawn();
                GameManager.Instance.StartLevel3();
                print("start Level 3");
            }
        }
    }
}
