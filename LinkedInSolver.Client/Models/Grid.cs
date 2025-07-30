using LinkedInSolver.Client.Models.Solver;

namespace LinkedInSolver.Client.Models;

/// <summary>
///     Represents a square grid of cells with a specified size
/// </summary>
public abstract class Grid
{
    // Some default constants
    public const int
        DefaultSize = 6,
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
    ///     If not empty, contains the solution found by the solver. This is mainly used by the UI
    ///     to display the solution
    /// </summary>
    public IEnumerable<Pos>? Solution { get; set; }

    /// <summary>
    ///     Resize the grid to a new size then reset its state
    /// </summary>
    /// <param name="newSize">Grid's new size</param>
    public void Reset(int newSize)
    {
        Size = newSize;
        Solution = null;
        ResetState();
    }

    /// <summary>
    ///     Check whether a cell position is within the bounds of the grid
    /// </summary>
    public bool IsInBounds(Pos pos)
    {
        return pos.Row >= 0 && pos.Col >= 0 && pos.Row < Size && pos.Col < Size;
    }

    /// <summary>
    ///     Check whether a solution has been found for the grid
    /// </summary>
    public bool HasSolution()
    {
        return Solution != null && Solution.Any();
    }

    /// <summary>
    ///     Reset grid's state
    /// </summary>
    protected abstract void ResetState();

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
