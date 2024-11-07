using UnityEngine;
using UnityEngine.UI;

public class CardGenerator : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform cardField;
    public int numCards = 12;

    private void Start()
    {
        GenerateCards();
    }

    private void GenerateCards()
    {
        for (int i = 0; i < numCards; i++)
        {
            // Instantiate a new card from the prefab
            GameObject card = Instantiate(cardPrefab, cardField);

            // Set the card's position in the grid layout
            GridLayoutGroup gridLayout = cardField.GetComponent<GridLayoutGroup>();
            float cardWidth = card.GetComponent<RectTransform>().rect.width;
            float cardHeight = card.GetComponent<RectTransform>().rect.height;
            card.transform.localPosition = new Vector3((i % 2) * (gridLayout.cellSize.x + gridLayout.spacing.x),
                                                       -(i / 2) * (gridLayout.cellSize.y + gridLayout.spacing.y),
                                                       0f);

            // Set the card's name with the number
            card.name = "" + i;
        }
    }
}
