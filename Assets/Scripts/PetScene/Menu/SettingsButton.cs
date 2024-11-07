using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsButton : MonoBehaviour, IPointerDownHandler
{
    public GameObject settings;
    public AudioSource buttonSound;

    private void Start()
    {
        // Get the AudioSource component attached to the same GameObject
        buttonSound = GetComponent<AudioSource>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonSound.Play();
        settings.SetActive(true);
    }
}