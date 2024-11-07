using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameMenuButton : MonoBehaviour, IPointerDownHandler
{
    public GameObject gameMenu;
    public AudioSource buttonSound;

    private void Start()
    {
       
        buttonSound = GetComponent<AudioSource>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        buttonSound.Play();
        gameMenu.SetActive(true);
    }
}