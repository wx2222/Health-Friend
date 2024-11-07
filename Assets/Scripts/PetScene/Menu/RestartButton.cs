using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class RestartButton : MonoBehaviour, IPointerDownHandler
{
    public FullGrowthNotificationBox fullGrowthNotificationBox;
    public Button restartButton;

    private void Start()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(ShowConfirmationNotification);
        }
    
        
    }

    private void ShowConfirmationNotification()
    {
        string confirmationMessage = "Are you sure you want to restart your game?\n\nThis action cannot be undone!";
        fullGrowthNotificationBox.ShowNotification(confirmationMessage);
        fullGrowthNotificationBox.restartButton.onClick.RemoveAllListeners();
        fullGrowthNotificationBox.restartButton.onClick.AddListener(fullGrowthNotificationBox.PerformRestart);
    }

  

    public void OnPointerDown(PointerEventData eventData)
    {
    
        if (restartButton != null && eventData.pointerPress == restartButton.gameObject)
        {
            ShowConfirmationNotification();
        }

    }

}