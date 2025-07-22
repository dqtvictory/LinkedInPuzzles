namespace LinkedInSolver.Client.Models;

public class TangoGrid : Grid
{
    // Tango-specific state array
    private bool[,] _isActive;

    public TangoGrid(int size = DEFAULT_SIZE) : base(size)
    {
        _isActive = new bool[Size, Size];
    }

    protected override void Initialize()
    {
        _isActive = new bool[Size, Size];
    }

    public override void Resize(int newSize)
    {
        base.Resize(newSize);
    }

    public bool GetIsActive(Pos pos) => _isActive[pos.Row, pos.Col];

    public void ToggleCell(Pos pos)
    {
        _isActive[pos.Row, pos.Col] = !_isActive[pos.Row, pos.Col];
    }

    public override void OnCellClick(Pos pos)
    {
        // Tango puzzle logic: toggle cell activation (for now)
        ToggleCell(pos);
    }

    public override void OnBorderClick(Pos pos1, Pos pos2)
    {
        // Tango puzzle: border logic (to be implemented)
        // For now, just a placeholder
    }

    public override bool HasBorderActions => true;
}
