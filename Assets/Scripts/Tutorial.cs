using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{

    public Button closeButton;

    public AudioSource buttonSound;


   
    void Start()
    {

        buttonSound = GetComponent<AudioSource>();

        HideTutorial();

        
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(Close);
        }

        
    }

    public void HideTutorial()
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


    private IEnumerator DelayedHideTutorial()
    {
        yield return new WaitForSeconds(buttonSound.clip.length); 

        HideTutorial();
    }

    public void Close()
    {
        PlayButtonSound();
        StartCoroutine(DelayedHideTutorial());

    }



    public void OnPointerDown(PointerEventData eventData)
    {
        if (closeButton != null && eventData.pointerPress == closeButton.gameObject)
        {
            Close();
        }


    }
}
