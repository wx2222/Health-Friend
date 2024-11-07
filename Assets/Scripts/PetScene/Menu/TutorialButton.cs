using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialButton : MonoBehaviour, IPointerDownHandler
{
    public GameObject tutorial;
    public AudioSource buttonSound;

    private void Start()
    {
        // Get the AudioSource component attached to the same GameObject
        buttonSound = GetComponent<AudioSource>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonSound.Play();
        tutorial.SetActive(true);
    }
}