using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpeechBubble : MonoBehaviour, IPointerDownHandler
{
    public GameObject speechBubble;
    private float timer;

    private void Start()
    {
        int randomNumber = Random.Range(0, 3);

        if (randomNumber == 0)
        {
            speechBubble.SetActive(true);
        }
        else
        {
            speechBubble.SetActive(false);
        }
    }

    private void Update()
    {
        
        if (speechBubble.activeSelf)
        {
          
            timer += Time.deltaTime;

            // Check if the bubble should disappear after 3 minutes
            if (timer >= 180f)
            {
                HideSpeechBubble();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        if (speechBubble.activeSelf)
        {
            HideSpeechBubble();
        }
    }

    private void HideSpeechBubble()
    {
       
        speechBubble.SetActive(false);

       
        timer = 0f;
    }
}
