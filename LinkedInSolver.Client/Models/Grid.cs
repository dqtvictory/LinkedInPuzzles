namespace LinkedInSolver.Client.Models;

public abstract class Grid
{
    public const int DEFAULT_SIZE = 8, MIN_SIZE = 4, MAX_SIZE = 16;

    public int Size { get; set; } = DEFAULT_SIZE;

    public Grid(int size = DEFAULT_SIZE)
    {
        Size = size;
        Initialize();
    }

    public virtual void Resize(int newSize)
    {
        Size = newSize;
        Initialize();
    }

    protected abstract void Initialize();
    public abstract void OnCellClick(Pos pos);
    public abstract void OnBorderClick(Pos pos1, Pos pos2);

    public virtual bool HasBorderActions => true;
}
