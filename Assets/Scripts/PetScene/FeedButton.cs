using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FeedButton : MonoBehaviour, IPointerDownHandler
{
    public int feedingCost = 10;  
    public Health health;  
    public CoinManager coinManager; 
    public NotificationBox notificationBox; 
    public AudioSource buttonSound;

    private void Start()
    {
        
        buttonSound = GetComponent<AudioSource>();
    }


    public void OnPointerDown(PointerEventData eventData)
    {

        buttonSound.Play();


       
        if (coinManager.CurrentCoins >= feedingCost)
        {
           
            coinManager.SubtractCoins(feedingCost);

            // Increase the health by 2 units
            health.ReplenishHealth(2);
        }
        else
        {
          
            notificationBox.ShowNotification("Not enough coins to feed!");
        }
    }

}