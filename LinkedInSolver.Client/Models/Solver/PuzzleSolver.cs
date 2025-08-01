namespace LinkedInSolver.Client.Models.Solver;

/// <summary>
///     Shared interface for puzzle solutions
/// </summary>
public abstract class PuzzleSolver
{
    /// <summary>
    ///     Validate the grid's state
    /// </summary>
    /// <returns>Validation message if invalid, null if valid</returns>
    public abstract string? Validate();

    /// <summary>
    ///     Solve the puzzle based on the current grid state
    /// </summary>
    /// <returns>
    ///     A list of positions as a common solution's type. Depending on the specific puzzle, the
    ///     list has a different meaning, but an empty list always means that the puzzle is not
    ///     solvable
    /// </returns>
    public abstract List<Pos> Solve();
}
