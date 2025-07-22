namespace LinkedInSolver.Client.Models;

public class QueensGrid : Grid
{
    // Queens-specific state array
    private bool[,] _isActive = null!;

    public QueensGrid(int size = DEFAULT_SIZE) : base(size) {}

    protected override void Initialize()
    {
        _isActive = new bool[Size, Size];
    }

    public bool GetIsActive(Pos pos) => _isActive[pos.Row, pos.Col];

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

    public override bool HasBorderActions => false;
}
