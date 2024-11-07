using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FullGrowthNotificationBox : MonoBehaviour, IPointerDownHandler
{
    public Button resumeButton;
    public Button restartButton;
    public TextMeshProUGUI notificationText;
    public NotificationBox notificationBox;

    public AudioSource buttonSound;

    private void Start()
    {
        buttonSound = GetComponent<AudioSource>();

        HideNotification();
        //messageText = transform.Find("NotificationText").GetComponent<TextMeshProUGUI>();

        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(Resume);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(Restart);
        }
    }

    public void ShowNotification(string message = "Your pet is now fully grown!\n\nWould you like to restart with a new pet or resume?")
    {
        if (notificationText != null)
        {
            notificationText.text = message;
        }

        gameObject.SetActive(true);
    }

    public void HideNotification()
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

    private IEnumerator DelayedHideNotification()
    {
        yield return new WaitForSeconds(buttonSound.clip.length);

        HideNotification();
    }


    public void Resume()
    {
        PlayButtonSound();
        notificationBox.ShowNotification("You can restart your progress in the settings.");
        StartCoroutine(DelayedHideNotification());

    }

    public void Restart()
    {
        PlayButtonSound();
        ShowConfirmationNotification();
    }

    private void ShowConfirmationNotification()
    {
        string confirmationMessage = "Are you sure you want to restart your game?\n\nThis action cannot be undone!";
        notificationText.text = confirmationMessage;
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(PerformRestart);
        restartButton.gameObject.SetActive(true);
    }

    

    public void PerformRestart()
    {
        PlayButtonSound();
        StartCoroutine(DelayedRestart());
    }

    private IEnumerator DelayedRestart()
    {
        yield return new WaitForSeconds(buttonSound.clip.length); 
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }


        

    public void OnPointerDown(PointerEventData eventData)
    {
        if (resumeButton != null && eventData.pointerPress == resumeButton.gameObject)
        {
            Resume();
        }
        else if (restartButton != null && eventData.pointerPress == restartButton.gameObject)
        {
            Restart();
        }

    }
}