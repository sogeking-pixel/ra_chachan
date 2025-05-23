using UnityEngine;

public class BreathingEffect : MonoBehaviour
{
    public RectTransform imageScan;
    public float frequency = 1f;
    public float amplitude = 0.1f;
    private float baseScale;
    private bool foundTarget = false;

    void Start()
    {
        baseScale = (Screen.width * 0.7f) / imageScan.rect.width;
    }

    void Update()
    {
        if (foundTarget) return;
        float scale = baseScale + Mathf.Sin(Time.time * frequency) * amplitude;
        imageScan.localScale = new Vector3(scale, scale, 1f);
    }

    public void OnTargetFound()
    {
        foundTarget = true;
        imageScan.gameObject.SetActive(false);
    }
    public void OnTargetLost()
    {
        foundTarget = false;
        imageScan.gameObject.SetActive(true);
    }
}

