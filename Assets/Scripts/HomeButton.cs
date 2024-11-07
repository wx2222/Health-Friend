using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class HomeButton : MonoBehaviour, IPointerDownHandler
{
    public AudioSource buttonSound;
    public ConfirmBox confirmBox; 

    private void Start()
    {
        buttonSound = GetComponent<AudioSource>();
    }

    private void PlayButtonSound()
    {
        if (buttonSound != null)
        {
            buttonSound.Play();
        }
    }

    private void Home()
    {
       
        // Show the confirm box with the appropriate message
        if (confirmBox != null)
        {
            confirmBox.ShowMessage("Are you sure you want to quit this game?");
        }
    }



    public void OnPointerDown(PointerEventData eventData)
    {

        PlayButtonSound();
        Home();
    }
}
