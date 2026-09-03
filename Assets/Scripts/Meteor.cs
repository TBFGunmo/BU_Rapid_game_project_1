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
                // เสกเอฟเฟค
                GameObject vfx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

                // สั่งทำลายวัตถุเอฟเฟคทิ้งหลังผ่านไป 2 วินาที (เพื่อไม่ให้ขยะล้นฉาก)
                // *หมายเหตุ: ปรับตัวเลข 2f ได้ตามความยาวเวลาของเอฟเฟคระเบิดที่คุณหามา
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
