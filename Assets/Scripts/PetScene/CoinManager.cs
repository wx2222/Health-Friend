using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CoinManager : MonoBehaviour
{
    public int startingCoins = 0;  
    public TextMeshProUGUI coinsText;  
    public TextMeshProUGUI animationText;  

    public Health petHealth;

    public float fadeDuration = 1f;

    private int currentCoins; 
    public int CurrentCoins => currentCoins;

    private const string CoinsKey = "PlayerCoins";

    private void Start()
    {
        LoadCoinsFromPlayerPrefs();
        UpdateCoinsText();
        animationText.gameObject.SetActive(false);
    }

    private void LoadCoinsFromPlayerPrefs()
    {
        if (PlayerPrefs.HasKey(CoinsKey))
        {
            currentCoins = PlayerPrefs.GetInt(CoinsKey);
        }
        else
        {
            currentCoins = startingCoins;
            SaveCoinsToPlayerPrefs();
        }
    }

    private void SaveCoinsToPlayerPrefs()
    {
        PlayerPrefs.SetInt(CoinsKey, currentCoins);
        PlayerPrefs.Save();
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        currentCoins = Mathf.Clamp(currentCoins, 0, 999);
        UpdateCoinsText();
        SaveCoinsToPlayerPrefs();
    }

    public void SubtractCoins(int amount)
    {
        if (petHealth.CurrentHealth >= petHealth.maxHealth) 
            return;

        currentCoins -= amount;
        currentCoins = Mathf.Clamp(currentCoins, 0, 999);
        UpdateCoinsText();
        SaveCoinsToPlayerPrefs();

        StartCoroutine(ShowSubtractionAnimation("-" + amount.ToString()));
    }

    private void UpdateCoinsText()
    {
        coinsText.text = currentCoins.ToString();
    }

    private IEnumerator ShowSubtractionAnimation(string animationText)
    {
        this.animationText.text = animationText;
        this.animationText.gameObject.SetActive(true);

        
        this.animationText.color = Color.white;

        float elapsedTime = 0f;
        Color initialColor = this.animationText.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            this.animationText.color = Color.Lerp(initialColor, targetColor, t);
            yield return null;
        }

        this.animationText.gameObject.SetActive(false);
    }
}