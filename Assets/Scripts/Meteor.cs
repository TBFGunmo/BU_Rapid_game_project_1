using UnityEngine;

public class Meteor : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject != null) 
        {

            if (collision.gameObject.CompareTag("Player"))
            {
                GameManager.Instance.GameOver();
                Destroy(this.gameObject);
            }
            else 
            {
                Destroy(this.gameObject);
            }

        }

        

    }
}
