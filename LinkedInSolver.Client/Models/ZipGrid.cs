using LinkedInSolver.Client.Models.Solver;

namespace LinkedInSolver.Client.Models;

public class ZipGrid(int size) : Grid(size)
{
    /// Map from a number to a position
    private Pos[] _numToPos = null!;

    /// Map from a position to a number. If 0, the cell is empty
    private int[,] _posToNum = null!;

    /// Collection of walls between cells, represented as pairs of positions where first position
    /// is always compared less than second
    private HashSet<(Pos, Pos)> _walls = null!;

    protected override void ResetState()
    {
        Solver = new ZipSolver(this);
        _posToNum = new int[Size, Size];
        _numToPos = new Pos[Size * Size + 1]; // +1 to ignore 0 index
        Array.Fill(_numToPos, Pos.Invalid);
        _walls = [];
    }

    /// <summary>
    ///     Whether there is a wall to the right of the specified cell position
    /// </summary>
    public bool HasRightWall(Pos pos)
    {
        return _walls.Contains((pos, pos.GetNeighbor(Pos.Direction.Right)));
    }

    /// <summary>
    ///     Whether there is a wall to the bottom of the specified cell position
    /// </summary>
    public bool HasBottomWall(Pos pos)
    {
        return _walls.Contains((pos, pos.GetNeighbor(Pos.Direction.Down)));
    }

    /// <summary>
    ///     Toggle a wall between two cell positions
    /// </summary>
    public void ToggleWall(Pos pos1, Pos pos2)
    {
        var wall = Pos.GetSortedPair(pos1, pos2);
        if (!_walls.Remove(wall)) _walls.Add(wall);
    }

    /// <summary>
    ///     Get the maximum number assigned to a cell in the grid
    /// </summary>
    public int GetMaxNumber()
    {
        return Enumerable
            .Range(1, Size * Size)
            .Reverse()
            .FirstOrDefault(i => _numToPos[i] != Pos.Invalid, -1);
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
        return _posToNum[pos.Row, pos.Col];
    }

    /// <summary>
    ///     Assign a number to the cell at the specified position
    /// </summary>
    public void SetCellNumber(Pos pos, int number)
    {
        _posToNum[pos.Row, pos.Col] = number;
        _numToPos[number] = pos;
    }

    /// <summary>
    ///     Unassign a number to the cell at the specified position
    /// </summary>
    public void ClearCellNumber(Pos pos)
    {
        var number = _posToNum[pos.Row, pos.Col];
        _posToNum[pos.Row, pos.Col] = 0;
        _numToPos[number] = Pos.Invalid;
    }

    /// <summary>
    ///     Get the smallest missing number in the grid. If all assigned numbers are consecutive
    ///     from 1, return the next number
    /// </summary>
    public int GetSmallestMissingNumber()
    {
        return Enumerable
            .Range(1, Size * Size)
            .FirstOrDefault(i => _numToPos[i] == Pos.Invalid, -1);
    }
}
