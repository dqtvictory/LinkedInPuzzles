using System.Diagnostics;

namespace LinkedInSolver.Client.Models;

public record Puzzle(string Name, string Description, string Icon, string Route)
{
    public static readonly List<Puzzle> ALL_PUZZLES = new()
    {
        new Puzzle("Zip", "Complete the path", "zip.svg", "/zip"),
        new Puzzle("Tango", "Harmonize the grid", "tango.svg", "/tango"),
        new Puzzle("Queens", "Crown each region", "queens.svg", "/queens"),
    };

    public static Puzzle GetPuzzle(string name)
    {
        var puzzle = ALL_PUZZLES.FirstOrDefault(p =>
            p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
        );
        Debug.Assert(puzzle != null, $"Puzzle with name '{name}' not found.");
        return puzzle;
    }
}
