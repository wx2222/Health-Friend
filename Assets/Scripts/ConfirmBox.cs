using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ConfirmBox : MonoBehaviour, IPointerDownHandler
{

    public Button resumeButton;
    public Button quitButton;
    public TextMeshProUGUI messageText;
    public AudioSource buttonSound;

   
    void Start()
    {

        buttonSound = GetComponent<AudioSource>();

        HideBox();
        

        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(HideBoxAndPlayButtonSound);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitAndPlayButtonSound);
        }
        
    }

    private void PlayButtonSound()
    {
        if (buttonSound != null)
        {
            buttonSound.Play();
        }
    }


    public void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
        }

        gameObject.SetActive(true);
        
    }

    public void HideBox()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator DelayedHideBox()
    {
        yield return new WaitForSeconds(buttonSound.clip.length); 

        HideBox();
    }


    public void HideBoxAndPlayButtonSound()
    {
        PlayButtonSound();
        StartCoroutine(DelayedHideBox());
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("PetScene");
    }

    private IEnumerator DelayedQuit()
    {
        yield return new WaitForSeconds(buttonSound.clip.length); 

        QuitGame();
    }

    public void QuitAndPlayButtonSound()
    {
        PlayButtonSound();
        StartCoroutine(DelayedQuit());
    }







    public void OnPointerDown(PointerEventData eventData)
    {
        if (resumeButton != null && eventData.pointerPress == resumeButton.gameObject)
        {
            HideBoxAndPlayButtonSound();
        }

        if (quitButton != null && eventData.pointerPress == quitButton.gameObject)
        {
            QuitAndPlayButtonSound();
        }
    }
}
