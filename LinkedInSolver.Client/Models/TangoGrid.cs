using LinkedInSolver.Client.Models.Solver;

namespace LinkedInSolver.Client.Models;

public class TangoGrid(int size) : Grid(size)
{
    /// <summary>
    ///     Type of a border between two cells
    /// </summary>
    public enum BorderType
    {
        None,
        Equal,
        Opposite
    }

    /// <summary>
    ///     Type of a cell
    /// </summary>
    public enum CellType
    {
        Empty,
        Sun,
        Moon
    }

    private Dictionary<(Pos, Pos), BorderType> _borders = null!;

    private CellType[,] _cells = null!;

    protected override void ResetState()
    {
        Solver = new TangoSolver(this);

        _cells = new CellType[Size, Size];
        // Assign every cell to type EMPTY initially
        for (var row = 0; row < Size; row++)
        for (var col = 0; col < Size; col++)
            _cells[row, col] = CellType.Empty;

        _borders = new Dictionary<(Pos, Pos), BorderType>();
    }

    public CellType GetCellType(Pos pos)
    {
        return _cells[pos.Row, pos.Col];
    }

    public void SetCellType(Pos pos, CellType cellType)
    {
        _cells[pos.Row, pos.Col] = cellType;
    }

    public BorderType GetBorderType(Pos pos1, Pos pos2)
    {
        return _borders.GetValueOrDefault(Pos.GetSortedPair(pos1, pos2), BorderType.None);
    }

    public void SetBorderType(Pos pos1, Pos pos2, BorderType borderType)
    {
        _borders[Pos.GetSortedPair(pos1, pos2)] = borderType;
    }
}
