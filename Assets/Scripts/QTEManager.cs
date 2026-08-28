using UnityEngine;

public class QTEManager : MonoBehaviour   //unuse now but will change to charging bar
{
    public GameObject qtePanel;
    public RectTransform backgroundBar;
    public RectTransform targetZone;
    public RectTransform needle;

    public float needleSpeed = 500f;

    private float barWidth;
    private float currentPosX = 0f;
    private int direction = 1;
    public bool isQTEActive = false;

    void Start()
    {
        barWidth = backgroundBar.rect.width / 2f;
    }
    void Update()
    {
        if (!isQTEActive) return;
        currentPosX += needleSpeed * direction * Time.deltaTime;

        if (currentPosX >= barWidth)
        {
            currentPosX = barWidth;
            direction = -1;
        }
        else if (currentPosX <= -barWidth)
        {
            currentPosX = -barWidth;
            direction = 1;
        }

        needle.anchoredPosition = new Vector2(currentPosX, 0f);
    }
    public void StartQTE()
    {
        qtePanel.SetActive(true);
        isQTEActive = true;

        float targetHalfWidth = targetZone.rect.width / 2f;
        float randomX = Random.Range(-barWidth + targetHalfWidth, barWidth - targetHalfWidth);
        targetZone.anchoredPosition = new Vector2(randomX, 0f);

        currentPosX = -barWidth;
        needle.anchoredPosition = new Vector2(currentPosX, 0f);
    }
    public void StopQTE()
    {
        qtePanel.SetActive(false);
        isQTEActive = false;
    }
    public bool CheckHit()
    {
        float targetHalfWidth = targetZone.rect.width / 2f;
        float distance = Mathf.Abs(needle.anchoredPosition.x - targetZone.anchoredPosition.x);

        return distance <= targetHalfWidth;
    }

}