namespace LinkedInSolver.Client.Models;

public record Pos(int Row, int Col)
{
    public static Pos operator +(Pos a, Pos b) => new(a.Row + b.Row, a.Col + b.Col);
    public static Pos operator -(Pos a, Pos b) => new(a.Row - b.Row, a.Col - b.Col);

    /// <summary>
    /// Gets the 4 orthogonal neighbors (up, down, left, right)
    /// </summary>
    public IEnumerable<Pos> GetNeighbors()
    {
        yield return new Pos(Row - 1, Col);     // Up
        yield return new Pos(Row + 1, Col);     // Down
        yield return new Pos(Row, Col - 1);     // Left
        yield return new Pos(Row, Col + 1);     // Right
    }

    /// <summary>
    /// Gets the 4 diagonal neighbors
    /// </summary>
    public IEnumerable<Pos> GetDiagNeighbors()
    {
        yield return new Pos(Row - 1, Col - 1); // Top-left
        yield return new Pos(Row - 1, Col + 1); // Top-right
        yield return new Pos(Row + 1, Col - 1); // Bottom-left
        yield return new Pos(Row + 1, Col + 1); // Bottom-right
    }

    /// <summary>
    /// Gets all 8 neighbors (orthogonal + diagonal)
    /// </summary>
    public IEnumerable<Pos> GetAllNeighbors()
    {
        return GetNeighbors().Concat(GetDiagNeighbors());
    }

    /// <summary>
    /// Checks if the position is within the given grid bounds
    /// </summary>
    public bool IsInBounds(int size) => Row >= 0 && Row < size && Col >= 0 && Col < size;

    public override string ToString() => $"({Row},{Col})";
}
