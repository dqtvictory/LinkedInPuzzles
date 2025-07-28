using LinkedInSolver.Client.Models.Solver;

namespace LinkedInSolver.Client.Models;

public class ZipGrid(int size) : Grid(size)
{
    // Map from a number to a position
    private Pos[] _numToPos = null!;

    // Map from a position to a number. If 0, the cell is empty
    private int[,] _posToNum = null!;

    // Set of walls between cells, represented as pairs of positions where first is always compared less than second
    private HashSet<(Pos, Pos)> _walls = null!;

    public override bool HasBorderActions => true;

    protected override void ResetState()
    {
        Solver = new ZipSolver(this);
        _posToNum = new int[Size, Size];
        _numToPos = new Pos[Size * Size + 1]; // +1 to ignore 0 index
        Array.Fill(_numToPos, Pos.Invalid);
        _walls = [];
    }

    public bool GetHasRightWall(Pos pos)
    {
        return _walls.Contains((pos, pos.GetNeighbor(Pos.Direction.Right)));
    }

    public bool GetHasBottomWall(Pos pos)
    {
        return _walls.Contains((pos, pos.GetNeighbor(Pos.Direction.Down)));
    }

    public int GetMaxNumber()
    {
        return Enumerable
            .Range(1, Size * Size)
            .Reverse()
            .FirstOrDefault(i => _numToPos[i] != Pos.Invalid, -1);
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
        if (!_walls.Remove(wall)) _walls.Add(wall);
    }

    public bool HasNumber(Pos pos)
    {
        return GetCellNumber(pos) > 0;
    }

    public int GetCellNumber(Pos pos)
    {
        return _posToNum[pos.Row, pos.Col];
    }

    private void SetCellNumber(Pos pos, int number)
    {
        _posToNum[pos.Row, pos.Col] = number;
        _numToPos[number] = pos;
    }

    private void ClearCellNumber(Pos pos)
    {
        var number = _posToNum[pos.Row, pos.Col];
        _posToNum[pos.Row, pos.Col] = 0;
        _numToPos[number] = Pos.Invalid;
    }

    private int GetSmallestMissingNumber()
    {
        return Enumerable
            .Range(1, Size * Size)
            .FirstOrDefault(i => _numToPos[i] == Pos.Invalid, -1);
    }
}
