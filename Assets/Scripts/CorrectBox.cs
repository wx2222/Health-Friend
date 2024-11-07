using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CorrectBox : MonoBehaviour, IPointerDownHandler
{

    public Button restartButton;
    public Button quitButton;
    public TextMeshProUGUI messageText;
    public AudioSource buttonSound;

   
    void Start()
    {

        buttonSound = GetComponent<AudioSource>();

        HideBox();
        

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartAndPlayButtonSound);
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

    private IEnumerator DelayedRestartScene()
    {
        yield return new WaitForSeconds(buttonSound.clip.length); 

        RestartScene();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void RestartAndPlayButtonSound()
    {
        PlayButtonSound();
        StartCoroutine(DelayedRestartScene());
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
        if (restartButton != null && eventData.pointerPress == restartButton.gameObject)
        {
            RestartAndPlayButtonSound();
        }

        if (quitButton != null && eventData.pointerPress == quitButton.gameObject)
        {
            QuitAndPlayButtonSound();
        }
    }
}
