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
        _numbers = new int[Size, Size];
        _hasRightWall = new bool[Size, Size];
        _hasBottomWall = new bool[Size, Size];
    }

    public override void Resize(int newSize)
    {
        base.Resize(newSize);
    }

    public bool GetHasRightWall(Pos pos) => _hasRightWall[pos.Row, pos.Col];

    public bool GetHasBottomWall(Pos pos) => _hasBottomWall[pos.Row, pos.Col];

    public void SetCellNumber(Pos pos, int number)
    {
        _numbers[pos.Row, pos.Col] = number;
    }

    public int GetCellNumber(Pos pos)
    {
        return _numbers[pos.Row, pos.Col];
    }

    public void ClearCellNumber(Pos pos)
    {
        _numbers[pos.Row, pos.Col] = 0;
    }

    public bool HasNumber(Pos pos) => GetCellNumber(pos) > 0;

    public void ToggleWall(Pos pos1, Pos pos2)
    {
        // Determine which wall to toggle based on cell positions
        if (pos1.Row == pos2.Row) // Vertical wall (between columns)
        {
            int minCol = Math.Min(pos1.Col, pos2.Col);
            int maxCol = Math.Max(pos1.Col, pos2.Col);
            if (maxCol == minCol + 1) // Adjacent cells
            {
                _hasRightWall[pos1.Row, minCol] = !_hasRightWall[pos1.Row, minCol];
            }
        }
        else if (pos1.Col == pos2.Col) // Horizontal wall (between rows)
        {
            int minRow = Math.Min(pos1.Row, pos2.Row);
            int maxRow = Math.Max(pos1.Row, pos2.Row);
            if (maxRow == minRow + 1) // Adjacent cells
            {
                _hasBottomWall[minRow, pos1.Col] = !_hasBottomWall[minRow, pos1.Col];
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

    public override void OnCellClick(Pos pos)
    {
        // Zip puzzle logic: toggle numbers
        if (HasNumber(pos))
        {
            // Remove number from cell
            ClearCellNumber(pos);
        }
        else
        {
            // Add smallest missing number
            var smallestNumber = GetSmallestMissingNumber();
            SetCellNumber(pos, smallestNumber);
        }
    }

    public override void OnBorderClick(Pos pos1, Pos pos2)
    {
        // Zip puzzle: toggle walls between cells
        ToggleWall(pos1, pos2);
    }

    public override bool HasBorderActions => true;
}
