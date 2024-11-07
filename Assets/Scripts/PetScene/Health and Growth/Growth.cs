using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growth : MonoBehaviour
{

   

    public int maxGrowth = 900;
    public int currentGrowth;

    public GrowthBar growthBar;

    private float elapsedTime = 0f;

    // Increase growth bar by a certain amount in a day
    private const float increaseInterval = 86400f;

    // Unit increased per day
    private const int dailyGrowth = 6;


    private const string GrowthKey = "PetGrowth";
    private const string LastSaveTimeKey = "LastSaveTime";
    private DateTime lastSaveTime;

    private List<int> growthSegmentBoundaries;

    private Health health;

    public FullGrowthNotificationBox fullGrowthNotificationBox;
    private bool isFullGrowthNotified = false;



    private void Awake()
    {
        InitializeGrowthSegmentBoundaries();

        if (PlayerPrefs.HasKey(GrowthKey))
        {
            currentGrowth = PlayerPrefs.GetInt(GrowthKey);
        }
        else
        {
            currentGrowth = 0;
        }

        growthBar.SetMaxGrowth(maxGrowth);
        growthBar.SetGrowth(currentGrowth);

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

        health = GetComponent<Health>();
    }

    private void InitializeGrowthSegmentBoundaries()
    {
        growthSegmentBoundaries = new List<int>();

        int segmentCount = 7;
        int segmentGrowth = maxGrowth / segmentCount;

        for (int i = 1; i <= segmentCount; i++)
        {
            int boundary = segmentGrowth * i;
            growthSegmentBoundaries.Add(boundary);
        }
    }

    public int MarkGrowthSegment(int growth)
    {
        int segment = -1;

        for (int i = 0; i < growthSegmentBoundaries.Count; i++)
        {
            if (growth <= growthSegmentBoundaries[i])
            {
                segment = i + 1;
                break;
            }
        }

        return segment;
    }

    private int GetDailyGrowth(int segment)
    {
        switch (segment)
        {
            case 1:
                return 0;
            case 2:
                return 1;
            case 3:
                return 2;
            case 4:
                return 3;
            case 5:
                return 4;
            case 6:
                return 5;
            case 7:
            default:
                return 6;
        }
    }

    private void OnApplicationQuit()
    {

        SaveGrowthData();

       

    }

    public void SaveGrowthData()
    {
        PlayerPrefs.SetInt(GrowthKey, currentGrowth);
        PlayerPrefs.SetString(LastSaveTimeKey, lastSaveTime.Ticks.ToString());
        PlayerPrefs.Save();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= increaseInterval)
        {
            int currentHealthSegment = health.MarkHealthSegment(health.CurrentHealth);
            int dailyGrowth = GetDailyGrowth(currentHealthSegment);

            currentGrowth += dailyGrowth;
            if (currentGrowth > maxGrowth && !isFullGrowthNotified)
            {
                currentGrowth = maxGrowth;

                if (!PlayerPrefs.HasKey("FullGrowthNotificationShown"))
                {
                    fullGrowthNotificationBox.ShowNotification();
                    PlayerPrefs.SetInt("FullGrowthNotificationShown", 1);
                }
            
                isFullGrowthNotified = true;
            }

            if (currentGrowth > maxGrowth)
            {
                currentGrowth = maxGrowth;
            }


            growthBar.SetGrowth(currentGrowth);

            elapsedTime = 0f;
        }
    }
}