using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Settings : MonoBehaviour, IPointerDownHandler
{
    public Button closeButton;
    public AudioSource buttonSound;

    private void Start()
    {
        buttonSound = GetComponent<AudioSource>();
        HideSettings();

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HideSettingsAndPlayButtonSound);
            
        }
    }

    public void ShowSettings()
    {
        gameObject.SetActive(true);
    }

    public void HideSettings()
    {
        gameObject.SetActive(false);
    }

    private void PlayButtonSound()
    {
        if (buttonSound != null)
        {
            buttonSound.Play();
        }
    }

    private IEnumerator DelayedHideSettings()
    {
        yield return new WaitForSeconds(buttonSound.clip.length); 

        HideSettings();
    }

    public void HideSettingsAndPlayButtonSound()
    {
        PlayButtonSound();
        StartCoroutine(DelayedHideSettings());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (closeButton != null && eventData.pointerPress == closeButton.gameObject)
        {
            HideSettingsAndPlayButtonSound();
            
        }
    }


}