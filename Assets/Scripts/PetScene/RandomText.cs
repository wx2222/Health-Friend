using UnityEngine;
using TMPro;

public class RandomText : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
   
    

    public string[] lines = {
    "Hello, how are you today?",
    "Remember to drink more water!",
    "Take walks and enjoy some fresh air.",
    "Take a break and do some stretches.",
    "Remember to take your medication!",
    "Hey there! You are doing great!",
    "Stay healthy, get regular check-ups!",
    "Exercising regularly can help you stay healthy.",
    "It's good to see you!",
    "Puzzles are good for your mind.",
    "Make sure to get enough sleep!",
    "Don't eat to much junk food!",
    "My appearance may change as I grow bigger...",
    "If you are stressed, try meditating.",
    "It's good to engage in social activities.",
    "Avoid caffeine late in the day.",
    "Listening to calming music can make you feel relaxed.",
    "Have you considered practicing yoga?",
    "Season your food with herbs to reduce sodium intake."
    };

    private void Start()
    {
       
        if (lines.Length > 0)
        {
           
            int randomIndex = Random.Range(0, lines.Length);

            // Set the randomly selected sentence as the text
            textMeshPro.text = lines[randomIndex];
        }
        else
        {
            Debug.LogWarning("No sentences available.");
        }
    }
}
