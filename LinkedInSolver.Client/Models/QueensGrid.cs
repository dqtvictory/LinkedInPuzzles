using LinkedInSolver.Client.Models.Solver;

namespace LinkedInSolver.Client.Models;

public class QueensGrid(int size) : Grid(size)
{
    /// <summary>
    ///     State array where each cell indicates which region index it belongs to
    /// </summary>
    public int[,] State { get; private set; } = null!;

    public override string? ValidateInputSize(int inputSize)
    {
        return inputSize is >= 6 and <= 12 ? null : "Size must be between 6 and 12";
    }

    protected override void ResetState()
    {
        Solver = new QueensSolver(this);
        State = new int[Size, Size];
    }

    /// <summary>
    ///     Get the region index for a specific cell position
    /// </summary>
    public int GetRegionForCell(Pos pos)
    {
        return State[pos.Row, pos.Col];
    }

    /// <summary>
    ///     Set the region index for a specific cell position
    /// </summary>
    public void SetRegionForCell(Pos pos, int regionIndex)
    {
        State[pos.Row, pos.Col] = regionIndex;
    }
}
