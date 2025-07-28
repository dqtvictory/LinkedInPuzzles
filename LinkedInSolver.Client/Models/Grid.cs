using LinkedInSolver.Client.Models.Solver;

namespace LinkedInSolver.Client.Models;

/// <summary>
///     Represents a square grid of cells with a specified size
/// </summary>
public abstract class Grid
{
    // Some default constants
    public const int
        DefaultSize = 8,
        MinSize = 4,
        MaxSize = 16;

    /// <summary>
    ///     The solver that will be used to solve the grid
    /// </summary>
    protected PuzzleSolver Solver = null!;

    /// <summary>
    ///     Construct a new grid with the specified size
    /// </summary>
    protected Grid(int size)
    {
        Reset(size);
    }

    /// <summary>
    ///     Size of the square grid
    /// </summary>
    public int Size { get; private set; } = DefaultSize;

    /// <summary>
    ///     Whether the grid should handle border actions
    /// </summary>
    public virtual bool HasBorderActions => true;

    /// <summary>
    ///     Resize the grid to a new size then reset its state
    /// </summary>
    /// <param name="newSize">Grid's new size</param>
    public void Reset(int newSize)
    {
        Size = newSize;
        ResetState();
    }

    /// <summary>
    ///     Reset grid's state
    /// </summary>
    protected abstract void ResetState();

    /// <summary>
    ///     Handle cell click
    /// </summary>
    /// <param name="pos">Position of cell</param>
    public abstract void OnCellClick(Pos pos);

    /// <summary>
    ///     Handle border click between two adjacent cells
    /// </summary>
    /// <param name="pos1">Position of cell 1</param>
    /// <param name="pos2">Position of cell 2</param>
    public abstract void OnBorderClick(Pos pos1, Pos pos2);

    /// <summary>
    ///     Validate the grid's current state
    /// </summary>
    /// <returns>Validation message if invalid, null if valid</returns>
    public string? Validate()
    {
        return Solver.Validate();
    }

    /// <summary>
    ///     Solve the grid's current state, assuming it is valid
    /// </summary>
    /// <returns>The solution to the current puzzle's grid</returns>
    public List<Pos> Solve()
    {
        return Solver.Solve();
    }
}
