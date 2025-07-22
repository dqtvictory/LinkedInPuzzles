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

    public bool GetIsActive(Pos pos) => _isActive[pos.Row, pos.Col];

    public void ToggleCell(Pos pos)
    {
        _isActive[pos.Row, pos.Col] = !_isActive[pos.Row, pos.Col];
    }

    public override void OnCellClick(Pos pos)
    {
        // Queens puzzle logic: toggle cell activation
        ToggleCell(pos);
    }

    public override void OnBorderClick(Pos pos1, Pos pos2)
    {
        // Queens puzzle: no border interactions
        // (border actions are disabled for Queens)
    }

    public override bool ShowBorderActions => false;
}
