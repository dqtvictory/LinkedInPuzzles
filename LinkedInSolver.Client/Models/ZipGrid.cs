namespace LinkedInSolver.Client.Models;

public class ZipGrid(int size) : Grid(size)
{
    // Map from a position to a number. If 0, the cell is empty
    private int[,] posToNum = null!;
    // Map from a number to a position
    private Pos[] numToPos = null!;
    // Set of walls between cells, represented as pairs of positions where first is always compared less than second
    private HashSet<(Pos, Pos)> walls = null!;

    protected override void Initialize()
    {
        posToNum = new int[Size, Size];
        numToPos = new Pos[Size * Size + 1]; // +1 to ignore 0 index
        Array.Fill(numToPos, Pos.Invalid);
        walls = [];
    }

    // public bool GetHasRightWall(Pos pos) => _hasRightWall[pos.Row, pos.Col];
    public bool GetHasRightWall(Pos pos) => walls.Contains((pos, pos.GetNeighbor(Pos.Direction.Right)));

    // public bool GetHasBottomWall(Pos pos) => _hasBottomWall[pos.Row, pos.Col];
    public bool GetHasBottomWall(Pos pos) => walls.Contains((pos, pos.GetNeighbor(Pos.Direction.Down)));

    public void SetCellNumber(Pos pos, int number)
    {
        posToNum[pos.Row, pos.Col] = number;
        numToPos[number] = pos;
    }

    public int GetCellNumber(Pos pos)
    {
        return posToNum[pos.Row, pos.Col];
    }

    public void ClearCellNumber(Pos pos)
    {
        int number = posToNum[pos.Row, pos.Col];
        posToNum[pos.Row, pos.Col] = 0;
        numToPos[number] = Pos.Invalid;
    }

    public bool HasNumber(Pos pos) => GetCellNumber(pos) > 0;

    public int GetSmallestMissingNumber()
    {
        return Enumerable.Range(1, Size * Size)
            .FirstOrDefault(i => numToPos[i] == Pos.Invalid, -1);
    }

    public int GetMaxNumber()
    {
        return Enumerable.Range(1, Size * Size)
            .Reverse()
            .FirstOrDefault(i => numToPos[i] != Pos.Invalid, -1);
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
        var wall = pos1 < pos2 ? (pos1, pos2) : (pos2, pos1);
        if (!walls.Remove(wall))
        {
            walls.Add(wall);
        }
    }

    public override bool HasBorderActions => true;
}
