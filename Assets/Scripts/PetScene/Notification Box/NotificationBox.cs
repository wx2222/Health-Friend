using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class NotificationBox : MonoBehaviour, IPointerDownHandler
{
    public Button okButton;
    public TextMeshProUGUI notificationText;
    public AudioSource buttonSound;
    
    

    private void Start()
    {
        buttonSound = GetComponent<AudioSource>();

        HideNotification();
        //messageText = transform.Find("NotificationText").GetComponent<TextMeshProUGUI>();

        if (okButton != null)
        {
            okButton.onClick.AddListener(HideNotificationAndPlayButtonSound);
        }
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



    public void ShowNotification(string message)
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

    public void HideNotificationAndPlayButtonSound()
    {
        PlayButtonSound();
        StartCoroutine(DelayedHideNotification());
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (okButton != null && eventData.pointerPress == okButton.gameObject)
        {
            HideNotificationAndPlayButtonSound();
        }
    }
}