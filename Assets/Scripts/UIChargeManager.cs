using UnityEngine;
using UnityEngine.UI;

public class UIChargeManager : MonoBehaviour
{
    public RectTransform needlePivot; 

    public float minAngle = 90f;   
    public float maxAngle = -90f; 

    void Start()
    {
        ResetChargeBar();
    }

    public void UpdateChargeBar(float currentForce, float maxForce)
    {
        float percent = currentForce / maxForce;
        float currentAngle = Mathf.Lerp(minAngle, maxAngle, percent);

        if (needlePivot != null)
        {
            needlePivot.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
        }
    }

    public void ResetChargeBar()
    {
        if (needlePivot != null)
        {
            needlePivot.localRotation = Quaternion.Euler(0f, 0f, minAngle);
        }
    }
}