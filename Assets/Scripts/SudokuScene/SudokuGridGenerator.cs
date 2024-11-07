using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SudokuGridGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    public TextMeshProUGUI numberTextPrefab;
    public Image overlayImage;
    public int gridSize = 9;
    public float cellSize = 120f;
    public float spacing = 10f;
    public Color outlineColor = Color.black;
    public float outlineThickness = 2f;
    public float lineThickness = 2f;
    public Canvas canvas;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        // Calculate the size of the grid based on the cell size and spacing
        float gridWidth = gridSize * cellSize + (gridSize - 1) * spacing;
        float gridHeight = gridSize * cellSize + (gridSize - 1) * spacing;

        
        float scale = Mathf.Min(canvas.pixelRect.width / gridWidth, canvas.pixelRect.height / gridHeight);

        
        float scaledCellSize = cellSize * scale;
        float scaledSpacing = spacing * scale;

       
        float scaledGridWidth = gridSize * scaledCellSize + (gridSize - 1) * scaledSpacing;
        float scaledGridHeight = gridSize * scaledCellSize + (gridSize - 1) * scaledSpacing;

      
        float startX = -scaledGridWidth / 2f + scaledCellSize / 2f + outlineThickness / 2f;
        float startY = scaledGridHeight / 2f - scaledCellSize / 2f + outlineThickness / 2f;

        
        GameObject gridParent = new GameObject("SudokuGrid");
        gridParent.transform.SetParent(transform);

        
        GameObject outline = new GameObject("Outline");
        outline.transform.SetParent(gridParent.transform);

        
        Image outlineImage = outline.AddComponent<Image>();
        outlineImage.color = outlineColor;

        
        RectTransform outlineRectTransform = outline.GetComponent<RectTransform>();
        outlineRectTransform.sizeDelta = new Vector2(scaledGridWidth + outlineThickness, scaledGridHeight + outlineThickness);
        outlineRectTransform.anchoredPosition = Vector2.zero;

        // Create the horizontal lines
        for (int row = 0; row < gridSize - 1; row++)
        {
            GameObject line = new GameObject("HorizontalLine");
            line.transform.SetParent(gridParent.transform);
            line.AddComponent<Image>().color = outlineColor;

            RectTransform lineRectTransform = line.GetComponent<RectTransform>();
            lineRectTransform.sizeDelta = new Vector2(scaledGridWidth, lineThickness);
            lineRectTransform.anchoredPosition = new Vector2(0f, startY - row * (scaledCellSize + scaledSpacing) - (scaledCellSize + scaledSpacing) / 2f);
        }

        // Create the vertical lines
        for (int col = 0; col < gridSize - 1; col++)
        {
            GameObject line = new GameObject("VerticalLine");
            line.transform.SetParent(gridParent.transform);
            Image lineImage = line.AddComponent<Image>();
            lineImage.color = outlineColor;

            RectTransform lineRectTransform = line.GetComponent<RectTransform>();
            lineRectTransform.sizeDelta = new Vector2(lineThickness, scaledGridHeight);
            lineRectTransform.anchoredPosition = new Vector2(startX + col * (scaledCellSize + scaledSpacing) + (scaledCellSize + scaledSpacing) / 2f, 0f);
        }

        // Create the individual cells and assign numbers
        int[,] sudokuGrid = GenerateSudokuGrid();

        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                // Create a new cell object
                GameObject cell = Instantiate(cellPrefab, gridParent.transform);

                // Set the position and size of the cell
                RectTransform cellRectTransform = cell.GetComponent<RectTransform>();
                cellRectTransform.sizeDelta = new Vector2(scaledCellSize, scaledCellSize);
                cellRectTransform.anchoredPosition = new Vector2(startX + col * (scaledCellSize + scaledSpacing), startY - row * (scaledCellSize + scaledSpacing));

               
                TextMeshProUGUI numberText = Instantiate(numberTextPrefab, cell.transform);
                numberText.fontSize = 80;

                

               
                if (sudokuGrid[row, col] != 0)
                {
                    numberText.text = sudokuGrid[row, col].ToString();

                   
                    Image overlay = Instantiate(overlayImage, cell.transform);
                    RectTransform overlayRectTransform = overlay.GetComponent<RectTransform>();
                    overlayRectTransform.sizeDelta = new Vector2(scaledCellSize, scaledCellSize);
                    overlayRectTransform.anchoredPosition = Vector2.zero;

                    numberText.canvas.sortingOrder = 1;

                    

                }
                else
                {
                    numberText.text = string.Empty; 
                }
                

                bool isGeneratedNumber = sudokuGrid[row, col] != 0;
                SudokuCell sudokuCell = cell.GetComponent<SudokuCell>();
                sudokuCell.SetCellNumber(sudokuGrid[row, col], sudokuGrid[row, col] != 0);

              
                RectTransform numberTextRectTransform = numberText.GetComponent<RectTransform>();
                numberTextRectTransform.sizeDelta = new Vector2(scaledCellSize, scaledCellSize);
                numberTextRectTransform.anchoredPosition = Vector2.zero;
            }
        }

       
        gridParent.transform.localPosition = Vector3.zero;
    }

    private int[,] GenerateSudokuGrid()
    {
        int[,] grid = new int[gridSize, gridSize];
        SudokuGenerator generator = new SudokuGenerator();
        generator.GenerateGrid(grid);
        return grid;
    }
}

