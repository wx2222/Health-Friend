using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;

public class MemoryGameController : MonoBehaviour, IPointerClickHandler
{
    public GameObject[] fruits;
    public List<Button> cards = new List<Button>();
    public List<GameObject> puzzles = new List<GameObject>();

    private bool firstGuess, secondGuess;
    private int guessCount;
    private int correctGuessCount;
    private int guess;
    private int firstGuessIndex, secondGuessIndex;
    private string firstGuessPuzzle, secondGuessPuzzle;
    private int score = 0;

    public float gameTime = 120f; // Total game time in seconds
    public TextMeshProUGUI timerText;
    private float remainingTime;

    public CorrectBox correctBox;
    public CoinManager coinManager;

    public AudioSource correctSound;

    void Awake()
    {
        fruits = Resources.LoadAll<GameObject>("Prefabs/Fruit");
    }

    void Start()
    {

        remainingTime = gameTime;
        UpdateTimerText();
        StartCoroutine(StartTimer());


        StartCoroutine(DelayedGetCards());
        AddPointerHandlers();
        AddPuzzles();
    }

    IEnumerator StartTimer()
    {
        while (remainingTime > 0f)
        {
            yield return new WaitForSeconds(1f);
            remainingTime--;
            UpdateTimerText();
        }

        GameOver();

    

    }

    private void PlayCorrectSound()
    {
        if (correctSound != null)
        {
            correctSound.Play();
        }
    }

    void GameOver()
    {
        if (score > 0)
        {
            
            Debug.Log("Game Over! Score: " + score);
            coinManager.AddCoins(score * 2);
          
        }

        if (correctBox != null)
        {
          
            string message = "Times up!\n\nYou have earned " + score * 2 + " coins!";
            correctBox.ShowMessage(message);

            if (score > 0) {
            PlayCorrectSound();
            }
            
        }
    }

    

    void UpdateTimerText()
    {
        timerText.text = Mathf.FloorToInt(remainingTime).ToString();
    }




    void AddPuzzles()
    {
        int totalPairs = 6;
        int fruitsCount = fruits.Length;
        List<int> indices = new List<int>();

        for (int i = 0; i < totalPairs; i++)
        {
            indices.Add(i);
            indices.Add(i);
        }

        Shuffle(indices);

        foreach (int index in indices)
        {
            puzzles.Add(fruits[index % fruitsCount]);
        }
    }

    void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    IEnumerator DelayedGetCards()
    {
        yield return new WaitForSeconds(0.1f);
        GetCards();
        AddPointerHandlers();
        StartCoroutine(ShowAllCardsRoutine(3f)); // Show all cards at the beginning for 3 seconds
    }

    void GetCards()
    {
        GameObject[] cardObjects = GameObject.FindGameObjectsWithTag("Card");

        for (int i = 0; i < cardObjects.Length; i++)
        {
            Button cardButton = cardObjects[i].GetComponent<Button>();
            cardButton.transition = Selectable.Transition.None;
            cards.Add(cardButton);
        }
    }

    void AddPointerHandlers()
    {
        foreach (Button card in cards)
        {
            card.onClick.AddListener(() => OnCardClicked(card));
            card.onClick.AddListener(OnCardPointerClick);
        }
    }

    void OnCardClicked(Button card)
    {
        GameObject cardObject = card.gameObject;
        int cardIndex = int.Parse(cardObject.name);

        if (!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = cardIndex;
            GameObject firstGuessPrefab = Instantiate(puzzles[firstGuessIndex], cardObject.transform);
            firstGuessPrefab.transform.localPosition = Vector3.zero;
            card.interactable = false;
        }
        else if (!secondGuess)
        {
            secondGuess = true;
            secondGuessIndex = cardIndex;
            GameObject secondGuessPrefab = Instantiate(puzzles[secondGuessIndex], cardObject.transform);
            secondGuessPrefab.transform.localPosition = Vector3.zero;
            StartCoroutine(CheckGuesses());
        }
    }

    IEnumerator CheckGuesses()
    {
        yield return new WaitForSeconds(0.5f);

        if (puzzles[firstGuessIndex].name == puzzles[secondGuessIndex].name)
        {
            correctGuessCount++;
        }
        else
        {
            cards[firstGuessIndex].interactable = true;
            cards[secondGuessIndex].interactable = true;
            yield return new WaitForSeconds(0.5f);
            Destroy(cards[firstGuessIndex].transform.GetChild(0).gameObject);
            Destroy(cards[secondGuessIndex].transform.GetChild(0).gameObject);
        }

        firstGuess = false;
        secondGuess = false;
        firstGuessIndex = -1;
        secondGuessIndex = -1;

        if (correctGuessCount == cards.Count / 2)
        {
            score++;
            Debug.Log("+1 Score");
            ResetGame();
        }
    }

    void ResetGame()
    {
        firstGuess = false;
        secondGuess = false;
        guessCount = 0;
        correctGuessCount = 0;

        SetCardInteractability(false); 

        foreach (Button card in cards)
        {
            card.interactable = true;
            if (card.transform.childCount > 0)
                Destroy(card.transform.GetChild(0).gameObject);
        }

        puzzles.Clear();
        AddPuzzles();

        StartCoroutine(ShowAllCardsRoutine(3f)); // Show all cards at the beginning of each round for 3 seconds
    }

    IEnumerator ShowAllCardsRoutine(float duration)
    {
        SetCardInteractability(false);

        ShowAllCards();

        yield return new WaitForSeconds(duration);

        HideAllCards();
        SetCardInteractability(true); 
    }

    void ShowAllCards()
    {
        foreach (Button card in cards)
        {
            if (card.transform.childCount > 0)
                Destroy(card.transform.GetChild(0).gameObject);
            int cardIndex = int.Parse(card.gameObject.name);
            GameObject prefab = Instantiate(puzzles[cardIndex], card.gameObject.transform);
            prefab.transform.localPosition = Vector3.zero;
        }
    }

    void HideAllCards()
    {
        foreach (Button card in cards)
        {
            if (card.transform.childCount > 0)
                Destroy(card.transform.GetChild(0).gameObject);
        }
    }

    void SetCardInteractability(bool interactable)
    {
        foreach (Button card in cards)
        {
            card.interactable = interactable;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Button clickedCard = eventData.pointerPress.GetComponent<Button>();
        if (clickedCard != null && cards.Contains(clickedCard))
        {
            OnCardClicked(clickedCard);
        }
    }

    void OnCardPointerClick()
    {
        Debug.Log("Card Pointer Clicked");
    }
}
