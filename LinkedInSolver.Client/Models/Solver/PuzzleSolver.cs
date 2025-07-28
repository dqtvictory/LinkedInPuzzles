namespace LinkedInSolver.Client.Models.Solver;

/// <summary>
///     Shared interface for puzzle solutions
/// </summary>
public abstract class PuzzleSolver
{
    /// <summary>
    ///     The puzzle grid to solve
    /// </summary>
    protected Grid grid;

    protected PuzzleSolver(Grid grid)
    {
        this.grid = grid;
    }

    /// <summary>
    ///     Validate the grid's state
    /// </summary>
    /// <returns>Validation message if invalid, null if valid</returns>
    public abstract string? Validate();

    /// <summary>
    ///     Solve the puzzle based on the current grid state
    /// </summary>
    /// <returns>
    ///     A list of positions as a common solution's type. Depending on the specific puzzle,
    ///     the list has a different meaning
    /// </returns>
    public abstract List<Pos> Solve();
}
