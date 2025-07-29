using LinkedInSolver.Client.Models.Solver;

namespace LinkedInSolver.Client.Models;

public class TangoGrid(int size) : Grid(size)
{
    private bool[,] _isActive = null!;

    protected override void ResetState()
    {
        Solver = new TangoSolver(this);
        _isActive = new bool[Size, Size];
    }

    public bool GetIsActive(Pos pos)
    {
        return _isActive[pos.Row, pos.Col];
    }

    public void OnCellClick(Pos pos)
    {
        _isActive[pos.Row, pos.Col] = !_isActive[pos.Row, pos.Col];
    }

    public void OnBorderClick(Pos pos1, Pos pos2)
    {
        // Tango puzzle: border logic (to be implemented)
        // For now, just a placeholder
    }
}
