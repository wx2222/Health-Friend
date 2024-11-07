using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSlider : MonoBehaviour
{
    public Slider volumeSlider;
    public TextMeshProUGUI volumeNumberText;

    private const string VolumeKey = "Volume";

    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(UpdateVolume);

        // Load the volume value from PlayerPrefs
        if (PlayerPrefs.HasKey(VolumeKey))
        {
            float savedVolume = PlayerPrefs.GetFloat(VolumeKey);
            volumeSlider.value = savedVolume;
        }
        else
        {
            
            volumeSlider.value = 100.0f;
        }

        UpdateVolume(volumeSlider.value);
    }

    private void UpdateVolume(float value)
    {
        // Update the volume number text based on the slider value
        volumeNumberText.text = Mathf.RoundToInt(value).ToString();

        AudioListener.volume = value / 100;

        // Save the volume value to PlayerPrefs
        PlayerPrefs.SetFloat(VolumeKey, value);
        PlayerPrefs.Save();
    }
}