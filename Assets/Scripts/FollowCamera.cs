using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject player;
    public Vector3 shakeOffset;
    public Vector3 offset;

    private void LateUpdate()
    {
        transform.position = new Vector3(
            player.transform.position.x + offset.x,
            player.transform.position.y + offset.y,
            transform.position.z
        ) + shakeOffset; 
    }
}

