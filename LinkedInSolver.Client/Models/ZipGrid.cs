using LinkedInSolver.Client.Models.Solver;

namespace LinkedInSolver.Client.Models;

public class ZipGrid(int size) : Grid(size)
{
    /// Collection of walls between cells, represented as pairs of positions where first position
    /// is always compared less than second
    public HashSet<(Pos, Pos)> Walls { get; private set; } = null!;

    /// Map from a number to a position
    public Pos[] NumToPos { get; private set; } = null!;

    /// Map from a position to a number. If 0, the cell is empty
    public int[,] PosToNum { get; private set; } = null!;

    public override string? ValidateInputSize(int inputSize)
    {
        return inputSize is >= 6 and <= 12 ? null : "Size must be between 6 and 12";
    }

    protected override void ResetState()
    {
        Solver = new ZipSolver(this);
        PosToNum = new int[Size, Size];
        NumToPos = new Pos[Size * Size + 1]; // +1 to ignore 0 index
        Array.Fill(NumToPos, Pos.Invalid);
        Walls = [];
    }

    /// <summary>
    ///     Whether there is a wall to the right of the specified cell position
    /// </summary>
    public bool HasRightWall(Pos pos)
    {
        return Walls.Contains((pos, pos.GetNeighbor(Pos.Direction.Right)));
    }

    /// <summary>
    ///     Whether there is a wall to the bottom of the specified cell position
    /// </summary>
    public bool HasBottomWall(Pos pos)
    {
        return Walls.Contains((pos, pos.GetNeighbor(Pos.Direction.Down)));
    }

    /// <summary>
    ///     Toggle a wall between two cell positions
    /// </summary>
    public void ToggleWall(Pos pos1, Pos pos2)
    {
        var wall = Pos.GetSortedPair(pos1, pos2);
        if (!Walls.Remove(wall)) Walls.Add(wall);
    }

    /// <summary>
    ///     Get the maximum number assigned to a cell in the grid
    /// </summary>
    public int GetMaxNumber()
    {
        return Enumerable
            .Range(1, Size * Size)
            .Reverse()
            .FirstOrDefault(i => NumToPos[i] != Pos.Invalid, -1);
    }

    /// <summary>
    ///     Whether the cell at the specified position has a number assigned to it
    /// </summary>
    public bool HasNumber(Pos pos)
    {
        return GetCellNumber(pos) > 0;
    }

    /// <summary>
    ///     Get the number assigned to the cell at the specified position
    /// </summary>
    public int GetCellNumber(Pos pos)
    {
        return PosToNum[pos.Row, pos.Col];
    }

    /// <summary>
    ///     Assign a number to the cell at the specified position
    /// </summary>
    public void SetCellNumber(Pos pos, int number)
    {
        PosToNum[pos.Row, pos.Col] = number;
        NumToPos[number] = pos;
    }

    /// <summary>
    ///     Unassign a number to the cell at the specified position
    /// </summary>
    public void ClearCellNumber(Pos pos)
    {
        var number = PosToNum[pos.Row, pos.Col];
        PosToNum[pos.Row, pos.Col] = 0;
        NumToPos[number] = Pos.Invalid;
    }

    /// <summary>
    ///     Get the smallest missing number in the grid. If all assigned numbers are consecutive
    ///     from 1, return the next number
    /// </summary>
    public int GetSmallestMissingNumber()
    {
        return Enumerable
            .Range(1, Size * Size)
            .FirstOrDefault(i => NumToPos[i] == Pos.Invalid, -1);
    }
}
