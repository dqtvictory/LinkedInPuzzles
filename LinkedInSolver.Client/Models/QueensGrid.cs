using LinkedInSolver.Client.Models.Solver;

namespace LinkedInSolver.Client.Models;

public class QueensGrid(int size) : Grid(size)
{
    private bool[,] _isActive = null!;

    public override bool HasBorderActions => false;

    protected override void ResetState()
    {
        Solver = new QueensSolver(this);
        _isActive = new bool[Size, Size];
    }

    public bool GetIsActive(Pos pos)
    {
        return _isActive[pos.Row, pos.Col];
    }

    public override void OnCellClick(Pos pos)
    {
        // Queens puzzle logic: toggle cell activation
        _isActive[pos.Row, pos.Col] = !_isActive[pos.Row, pos.Col];
    }

    public override void OnBorderClick(Pos pos1, Pos pos2)
    {
        // Queens: no border interactions
        // (border actions are disabled for Queens)
    }
}
