using UnityEngine;

public class Card : MonoBehaviour
{
    public GameObject cardCover; 
    public GameObject fruitObject;
    
    public bool IsFlipped { get; private set; }

    private void Start()
    {
        FlipCard(false); // Initially hide the fruit by flipping the card
    }

    public void FlipCard(bool showFruit)
    {
        fruitObject.SetActive(showFruit);
        cardCover.SetActive(!showFruit);
        IsFlipped = showFruit;
    }
}
