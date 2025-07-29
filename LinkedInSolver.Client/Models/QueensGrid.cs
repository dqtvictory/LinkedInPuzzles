using LinkedInSolver.Client.Models.Solver;

namespace LinkedInSolver.Client.Models;

public class QueensGrid(int size) : Grid(size)
{
    /// <summary>
    ///     State array where each cell indicates which region index it belongs to
    /// </summary>
    private int[,] _state = null!;

    protected override void ResetState()
    {
        Solver = new QueensSolver(this);
        _state = new int[Size, Size];
        // Assign every cell to region 0 initially
        for (var row = 0; row < Size; row++)
        for (var col = 0; col < Size; col++)
            _state[row, col] = 0;
    }

    /// <summary>
    ///     Get the region index for a specific cell position
    /// </summary>
    public int GetRegionForCell(Pos pos)
    {
        return _state[pos.Row, pos.Col];
    }

    /// <summary>
    ///     Set the region index for a specific cell position
    /// </summary>
    public void SetRegionForCell(Pos pos, int regionIndex)
    {
        _state[pos.Row, pos.Col] = regionIndex;
    }
}
