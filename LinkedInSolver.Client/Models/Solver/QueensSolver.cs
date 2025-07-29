namespace LinkedInSolver.Client.Models.Solver;

public class QueensSolver(QueensGrid grid) : PuzzleSolver(grid)
{
    /// <summary>
    ///     Solve the Queens puzzle
    /// </summary>
    /// <returns>List of positions where the queens should be placed</returns>
    public override List<Pos> Solve()
    {
        var grid = (QueensGrid)PuzzleGrid;
        var state = grid.State;

        // The approach is simple: go through each region and try to place a queen in each cell. For
        // this reason, we want to go from the region with the smallest size to the largest one
        var regions = new Dictionary<int, List<Pos>>();
        for (var row = 0; row < grid.Size; row++)
        for (var col = 0; col < grid.Size; col++)
        {
            var region = state[row, col];
            if (!regions.ContainsKey(region))
                regions[region] = new List<Pos>();
            regions[region].Add(new Pos(row, col));
        }

        var sortedRegions = regions
            .OrderBy(pair => pair.Value.Count)
            .Select(pair => pair.Value)
            .ToList();

        var res = SolveImpl(sortedRegions, [], 0);
        return res.ToList();
    }

    public override string? Validate()
    {
        var grid = (QueensGrid)PuzzleGrid;
        var state = grid.State;

        // Number of regions equals grid size
        var regionCount = state.Cast<int>().Distinct().Count();
        if (regionCount != grid.Size)
            return
                $"Size of grid and number of regions must match! Size: {grid.Size}, Regions: {regionCount}";

        // The same region must be connected. Use DFS to check connectivity
        {
            var visited = new bool[grid.Size, grid.Size];
            var processedRegions = new HashSet<int>();
            for (var row = 0; row < grid.Size; row++)
            for (var col = 0; col < grid.Size; col++)
            {
                // Skip already visited cells
                if (visited[row, col])
                    continue;

                var region = state[row, col];
                var currentPos = new Pos(row, col);

                // If this region has already been found by DFS, this means that the current cell is
                // disconnected. Return the proper error message
                if (processedRegions.Contains(region))
                    return $"Cell {currentPos} disconnected from region {region}";

                // Start DFS from the first unvisited cell to find its region
                var toVisit = new Stack<Pos>([currentPos]);
                while (toVisit.TryPop(out var nextPos))
                {
                    if (visited[nextPos.Row, nextPos.Col])
                        continue;
                    visited[nextPos.Row, nextPos.Col] = true;
                    processedRegions.Add(region);
                    // Visit all neighbors that belong to the same region
                    var validNeighbors =
                        nextPos
                            .GetNeighbors()
                            .Where(pos =>
                                grid.IsInBounds(pos) && state[pos.Row, pos.Col] == region);
                    foreach (var neighbor in validNeighbors) toVisit.Push(neighbor);
                }
            }
        }

        return null;
    }

    /// <summary>
    ///     Check if a queen can be placed at a specific position
    /// </summary>
    /// <param name="queens">Position of queens that are already placed</param>
    /// <param name="pos">Position being checked</param>
    /// <returns>Whether we can place a new queen at pos</returns>
    private bool CanPlaceQueen(HashSet<Pos> queens, Pos pos)
    {
        // No queens placed yet, a new queen can always be placed
        if (queens.Count == 0) return true;

        // A queen is already placed at this position
        if (queens.Contains(pos)) return false;

        // Check if there is a queen in the same row, column, or one cell diagonally
        return queens.All(q =>
            q.Row != pos.Row && q.Col != pos.Col && (Math.Abs(q.Row - pos.Row) != 1 ||
                                                     Math.Abs(q.Col - pos.Col) != 1));
    }

    private HashSet<Pos> SolveImpl(List<List<Pos>> regions, HashSet<Pos> queens, int regionIndex)
    {
        // Terminal: queens placed in all regions, return the current set of queens
        if (regionIndex >= regions.Count) return queens;

        // Get the current region's cells
        var currentRegion = regions[regionIndex];

        // Try to place a queen in each cell of the current region
        foreach (var pos in currentRegion)
            if (CanPlaceQueen(queens, pos))
            {
                // Place the queen and continue to the next region
                queens.Add(pos);
                var result = SolveImpl(regions, queens, regionIndex + 1);
                if (result.Count == regions.Count)
                    // Found a valid solution
                    return result;
                queens.Remove(pos);
            }

        // No valid placement found in this branch
        return [];
    }
}
