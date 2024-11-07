using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class BrightnessSlider : MonoBehaviour
{
    public Slider brightnessSlider;
    public TextMeshProUGUI brightnessNumberText;
    public PostProcessVolume postProcessVolume;
    public PostProcessLayer postProcessLayer;
    public Image brightnessLayer;

    public float minAlpha = 0f;
    public float maxAlpha = 0.5f;

    private RectTransform knobRectTransform;
    private RectTransform textRectTransform;
    AutoExposure exposure;

    private const string BrightnessValueKey = "BrightnessValue";

    private void Start()
    {
        knobRectTransform = brightnessSlider.handleRect.GetComponent<RectTransform>();
        textRectTransform = brightnessNumberText.GetComponent<RectTransform>();

        brightnessSlider.onValueChanged.AddListener(UpdateBrightness);

        
        postProcessVolume.profile.TryGetSettings(out exposure);

        // Load the brightness value from PlayerPrefs
        if (PlayerPrefs.HasKey(BrightnessValueKey))
        {
            float savedBrightness = PlayerPrefs.GetFloat(BrightnessValueKey);
            brightnessSlider.value = savedBrightness;
        }
        else
        {
            // Set default brightness value if not found in PlayerPrefs
            brightnessSlider.value = brightnessSlider.maxValue;
        }

        // Update the brightness based on the initial value
        UpdateBrightness(brightnessSlider.value);
    }

    private void UpdateBrightness(float value)
    {
        // Update the brightness number text based on the slider value
        brightnessNumberText.text = Mathf.RoundToInt(value).ToString();

        
        float normalizedBrightness = Remap(value, 0f, brightnessSlider.maxValue, 0.5f, 1.5f);

       
        exposure.keyValue.Override(normalizedBrightness);

        
        float alpha = Mathf.Lerp(maxAlpha, minAlpha, value / brightnessSlider.maxValue);

        
        Color layerColor = brightnessLayer.color;
        layerColor.a = alpha;
        brightnessLayer.color = layerColor;

        // Save the brightness value to PlayerPrefs
        PlayerPrefs.SetFloat(BrightnessValueKey, value);
        PlayerPrefs.Save();
    }

    private float Remap(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return Mathf.Lerp(outputMin, outputMax, Mathf.InverseLerp(inputMin, inputMax, value));
    }
}
