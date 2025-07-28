using LinkedInSolver.Client.Models.Solver;

namespace LinkedInSolver.Client.Models;

public class TangoGrid(int size) : Grid(size)
{
    private bool[,] _isActive = null!;

    public override bool HasBorderActions => true;

    protected override void ResetState()
    {
        Solver = new TangoSolver(this);
        _isActive = new bool[Size, Size];
    }

    public bool GetIsActive(Pos pos)
    {
        return _isActive[pos.Row, pos.Col];
    }

    public override void OnCellClick(Pos pos)
    {
        // Tango puzzle logic: toggle cell activation (for now)
        _isActive[pos.Row, pos.Col] = !_isActive[pos.Row, pos.Col];
    }

    public override void OnBorderClick(Pos pos1, Pos pos2)
    {
        // Tango puzzle: border logic (to be implemented)
        // For now, just a placeholder
    }
}