public class SudokuGenerator
{
    private const int EmptyCell = 0;
    private const int MinNumber = 1;
    private const int MaxNumber = 9;

    private int[,] grid;

    public void GenerateGrid(int[,] outputGrid)
    {
        grid = outputGrid;
        FillGrid();
        RemoveCells();
    }

    private void FillGrid()
    {
        FillDiagonalBoxes();
        FillRemaining(0, 3);
    }

    private void FillDiagonalBoxes()
    {
        for (int boxStartRow = 0; boxStartRow < MaxNumber; boxStartRow += 3)
        {
            FillBox(boxStartRow, boxStartRow);
        }
    }

    private bool FillBox(int row, int col)
    {
        int num;
        bool[] used = new bool[MaxNumber + 1];

        for (int boxRow = 0; boxRow < 3; boxRow++)
        {
            for (int boxCol = 0; boxCol < 3; boxCol++)
            {
                do
                {
                    num = Random.Range(MinNumber, MaxNumber + 1);
                } while (used[num]);

                used[num] = true;
                grid[row + boxRow, col + boxCol] = num;
            }
        }

        return true;
    }

    private bool FillRemaining(int row, int col)
    {
        if (col >= MaxNumber && row < MaxNumber - 1)
        {
            row++;
            col = 0;
        }

        if (row >= MaxNumber && col >= MaxNumber)
        {
            return true;
        }

        if (row < 3)
        {
            if (col < 3)
            {
                col = 3;
            }
        }
        else if (row < MaxNumber - 3)
        {
            if (col == (int)(row / 3) * 3)
            {
                col += 3;
            }
        }
        else
        {
            if (col == MaxNumber - 3)
            {
                row++;
                col = 0;
                if (row >= MaxNumber)
                {
                    return true;
                }
            }
        }

        for (int num = MinNumber; num <= MaxNumber; num++)
        {
            if (IsValidNumber(num, row, col))
            {
                grid[row, col] = num;
                if (FillRemaining(row, col + 1))
                {
                    return true;
                }
                grid[row, col] = EmptyCell;
            }
        }

        return false;
    }

    private bool IsValidNumber(int num, int row, int col)
    {
        return !IsNumberInRow(num, row) && !IsNumberInColumn(num, col) && !IsNumberInBox(num, row - row % 3, col - col % 3);
    }

    private bool IsNumberInRow(int num, int row)
    {
        for (int col = 0; col < MaxNumber; col++)
        {
            if (grid[row, col] == num)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsNumberInColumn(int num, int col)
    {
        for (int row = 0; row < MaxNumber; row++)
        {
            if (grid[row, col] == num)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsNumberInBox(int num, int boxStartRow, int boxStartCol)
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (grid[row + boxStartRow, col + boxStartCol] == num)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void RemoveCells()
    {
        int emptyCellsCount = 0;

        while (emptyCellsCount < 40)
        {
            int row = Random.Range(0, MaxNumber);
            int col = Random.Range(0, MaxNumber);

            if (grid[row, col] != EmptyCell)
            {
                int temp = grid[row, col];
                grid[row, col] = EmptyCell;

                if (!IsValidPuzzle())
                {
                    grid[row, col] = temp;
                }
                else
                {
                    emptyCellsCount++;
                }
            }
        }
    }

    private bool IsValidPuzzle()
    {
        int[,] copyGrid = new int[MaxNumber, MaxNumber];

        for (int row = 0; row < MaxNumber; row++)
        {
            for (int col = 0; col < MaxNumber; col++)
            {
                copyGrid[row, col] = grid[row, col];
            }
        }

        return SolvePuzzle(copyGrid);
    }

    private bool SolvePuzzle(int[,] puzzle)
    {
        int row, col;

        if (!FindEmptyLocation(puzzle, out row, out col))
        {
            return true;
        }

        for (int num = MinNumber; num <= MaxNumber; num++)
        {
            if (IsValidNumber(num, row, col))
            {
                puzzle[row, col] = num;

                if (SolvePuzzle(puzzle))
                {
                    return true;
                }

                puzzle[row, col] = EmptyCell;
            }
        }

        return false;
    }

    private bool FindEmptyLocation(int[,] puzzle, out int row, out int col)
    {
        for (row = 0; row < MaxNumber; row++)
        {
            for (col = 0; col < MaxNumber; col++)
            {
                if (puzzle[row, col] == EmptyCell)
                {
                    return true;
                }
            }
        }

        row = -1;
        col = -1;
        return false;
    }
}
