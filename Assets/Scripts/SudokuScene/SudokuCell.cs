using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SudokuCell : MonoBehaviour, IPointerClickHandler
{
    public GameObject numberTextPrefab; 

    private TextMeshProUGUI numberText;

    private bool isGenerated;

    private void Awake()
    {
        GameObject numberTextObject = Instantiate(numberTextPrefab, transform);
        numberTextObject.transform.SetParent(transform, false);

        Canvas numberTextCanvas = numberTextObject.GetComponent<Canvas>();
        if (numberTextCanvas == null)
        {
            numberTextCanvas = numberTextObject.AddComponent<Canvas>();
        }
        numberTextCanvas.overrideSorting = true;
        numberTextCanvas.sortingOrder = 1;

        RectTransform numberTextRectTransform = numberTextObject.GetComponent<RectTransform>();
        numberTextRectTransform.pivot = new Vector2(0.5f, 0.5f);
        numberTextRectTransform.anchorMin = new Vector2(0f, 0f);
        numberTextRectTransform.anchorMax = new Vector2(1f, 1f);
        numberTextRectTransform.anchoredPosition = Vector2.zero;

        numberText = numberTextObject.GetComponent<TextMeshProUGUI>();

        numberTextRectTransform.localPosition = Vector3.zero;
        numberTextRectTransform.offsetMin = Vector2.zero;
        numberTextRectTransform.offsetMax = Vector2.zero;
    }

public void SetCellNumber(int number, bool generated)
{
    isGenerated = generated;

    if (number != 0)
    {
        numberText.text = number.ToString();

        if (isGenerated)
        {
            numberText.color = Color.black; // Set the text color to black for generated numbers
        }
        else
        {
            numberText.color = Color.blue; // Set the text color to blue for filled numbers
            numberText.canvas.sortingOrder = 2;
            numberText.fontSize = 80;
        }
    }
    else
    {
        numberText.text = string.Empty; 
        numberText.color = Color.black; 
        numberText.canvas.sortingOrder = 1;
    }

    // Enable or disable the SudokuCell component based on whether the number is generated
    if (isGenerated)
    {
        enabled = false;
    }
    else
    {
        enabled = true;
    }
}
    public bool IsFilled()
    {
        return !string.IsNullOrEmpty(numberText.text);
    }

    public int GetCellNumber()
    {
        int number = 0;
        if (int.TryParse(numberText.text, out number))
        {
            return number;
        }
        return number;
    }


    public void ClearCellNumber()
    {
        numberText.text = string.Empty;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int storedNumber = NumberButtonHighlighter.Instance.GetStoredNumber();

        if (isGenerated)
        {
            Debug.Log("Cannot modify generated number.");
            return;
        }

        if (storedNumber == -1)
        {
            Debug.Log("No number selected.");
            return;
        }

        SetCellNumber(storedNumber, isGenerated);
    }
}
