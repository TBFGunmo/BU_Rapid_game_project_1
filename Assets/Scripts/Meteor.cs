using UnityEngine;

public class Meteor : MonoBehaviour
{
    [Header("Effects")]
    public GameObject explosionPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject != null) 
        {

            if (explosionPrefab != null)
            {
                GameObject vfx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(vfx, 2f);
            }

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
