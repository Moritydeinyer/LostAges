using UnityEngine;
using TMPro;

public class FPSdisplay : MonoBehaviour
{
    public TextMeshProUGUI FpsDisplay;
    private float pollingTime = 1f;
    private float time;
    private int frameCount;

    void Update()
    {
        time += Time.deltaTime;
        frameCount++;
        if (time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            FpsDisplay.text = frameRate.ToString();
            time -= pollingTime;
            frameCount = 0;
        }
    }
}
