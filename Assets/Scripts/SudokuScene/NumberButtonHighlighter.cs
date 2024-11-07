using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NumberButtonHighlighter : MonoBehaviour, IPointerClickHandler
{
    public Color highlightColor = new Color(0, 0, 1, 1); 
    public static NumberButtonHighlighter Instance { get; private set; }

    private Button[] numberButtons;
    private Button currentButton;
    private int storedNumber = -1;
    private Color normalColor;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Get all the child buttons
        numberButtons = GetComponentsInChildren<Button>();

       
        normalColor = numberButtons[0].image.color;

       
        foreach (Button button in numberButtons)
        {
            button.onClick.AddListener(() => OnNumberButtonClick(button));
        }
    }

    private void OnNumberButtonClick(Button clickedButton)
    {
        if (currentButton == clickedButton)
        {
           
            DeselectButton();
        }
        else
        {
           
            DeselectButton();

         
            currentButton = clickedButton;
            currentButton.image.color = highlightColor;

            storedNumber = ExtractNumber(currentButton);
        }

        Debug.Log("Stored Number: " + storedNumber);
    }

    private void DeselectButton()
    {
        if (currentButton != null)
        {
            currentButton.image.color = normalColor;
            currentButton = null;
            storedNumber = -1;
        }
    }

    private int ExtractNumber(Button button)
    {
        
        string buttonName = button.name;

       
        string[] parts = buttonName.Split(' ');
        if (parts.Length >= 2 && int.TryParse(parts[0], out int number))
        {
            return number;
        }

        
        Text buttonText = button.GetComponentInChildren<Text>();
        if (buttonText != null && int.TryParse(buttonText.text, out int numberFromText))
        {
            return numberFromText;
        }

        return -1;
    }

    public bool HasStoredNumber(int number)
    {
        return storedNumber == number;
    }

    public int GetStoredNumber()
    {
        return storedNumber;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       
    }
}
