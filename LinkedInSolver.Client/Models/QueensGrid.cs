namespace LinkedInSolver.Client.Models;

public class QueensGrid : Grid
{
    // Queens-specific state array
    private bool[,] _isActive;

    public QueensGrid(int size = DEFAULT_SIZE) : base(size)
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
        // Queens puzzle logic: toggle cell activation
        ToggleCell(row, col);
    }

    public override void OnBorderClick(int row1, int col1, int row2, int col2)
    {
        // Queens puzzle: no border interactions
        // (border actions are disabled for Queens)
    }

    public override bool ShowBorderActions => false;
}
