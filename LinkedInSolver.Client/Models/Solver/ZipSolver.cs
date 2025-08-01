namespace LinkedInSolver.Client.Models.Solver;

public class ZipSolver(ZipGrid grid) : PuzzleSolver
{
    public override string? Validate()
    {
        return null;
    }

    /// <summary>
    ///     Solve the Zip puzzle
    /// </summary>
    /// <returns>List of cells in the order of the cells to travel, empty if not solvable</returns>
    public override List<Pos> Solve()
    {
        return [new Pos(0, 0), new Pos(0, 1), new Pos(0, 2)];
    }
}
