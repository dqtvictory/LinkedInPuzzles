namespace LinkedInSolver.Client.Models;

public readonly record struct Pos(int Row, int Col)
{
    /// <summary>
    ///     The 8 directions
    /// </summary>
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    private static readonly Pos[] DeltaDirections =
    [
        new(-1, 0), // Up
        new(1, 0), // Down
        new(0, -1), // Left
        new(0, 1), // Right
        new(-1, -1), // TopLeft
        new(-1, 1), // TopRight
        new(1, -1), // BottomLeft
        new(1, 1) // BottomRight
    ];

    /// <summary>
    ///     An invalid position
    /// </summary>
    public static Pos Invalid => new(-1, -1);

    public static Pos operator +(Pos a, Pos b)
    {
        return new Pos(a.Row + b.Row, a.Col + b.Col);
    }

    public static Pos operator -(Pos a, Pos b)
    {
        return new Pos(a.Row - b.Row, a.Col - b.Col);
    }

    public static bool operator <(Pos a, Pos b)
    {
        return a.Row < b.Row || (a.Row == b.Row && a.Col < b.Col);
    }

    public static bool operator >(Pos a, Pos b)
    {
        return !(a < b || a == b);
    }

    /// <summary>
    ///     Return sorted pair of positions
    /// </summary>
    /// <returns>(pos1, pos2) if pos1 compares less than pos2, otherwise (pos2, pos1)</returns>
    public static (Pos, Pos) GetSortedPair(Pos pos1, Pos pos2)
    {
        return pos1 < pos2 ? (pos1, pos2) : (pos2, pos1);
    }

    /// <summary>
    ///     Get the neighboring position to a direction
    /// </summary>
    public Pos GetNeighbor(Direction direction)
    {
        var offset = DeltaDirections[(int)direction];
        return this + offset;
    }

    /// <summary>
    ///     Gets the 4 orthogonal neighbors (up, down, left, right)
    /// </summary>
    public IEnumerable<Pos> GetNeighbors()
    {
        yield return GetNeighbor(Direction.Up);
        yield return GetNeighbor(Direction.Down);
        yield return GetNeighbor(Direction.Left);
        yield return GetNeighbor(Direction.Right);
    }

    /// <summary>
    ///     Gets the 4 diagonal neighbors
    /// </summary>
    public IEnumerable<Pos> GetDiagNeighbors()
    {
        yield return GetNeighbor(Direction.TopLeft);
        yield return GetNeighbor(Direction.TopRight);
        yield return GetNeighbor(Direction.BottomLeft);
        yield return GetNeighbor(Direction.BottomRight);
    }

    /// <summary>
    ///     Gets all 8 neighbors (orthogonal + diagonal)
    /// </summary>
    public IEnumerable<Pos> GetAllNeighbors()
    {
        return GetNeighbors().Concat(GetDiagNeighbors());
    }

    public override string ToString()
    {
        return $"({Row},{Col})";
    }
}
