using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour, IPointerDownHandler
{

    public Button closeButton;
    public AudioSource buttonSound;

    public Button sudokuButton;
    public Button fastCalcButton;
    public Button memoryButton;

    public Growth growth;
    public Health health;
    

    private void Start()
    {
        buttonSound = GetComponent<AudioSource>();
        HideMenu();

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HideMenuAndPlayButtonSound);
            
        }

        if (sudokuButton != null)
        {
            sudokuButton.onClick.AddListener(OnSudokuButtonClick);
        }

        if (fastCalcButton != null)
        {
            fastCalcButton.onClick.AddListener(OnFastCalcButtonClick);
        }

        if (memoryButton != null)
        {
            memoryButton.onClick.AddListener(OnMemoryButtonClick);
        }
    }


    public void ShowMenu()
    {
        gameObject.SetActive(true);
    }

    public void HideMenu()
    {
        gameObject.SetActive(false);
    }

    private void PlayButtonSound()
    {
        if (buttonSound != null)
        {
            buttonSound.Play();
        }
    }

    public void HideMenuAndPlayButtonSound()
    {
        PlayButtonSound();
        StartCoroutine(DelayedHideMenu());
    }


    private IEnumerator DelayedHideMenu()
    {
        yield return new WaitForSeconds(buttonSound.clip.length);

        HideMenu();
    }

    private IEnumerator DelayedSudoku()
    {
        yield return new WaitForSeconds(buttonSound.clip.length); 
        SceneManager.LoadScene("SudokuScene");

        
    }

    public void SwitchToSudokuScene()
    {
        PlayButtonSound();
        growth.SaveGrowthData();
        health.SaveHealthToPlayerPrefs();
        StartCoroutine(DelayedSudoku());
    }

    public void OnSudokuButtonClick()
    {
        SwitchToSudokuScene();
        
    }


    

 

    private IEnumerator DelayedCalc()
    {
        yield return new WaitForSeconds(buttonSound.clip.length); 
        SceneManager.LoadScene("CalcScene");

        
    }

    public void SwitchToCalcScene()
    {
        PlayButtonSound();
        StartCoroutine(DelayedCalc());
    }



    public void OnFastCalcButtonClick()
    {
        SwitchToCalcScene();
    }


    private IEnumerator DelayedMemory()
    {
        yield return new WaitForSeconds(buttonSound.clip.length); 
        SceneManager.LoadScene("MemoryScene");

        
    }

    public void SwitchToMemoryScene()
    {
        PlayButtonSound();
        growth.SaveGrowthData();
        health.SaveHealthToPlayerPrefs();
        StartCoroutine(DelayedMemory());
    }


    public void OnMemoryButtonClick()
    {
        SwitchToMemoryScene();
    }


    public void OnPointerDown(PointerEventData eventData)
    {

        if (sudokuButton != null && eventData.pointerPress == sudokuButton.gameObject)
        {
            OnSudokuButtonClick();
        }

        if (fastCalcButton != null && eventData.pointerPress == fastCalcButton.gameObject)
        {
            OnFastCalcButtonClick(); 
        }

        if (memoryButton != null && eventData.pointerPress == memoryButton.gameObject)
        {
            OnMemoryButtonClick(); 
        }
        

       if (closeButton != null && eventData.pointerPress == closeButton.gameObject)
        {
            HideMenuAndPlayButtonSound(); 
        }
    }
}