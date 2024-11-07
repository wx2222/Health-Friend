using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PetTutorial : MonoBehaviour
{

    public Button closeButton;

    public AudioSource buttonSound;

    private const string TutorialShownKey = "TutorialShown";


 
    void Start()
    {

        buttonSound = GetComponent<AudioSource>();

       

        
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(Close);
        }


        //Tutorial pop up the first time the player opens the app
        if (!PlayerPrefs.HasKey(TutorialShownKey))
        {
            ShowTutorial();
            PlayerPrefs.SetInt(TutorialShownKey, 1);
        }
        else
        {
            HideTutorial();
        }



        
    }

    public void ShowTutorial()
    {
        gameObject.SetActive(true);
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
