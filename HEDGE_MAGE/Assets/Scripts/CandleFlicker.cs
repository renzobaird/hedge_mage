using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickeringLight2D : MonoBehaviour
{
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.2f;
    public float flickerSpeed = 0.1f;

    private Light2D light2D;
    private float targetIntensity;
    private float flickerTimer;

    void Start()
    {
        light2D = GetComponent<Light2D>();
        if (light2D == null)
        {
            Debug.LogWarning("FlickeringLight2D: No Light2D component found.");
            enabled = false;
        }
        targetIntensity = light2D.intensity;
    }

    void Update()
    {
        flickerTimer -= Time.deltaTime;

        if (flickerTimer <= 0f)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            flickerTimer = flickerSpeed;
        }

        light2D.intensity = Mathf.Lerp(light2D.intensity, targetIntensity, Time.deltaTime * 10f);
    }
}
