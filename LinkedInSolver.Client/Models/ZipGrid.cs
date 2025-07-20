namespace LinkedInSolver.Client.Models;

public class ZipGrid : Grid
{
    // Zip-specific state arrays
    private int[,] _numbers;
    private bool[,] _hasRightWall;
    private bool[,] _hasBottomWall;

    public ZipGrid(int size = DEFAULT_SIZE) : base(size)
    {
        _numbers = new int[Size, Size];
        _hasRightWall = new bool[Size, Size];
        _hasBottomWall = new bool[Size, Size];
    }

    protected override void Initialize()
    {
        base.Initialize();
        _numbers = new int[Size, Size];
        _hasRightWall = new bool[Size, Size];
        _hasBottomWall = new bool[Size, Size];
    }

    public override void Resize(int newSize)
    {
        base.Resize(newSize);
    }

    public bool GetHasRightWall(int row, int col) => _hasRightWall[row, col];

    public bool GetHasBottomWall(int row, int col) => _hasBottomWall[row, col];

    public void SetCellNumber(int row, int col, int number)
    {
        _numbers[row, col] = number;
    }

    public int GetCellNumber(int row, int col)
    {
        return _numbers[row, col];
    }

    public void ClearCellNumber(int row, int col)
    {
        _numbers[row, col] = 0;
    }

    public bool HasNumber(int row, int col) => GetCellNumber(row, col) > 0;

    public void ToggleWall(int row1, int col1, int row2, int col2)
    {
        // Determine which wall to toggle based on cell positions
        if (row1 == row2) // Vertical wall (between columns)
        {
            int minCol = Math.Min(col1, col2);
            int maxCol = Math.Max(col1, col2);
            if (maxCol == minCol + 1) // Adjacent cells
            {
                _hasRightWall[row1, minCol] = !_hasRightWall[row1, minCol];
            }
        }
        else if (col1 == col2) // Horizontal wall (between rows)
        {
            int minRow = Math.Min(row1, row2);
            int maxRow = Math.Max(row1, row2);
            if (maxRow == minRow + 1) // Adjacent cells
            {
                _hasBottomWall[minRow, col1] = !_hasBottomWall[minRow, col1];
            }
        }
    }

    public int GetSmallestMissingNumber()
    {
        var usedNumbers = new HashSet<int>();
        for (int row = 0; row < Size; row++)
        {
            for (int col = 0; col < Size; col++)
            {
                int number = _numbers[row, col];
                if (number > 0)
                {
                    usedNumbers.Add(number);
                }
            }
        }

        for (int i = 1; i <= Size * Size; i++)
        {
            if (!usedNumbers.Contains(i))
            {
                return i;
            }
        }
        return Size * Size + 1; // All numbers used
    }

    public int GetMaxNumber()
    {
        int maxNumber = 0;
        for (int row = 0; row < Size; row++)
        {
            for (int col = 0; col < Size; col++)
            {
                int number = _numbers[row, col];
                if (number > maxNumber)
                {
                    maxNumber = number;
                }
            }
        }
        return maxNumber;
    }

    public override void OnCellClick(int row, int col)
    {
        // Zip puzzle logic: toggle numbers
        if (HasNumber(row, col))
        {
            // Remove number from cell
            ClearCellNumber(row, col);
        }
        else
        {
            // Add smallest missing number
            var smallestNumber = GetSmallestMissingNumber();
            SetCellNumber(row, col, smallestNumber);
        }
    }

    public override void OnBorderClick(int row1, int col1, int row2, int col2)
    {
        // Zip puzzle: toggle walls between cells
        ToggleWall(row1, col1, row2, col2);
    }

    public override bool ShowBorderActions => true;
}
