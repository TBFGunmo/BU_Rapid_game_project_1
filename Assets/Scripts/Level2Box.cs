using UnityEngine;

public class Level2Box : MonoBehaviour
{
    private bool isTrigger = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTrigger)
        {
            //print("check_1");
            if (collision.gameObject.CompareTag("Player"))
            {
                //print("check_2");
                isTrigger = true;
                VolcanoManager.instant.StartSpawn();
                print("start Level 2");
            }
        }
    }
}
