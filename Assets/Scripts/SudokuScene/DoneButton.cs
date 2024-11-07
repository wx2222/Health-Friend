using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DoneButton : MonoBehaviour, IPointerDownHandler
{
    public AudioSource buttonSound;
    public ConfirmBox confirmBox;
    public CorrectBox correctBox;
    public CoinManager coinManager;

    public AudioSource correctSound;
    
    

    private void Start()
    {
        buttonSound = GetComponent<AudioSource>();
       
        
       
    }

    private void PlayButtonSound()
    {
        if (buttonSound != null)
        {
            buttonSound.Play();
        }
    }

    private void PlayCorrectSound()
    {
        if (correctSound != null)
        {
            correctSound.Play();
        }
    }


    private bool CheckSudoku()
    {
        // Get all SudokuCell components in the scene
        SudokuCell[] sudokuCells = FindObjectsOfType<SudokuCell>();

        // Create a 2D array to store the Sudoku grid
        int[,] grid = new int[9, 9];

        // Fill the grid with cell numbers
        for (int i = 0; i < sudokuCells.Length; i++)
        {
            SudokuCell cell = sudokuCells[i];
            int row = i / 9;
            int col = i % 9;

            if (cell.IsFilled())
            {
                int number = cell.GetCellNumber();
                grid[row, col] = number;
            }
            else
            {
                // If any cell is not filled, the Sudoku is incorrect
                return false;
            }
        }

        // Check rows
        for (int row = 0; row < 9; row++)
        {
            HashSet<int> rowSet = new HashSet<int>();
            for (int col = 0; col < 9; col++)
            {
                int number = grid[row, col];
                if (number != 0)
                {
                    if (rowSet.Contains(number))
                    {
                        // Duplicate number found in the row
                        return false;
                    }
                    rowSet.Add(number);
                }
                else
                {
                    // If any cell is empty, the Sudoku is incorrect
                    return false;
                }
            }
        }

        // Check columns
        for (int col = 0; col < 9; col++)
        {
            HashSet<int> colSet = new HashSet<int>();
            for (int row = 0; row < 9; row++)
            {
                int number = grid[row, col];
                if (number != 0)
                {
                    if (colSet.Contains(number))
                    {
                        // Duplicate number found in the column
                        return false;
                    }
                    colSet.Add(number);
                }
                else
                {
                    // If any cell is empty, the Sudoku is incorrect
                    return false;
                }
            }
        }

        // Check 3x3 grids
        for (int rowOffset = 0; rowOffset < 9; rowOffset += 3)
        {
            for (int colOffset = 0; colOffset < 9; colOffset += 3)
            {
                HashSet<int> subgridSet = new HashSet<int>();
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        int number = grid[row + rowOffset, col + colOffset];
                        if (number != 0)
                        {
                            if (subgridSet.Contains(number))
                            {
                                // Duplicate number found in the grid
                                return false;
                            }
                            subgridSet.Add(number);
                        }
                        else
                        {
                            // If any cell is empty, the Sudoku is incorrect
                            return false;
                        }
                    }
                }
            }
        }

        // If all checks pass, the Sudoku is correct
        return true;
    }


    private void Done()
    {
        bool isSudokuValid = CheckSudoku();
        Debug.Log("Is Sudoku Valid: " + isSudokuValid);

        if (isSudokuValid)
        {
            if (correctBox != null)
            {
               
                coinManager.AddCoins(20);
                Debug.Log("Coins added: 20");
                correctBox.ShowMessage("Your answers are correct!\n\nYou have earned 6 coins!");
                PlayCorrectSound();
            }
            else
            {
                Debug.Log("null");
            }
        }
        else
        {
            if (confirmBox != null)
            {
            
                confirmBox.ShowMessage("Your answers are incorrect!");
               
            }
            else
            {
                Debug.Log("null");
            }
        }
    }


    


    public void OnPointerDown(PointerEventData eventData)
    {

        PlayButtonSound();
        Done();
    }
}
