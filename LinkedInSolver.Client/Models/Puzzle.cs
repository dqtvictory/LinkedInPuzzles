using System.Diagnostics;

namespace LinkedInSolver.Client.Models;

/// <summary>
///     Helper class containing information about a puzzle
/// </summary>
public record Puzzle(string Name, string Description, string Icon, string Route)
{
    public static readonly IReadOnlyList<Puzzle> AllPuzzles =
    [
        new("Zip", "Complete the path", "zip.svg", "/zip"),
        new("Tango", "Harmonize the grid", "tango.svg", "/tango"),
        new("Queens", "Crown each region", "queens.svg", "/queens")
    ];

    /// <summary>
    ///     Get a puzzle object by name
    /// </summary>
    /// <param name="name">A valid puzzle's name</param>
    public static Puzzle GetPuzzle(string name)
    {
        var puzzle =
            AllPuzzles.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        Debug.Assert(puzzle != null, $"Puzzle with name '{name}' not found.");
        return puzzle;
    }
}
