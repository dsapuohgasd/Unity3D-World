using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    [SerializeField]
    private Material daySkybox;
    [SerializeField]
    private Material nightSkybox;

    private AudioSource daySound;
    private AudioSource nightSound;

    private GameObject lights;
    private Light sun;
    private Light moon;

    const float fullDayTime = 100f;
    float dayTime;
    float ligthAngle = -360 / fullDayTime;

    void Start()
    {
        lights = GameObject.Find("Lights");
        sun= GameObject.Find("Sun").GetComponent<Light>();
        moon= GameObject.Find("Moon").GetComponent<Light>();

        AudioSource[] audioSources = this.GetComponents<AudioSource>();
        daySound = audioSources[0];
        nightSound = audioSources[1];

        daySound.volume = nightSound.volume = GameSettings.EffectsVolume;

        if (!GameSettings.AllSoundsDisabled)
        {
            daySound.Play();
        }
        RenderSettings.skybox = daySkybox;
        dayTime = 0;
    }

    void Update()
    {
        lights.transform.Rotate(ligthAngle * Time.deltaTime, 0, 0);
    }

    private void LateUpdate()
    {
        daySound.volume = nightSound.volume =
            GameSettings.AllSoundsDisabled
            ? 0f
            : GameSettings.EffectsVolume;

        dayTime += Time.deltaTime;
        dayTime %= fullDayTime;
        float dayPhase = dayTime / fullDayTime;
        bool isNight = (dayPhase > 0.25f && dayPhase < 0.75f);
        if (isNight)
        {
            RenderSettings.skybox = nightSkybox;
        }
        else
        {
            RenderSettings.skybox = daySkybox;
        }
        float k = Mathf.Abs(Mathf.Cos(dayPhase * 2 * Mathf.PI) * 0.9f) + 0.1f;
        RenderSettings.skybox.SetFloat("_Exposure" , k);
        RenderSettings.ambientIntensity = (dayPhase > 0.25f && dayPhase < 0.75) ? k / 4f : k;
        moon.intensity = isNight ? k  /2f : 0;
        sun.intensity = isNight ? 0 : k;
    }
}