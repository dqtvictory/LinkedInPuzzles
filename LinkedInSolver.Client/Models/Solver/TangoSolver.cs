namespace LinkedInSolver.Client.Models.Solver;

public class TangoSolver(TangoGrid puzzleGrid) : PuzzleSolver(puzzleGrid)
{
    public override string? Validate()
    {
        return null;
    }

    /// <summary>
    ///     Solve the Tango puzzle
    /// </summary>
    /// <returns>
    ///     List of cells where the Moons are placed. Cells that are not listed are either
    ///     pre-placed or to be filled with Suns. Returns an empty list if not solvable
    /// </returns>
    public override List<Pos> Solve()
    {
        return [new Pos(0, 0), new Pos(0, 1), new Pos(0, 2)];
    }
}
