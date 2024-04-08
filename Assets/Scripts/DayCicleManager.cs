using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    [SerializeField] Light sun;
    [SerializeField] Light moon;


    [SerializeField] AnimationCurve skyboxCurve;
    [SerializeField] AnimationCurve sunCurve;
    [SerializeField] AnimationCurve moonCurve;

    [SerializeField, Range(1, 3600)] float timeDayInSeconds;
    [SerializeField, Range(0f, 1f)] float timeProgress;

    [SerializeField] Material daySkybox;
    [SerializeField] Material nightSkybox;

    float sunIntensity;
    float moonIntensity;

    private void Start()
    {
        sunIntensity = sun.intensity;
        moonIntensity = moon.intensity;
    }

    private void Update()
    {
        timeProgress += Time.deltaTime / timeDayInSeconds;

        if (timeProgress > 1f)
        {
            timeProgress = 0f;
        }

        RenderSettings.skybox.Lerp(nightSkybox, daySkybox, skyboxCurve.Evaluate(timeProgress));
        RenderSettings.sun = skyboxCurve.Evaluate(timeProgress) > 0.1f ? sun : moon;
        DynamicGI.UpdateEnvironment();

        sun.transform.localRotation = Quaternion.Euler(timeProgress * 360f, 180, 0);
        moon.transform.localRotation = Quaternion.Euler(timeProgress * 360f + 180f, 180, 0);

        sun.intensity = sunIntensity * sunCurve.Evaluate(timeProgress);
        moon.intensity = moonIntensity * moonCurve.Evaluate(timeProgress);

        

    }
}

