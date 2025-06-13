using UnityEngine;

public class CameraBreathing : MonoBehaviour
{
    public float verticalAmplitude = 0.05f;   // Up and down movement
    public float horizontalAmplitude = 0.02f; // Left and right movement
    public float frequency = 1f;              // Breathing speed

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * frequency) * verticalAmplitude;
        float offsetX = Mathf.Sin(Time.time * frequency * 0.5f) * horizontalAmplitude;

        transform.localPosition = startPos + new Vector3(offsetX, offsetY, 0f);
    }
}
