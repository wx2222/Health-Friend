
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 1680;
    public int currentHealth;

    public HealthBar healthBar;

    private float depletionInterval = 3600f;
    private float elapsedTime = 0f;

    private const string HealthKey = "PetHealth";
    private const string LastSaveTimeKey = "LastSaveTime";
    private DateTime lastSaveTime;

    private List<int> healthSegmentBoundaries;

    public NotificationBox notificationBox;

   void Start()
    {
        InitializeHealthSegmentBoundaries();

        if (PlayerPrefs.HasKey(HealthKey))
        {
            currentHealth = PlayerPrefs.GetInt(HealthKey);
        }
        else
        {
            currentHealth = maxHealth;
            //currentHealth = maxHealth * 3 / 4;
        }

        if (PlayerPrefs.HasKey(LastSaveTimeKey))
        {
            long ticks;
            if (long.TryParse(PlayerPrefs.GetString(LastSaveTimeKey), out ticks))
            {
                lastSaveTime = new DateTime(ticks);
            }
            else
            {
                lastSaveTime = DateTime.Now;
            }
        }
        else
        {
            lastSaveTime = DateTime.Now;
        }

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);

        StartCoroutine(DepleteHealth());
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    private void InitializeHealthSegmentBoundaries()
    {
        healthSegmentBoundaries = new List<int>();

        int segmentCount = 7;
        int segmentHealth = maxHealth / segmentCount;

        for (int i = 1; i <= segmentCount; i++)
        {
            int boundary = segmentHealth * i;
            healthSegmentBoundaries.Add(boundary);
        }
    }

    public int MarkHealthSegment(int health)
    {
        int segment = -1;

        for (int i = 0; i < healthSegmentBoundaries.Count; i++)
        {
            if (health <= healthSegmentBoundaries[i])
            {
                segment = i + 1;
                break;
            }
        }

        return segment;
    }

    public void SaveHealthToPlayerPrefs()
    {
        PlayerPrefs.SetInt(HealthKey, currentHealth);
        PlayerPrefs.SetString(LastSaveTimeKey, DateTime.Now.Ticks.ToString());
        PlayerPrefs.Save();
    }

    public void ReplenishHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            notificationBox.ShowNotification("Pet is not hungry right now!");
        }

        healthBar.SetHealth(currentHealth);
        SaveHealthToPlayerPrefs(); 
    }

    private IEnumerator DepleteHealth()
    {
        TimeSpan timeSinceLastSave = DateTime.Now - lastSaveTime;
        float depletionCount = (float)timeSinceLastSave.TotalSeconds / depletionInterval;
        int unitsToDeplete = Mathf.RoundToInt(depletionCount) * 10;

        if (unitsToDeplete > 0)
        {
            currentHealth -= unitsToDeplete;
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
            healthBar.SetHealth(currentHealth);

            int currentSegment = MarkHealthSegment(currentHealth);

            if (currentHealth <= 0)
            {
                Debug.Log("Pet's health has depleted.");
            }
        }

        yield return new WaitForSeconds(depletionInterval);

        while (currentHealth > 0)
        {
            currentHealth -= 10; // Reduce the health by 10 units per interval
            healthBar.SetHealth(currentHealth);

            int currentSegment = MarkHealthSegment(currentHealth);

            if (currentHealth <= 0)
            {
                Debug.Log("Pet's health has depleted.");
            }

            SaveHealthToPlayerPrefs(); 

            yield return new WaitForSeconds(depletionInterval);
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 10f)
        {
            elapsedTime = 0f;
        }
    }
}
