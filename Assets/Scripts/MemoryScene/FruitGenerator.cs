using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FruitGenerator : MonoBehaviour
{
    public GameObject[] fruitPrefabs; 
    public Transform cardField;
    public int numCards = 12;

    private void Start()
    {
        GenerateFruits();
    }

    

private void GenerateFruits()
{
    
    List<int> fruitIndices = new List<int>();

    
    for (int i = 0; i < fruitPrefabs.Length; i++)
    {
        fruitIndices.Add(i);
    }

   
    Shuffle(fruitIndices);

    // Instantiate the fruits and set their positions
    GridLayoutGroup gridLayout = cardField.GetComponent<GridLayoutGroup>();
    for (int i = 0; i < numCards; i++)
    {
        if (i >= fruitPrefabs.Length)
        {
            Debug.LogError("Not enough fruit prefabs to match the number of cards.");
            return;
        }

        int fruitIndex = fruitIndices[i];
        GameObject fruitPrefab = fruitPrefabs[fruitIndex];

        
        GameObject fruit = Instantiate(fruitPrefab, cardField);

        
        float offsetX = Random.Range(-gridLayout.cellSize.x / 2f, gridLayout.cellSize.x / 2f);
        float offsetY = Random.Range(-gridLayout.cellSize.y / 2f, gridLayout.cellSize.y / 2f);
        fruit.transform.localPosition = new Vector3(offsetX, offsetY, 0f);
    }
}

private void Shuffle(List<int> list)
{
    int count = list.Count;
    for (int i = 0; i < count - 1; i++)
    {
        int randomIndex = Random.Range(i, count);
        int temp = list[i];
        list[i] = list[randomIndex];
        list[randomIndex] = temp;
    }
}

}
