namespace LinkedInSolver.Client.Models;

public record struct Pos(int Row, int Col)
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
    }

    private static readonly Pos[] _directions =
    {
        new(-1, 0), // Up
        new(1, 0), // Down
        new(0, -1), // Left
        new(0, 1), // Right
        new(-1, -1), // TopLeft
        new(-1, 1), // TopRight
        new(1, -1), // BottomLeft
        new(1, 1), // BottomRight
    };

    public static Pos Invalid => new(-1, -1);

    public static Pos operator +(Pos a, Pos b) => new(a.Row + b.Row, a.Col + b.Col);

    public static Pos operator -(Pos a, Pos b) => new(a.Row - b.Row, a.Col - b.Col);

    public static bool operator <(Pos a, Pos b) =>
        (a.Row < b.Row) || (a.Row == b.Row && a.Col < b.Col);

    public static bool operator >(Pos a, Pos b) => !(a < b || a == b);

    public Pos GetNeighbor(Direction direction)
    {
        var offset = _directions[(int)direction];
        return this + offset;
    }

    /// <summary>
    /// Gets the 4 orthogonal neighbors (up, down, left, right)
    /// </summary>
    public IEnumerable<Pos> GetNeighbors()
    {
        yield return GetNeighbor(Direction.Up);
        yield return GetNeighbor(Direction.Down);
        yield return GetNeighbor(Direction.Left);
        yield return GetNeighbor(Direction.Right);
    }

    /// <summary>
    /// Gets the 4 diagonal neighbors
    /// </summary>
    public IEnumerable<Pos> GetDiagNeighbors()
    {
        yield return GetNeighbor(Direction.TopLeft);
        yield return GetNeighbor(Direction.TopRight);
        yield return GetNeighbor(Direction.BottomLeft);
        yield return GetNeighbor(Direction.BottomRight);
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
