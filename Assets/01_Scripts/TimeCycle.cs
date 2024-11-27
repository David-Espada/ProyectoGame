using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCycle : MonoBehaviour
{
    [Header("Counters")]
    public int days = 1;
    public int nights = 0;
    public int daysToStartSpawn = 2;
    public bool dayWaveComplete = false;

    [Header("Cycle Settings")]
    public float dayDuration = 120f; // Duración de un ciclo completo en segundos

    [Header("Sun Settings")]
    public Light sunLight;
    public Gradient lightColorGradient;

    [Header("Skybox Settings")]
    public Material skyboxMaterial; // Material del HDRI
    [Tooltip("Exposición mínima para el HDRI durante la noche")]
    public float nightExposure = 0.2f;
    [Tooltip("Exposición máxima para el HDRI durante el día")]
    public float dayExposure = 1.2f;

    [Header("Intensity Settings")]
    public float minIntensity = 0.1f;
    public float maxIntensity = 1.0f;

    public bool isDay; // True si es de día, false si es de noche

    private float currentTime;
    private float originalExposure; // Valor original de la exposición del material
    private bool transitionedToDay; // Para detectar cambios de ciclo día-noche
    private bool transitionedToNight;

    [Header("References")]
    public MapManager mapManager;
    public EnemySpawner enemySpawner;

    void Start()
    {
        if (sunLight == null || skyboxMaterial == null)
        {
            Debug.LogError("¡Por favor asigna la Luz Solar y el Material del Skybox en el Inspector!");
            enabled = false;
        }

        // Guardar el valor original de la exposición
        originalExposure = skyboxMaterial.GetFloat("_Exposure");

        // Inicializar el ciclo en el inicio del día
        currentTime = 0.25f; // 0.25 representa el amanecer o inicio del día
        transitionedToDay = false;
        transitionedToNight = false;
    }

    void Update()
    {
        if (!isDay)
        {
            dayWaveComplete = false;
        }
        if (!dayWaveComplete && days >= daysToStartSpawn && isDay)
        {
            enemySpawner.GenerateEnemys();
            dayWaveComplete = true;
        }
        // Progresión del tiempo
        currentTime += Time.deltaTime / dayDuration;
        if (currentTime > 1f) currentTime = 0f;

        // Rotación del sol
        float sunRotation = currentTime * 360f;
        sunLight.transform.rotation = Quaternion.Euler(new Vector3(sunRotation - 90f, 0f, 0f));

        // Ajustar la intensidad de la luz
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.Clamp01(Mathf.Sin(currentTime * Mathf.PI)));
        sunLight.intensity = intensity;

        // Ajustar el color de la luz
        if (lightColorGradient != null)
        {
            sunLight.color = lightColorGradient.Evaluate(currentTime);
        }

        // Detección de día/noche
        bool previousIsDay = isDay;
        isDay = sunLight.transform.rotation.eulerAngles.x > 0 && sunLight.transform.rotation.eulerAngles.x < 180;

        // Incrementar contadores al cambiar de ciclo
        if (isDay && !previousIsDay && !transitionedToDay)
        {
            days++;
            transitionedToDay = true;
            transitionedToNight = false;
        }
        else if (!isDay && previousIsDay && !transitionedToNight)
        {
            nights++;
            transitionedToNight = true;
            transitionedToDay = false;
        }

        mapManager.sheeps.ForEach(s => s.isDay = isDay);
        mapManager.Wheats.ForEach(w => w.GetComponent<WheatPlace>().IsDay = isDay);

        // Ajustar la exposición del HDRI
        if (skyboxMaterial != null)
        {
            float targetExposure = isDay ? dayExposure : nightExposure;
            skyboxMaterial.SetFloat("_Exposure", Mathf.Lerp(nightExposure, dayExposure, Mathf.Clamp01(Mathf.Sin(currentTime * Mathf.PI))));
        }
    }

    void OnDisable()
    {
        // Restaurar el valor original de la exposición al detenerse la ejecución
        if (skyboxMaterial != null)
        {
            skyboxMaterial.SetFloat("_Exposure", originalExposure);
        }
    }

}
