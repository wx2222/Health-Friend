using UnityEngine;

public class SudokuGameManager : MonoBehaviour
{
    public static SudokuGameManager Instance { get; private set; }

    public SudokuCell[,] gridCells;

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
    }

    public bool IsGridFilled()
    {
        foreach (SudokuCell cell in gridCells)
        {
            if (!cell.IsFilled())
            {
                return false;
            }
        }
        return true;
    }
}
