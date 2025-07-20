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
        base.Initialize();
        _isActive = new bool[Size, Size];
    }

    public override void Resize(int newSize)
    {
        base.Resize(newSize);
    }

    public bool GetIsActive(int row, int col) => _isActive[row, col];

    public void ToggleCell(int row, int col)
    {
        _isActive[row, col] = !_isActive[row, col];
    }

    public override void OnCellClick(int row, int col)
    {
        // Tango puzzle logic: toggle cell activation (for now)
        ToggleCell(row, col);
    }

    public override void OnBorderClick(int row1, int col1, int row2, int col2)
    {
        // Tango puzzle: border logic (to be implemented)
        // For now, just a placeholder
    }

    public override bool ShowBorderActions => true;
}
