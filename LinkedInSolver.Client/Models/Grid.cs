namespace LinkedInSolver.Client.Models;

public class Grid
{
    public const int DEFAULT_SIZE = 8, MIN_SIZE = 4, MAX_SIZE = 16;

    public int Size { get; set; } = DEFAULT_SIZE;
    public Pos[,] Cells { get; set; }

    public Grid(int size = DEFAULT_SIZE)
    {
        Size = size;
        Cells = new Pos[Size, Size];
        Initialize();
    }

    protected virtual void Initialize()
    {
        for (int row = 0; row < Size; row++)
        {
            for (int col = 0; col < Size; col++)
            {
                Cells[row, col] = new Pos(row, col);
            }
        }
    }

    public virtual void Resize(int newSize)
    {
        if (newSize >= MIN_SIZE && newSize <= MAX_SIZE)
        {
            Size = newSize;
            Cells = new Pos[Size, Size];
            Initialize();
        }
    }

    public virtual void OnCellClick(Pos pos)
    {
        // Default behavior - do nothing (let subclasses override)
    }

    public virtual void OnBorderClick(Pos pos1, Pos pos2)
    {
        // Default behavior - no action (can be overridden by subclasses)
    }

    public virtual bool ShowBorderActions => true;
}
