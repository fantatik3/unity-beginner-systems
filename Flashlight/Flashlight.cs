using UnityEngine;

[RequireComponent(typeof(Light))]
public class Flashlight : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Key to toggle the flashlight.")]
    public KeyCode toggleKey = KeyCode.F;

    [Tooltip("Whether the flashlight starts on.")]
    public bool startOn = false;

    [Tooltip("If true, the light slowly drains battery over time.")]
    public bool useBattery = false;

    [Tooltip("Battery drain rate per second (0–1).")]
    [Range(0f, 1f)] public float drainRate = 0.01f;

    [Tooltip("How long the battery lasts in seconds when drainRate = 1.")]
    public float maxBattery = 300f;

    [Header("Audio")]
    [Tooltip("Click sound when toggled.")]
    public AudioClip toggleSound;
    [Tooltip("AudioSource is created automatically if not set.")]
    public AudioSource audioSource;

    private Light flashlight;
    private bool isOn;
    private float batteryLevel = 1f; // 1 = full, 0 = empty

    private void Awake()
    {
        flashlight = GetComponent<Light>();
        flashlight.type = LightType.Spot;
        flashlight.spotAngle = 70f;
        flashlight.innerSpotAngle = 50f;
        flashlight.range = 25f;
        flashlight.intensity = 50f;
        flashlight.shadows = LightShadows.Soft;
        flashlight.enabled = startOn;
        isOn = startOn;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.spatialBlend = 1f;
                audioSource.rolloffMode = AudioRolloffMode.Linear;
                audioSource.maxDistance = 20f;
                audioSource.minDistance = 0.5f;
                audioSource.volume = 0.35f;
            }
        }
    }

    private void Update()
    {
        HandleInput();

        if (useBattery && isOn)
        {
            batteryLevel -= (drainRate / maxBattery) * Time.deltaTime;
            batteryLevel = Mathf.Clamp01(batteryLevel);

            // Gradually dim as battery drains
            flashlight.intensity = Mathf.Lerp(0f, 50f, batteryLevel);

            if (batteryLevel <= 0f)
                SetFlashlight(false);
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            SetFlashlight(!isOn);
        }
    }

    public void SetFlashlight(bool on)
    {
        isOn = on;
        flashlight.enabled = on;

        if (toggleSound && audioSource)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(toggleSound);
        }
    }

    public void RechargeBattery(float amount)
    {
        batteryLevel = Mathf.Clamp01(batteryLevel + amount);
    }

    public float GetBatteryLevel() => batteryLevel;
}
