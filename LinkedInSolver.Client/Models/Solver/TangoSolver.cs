namespace LinkedInSolver.Client.Models.Solver;

public class TangoSolver(TangoGrid grid) : PuzzleSolver
{
    private readonly int[] _colMoonCount = new int[grid.Size];
    private readonly int[] _colSunCount = new int[grid.Size];

    private readonly int[] _rowMoonCount = new int[grid.Size];
    private readonly int[] _rowSunCount = new int[grid.Size];

    /// <summary>
    ///     The state of the Tango puzzle grid, frequently updated during the solving process
    /// </summary>
    private TangoGrid.CellType[,] _state = null!;

    public override string? Validate()
    {
        // Size of grid must be 6 by 6
        if (grid.Size != 6)
            return $"Size of grid must be 6, but was {grid.Size}";

        if (!ValidateNumMoonsAndSuns())
            return "Each row and column must not contain more than 3 Moons or 3 Suns";

        if (!ValidateAdjacentMoonsOrSun())
            return "Found 3 adjacent Moons or Suns in a row or column";

        if (!ValidateBorders())
            return "Puzzle cannot be solved due to invalid border state";

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
        // Clone the state. We need the original state to validate the solution while solving
        _state = (TangoGrid.CellType[,])grid.Cells.Clone();
        var finalState = SolveImpl();

        if (finalState == null)
            // If the final state is null, the puzzle is not solvable
            return [];

        // Construct the list of positions where Moons are newly placed
        var result = new List<Pos>();
        for (var row = 0; row < grid.Size; row++)
        for (var col = 0; col < grid.Size; col++)
            if (finalState[row, col] == TangoGrid.CellType.Moon &&
                grid.Cells[row, col] == TangoGrid.CellType.Empty)
                result.Add(new Pos(row, col));

        return result;
    }

    /// <summary>
    ///     Check that each row and column must not contain more than 3 Moons or 3 Suns
    /// </summary>
    private bool ValidateNumMoonsAndSuns()
    {
        // Check that each row and column must not contain more than 3 Moons or 3 Suns
        for (var i = 0; i < grid.Size; i++)
        {
            var (rowMoonCount, rowSunCount) = (0, 0);
            var (colMoonCount, colSunCount) = (0, 0);
            for (var j = 0; j < grid.Size; j++)
            {
                rowMoonCount += grid.Cells[i, j] == TangoGrid.CellType.Moon ? 1 : 0;
                rowSunCount += grid.Cells[i, j] == TangoGrid.CellType.Sun ? 1 : 0;
                colMoonCount += grid.Cells[j, i] == TangoGrid.CellType.Moon ? 1 : 0;
                colSunCount += grid.Cells[j, i] == TangoGrid.CellType.Sun ? 1 : 0;

                if (rowMoonCount > 3 || rowSunCount > 3 ||
                    colMoonCount > 3 || colSunCount > 3)
                    return false;
            }

            // Update the counts for the current row and column
            _rowMoonCount[i] = rowMoonCount;
            _rowSunCount[i] = rowSunCount;
            _colMoonCount[i] = colMoonCount;
            _colSunCount[i] = colSunCount;
        }

        return true;
    }

    /// <summary>
    ///     Check that no 3 adjacent cells in a row or column contain all Moons or all Suns
    /// </summary>
    private bool ValidateAdjacentMoonsOrSun()
    {
        // Validate rows
        for (var row = 0; row < grid.Size; row++)
        for (var col = 0; col < grid.Size - 2; col++)
        {
            var cellType1 = grid.Cells[row, col];
            var cellType2 = grid.Cells[row, col + 1];
            var cellType3 = grid.Cells[row, col + 2];

            if (cellType1 == cellType2 && cellType1 == cellType3 &&
                cellType1 != TangoGrid.CellType.Empty)
                return false;
        }

        // Validate columns
        for (var col = 0; col < grid.Size; col++)
        for (var row = 0; row < grid.Size - 2; row++)
        {
            var cellType1 = grid.Cells[row, col];
            var cellType2 = grid.Cells[row + 1, col];
            var cellType3 = grid.Cells[row + 2, col];

            if (cellType1 == cellType2 && cellType1 == cellType3 &&
                cellType1 != TangoGrid.CellType.Empty)
                return false;
        }

        return true;
    }

    /// <summary>
    ///     Check that adjacent cells of an Equal border have the same type, and adjacent cells of
    ///     an Opposite border have different types. Also check that two adjacent borders are not
    ///     both Equal which would force three adjacent cells to have the same type
    /// </summary>
    private bool ValidateBorders()
    {
        foreach (var ((pos1, pos2), borderType) in grid.Borders)
        {
            var cellType1 = grid.Cells[pos1.Row, pos1.Col];
            var cellType2 = grid.Cells[pos2.Row, pos2.Col];

            switch (borderType)
            {
                case TangoGrid.BorderType.Equal
                    // Adjacent cells of an Equal border have the different type
                    when (cellType1 != TangoGrid.CellType.Empty &&
                          cellType2 != TangoGrid.CellType.Empty && cellType1 != cellType2) ||
                         // Or consecutive Equal borders
                         !ValidateAdjacentBordersNotEqual(pos1, pos2):
                // Flow through to return false
                case TangoGrid.BorderType.Opposite
                    // Adjacent cells of an Opposite border have the same type
                    when cellType1 == cellType2 && cellType1 != TangoGrid.CellType.Empty:
                    return false;
            }
        }

        return true;
    }

    /// <summary>
    ///     Check that two adjacent borders are not both Equal which would force three adjacent
    ///     cells to have the same type
    /// </summary>
    private bool ValidateAdjacentBordersNotEqual(Pos pos1, Pos pos2)
    {
        // Determine the direction of the border
        var forwardDir = pos2 - pos1;
        var backwardDir = forwardDir switch
        {
            { Row: 0, Col: 1 } => new Pos(0, -1), // Right -> Left
            { Row: 1, Col: 0 } => new Pos(-1, 0), // Down -> Up
            _ => throw new ArgumentException("Invalid border direction")
        };

        return
            // Forward border is either not Equal or does not exist
            !(grid.Borders.TryGetValue((pos2, pos2 + forwardDir), out var forwardBorder) &&
              forwardBorder == TangoGrid.BorderType.Equal)
            // Backward border is either not Equal or does not exist
            && !(grid.Borders.TryGetValue((pos1 + backwardDir, pos1), out var backwardBorder) &&
                 backwardBorder == TangoGrid.BorderType.Equal);
    }

    /// <summary>
    ///     Recursively solve the Tango puzzle by backtracking through the grid state
    /// </summary>
    /// <param name="i">Index of the current cell being evaluated in the state array</param>
    /// <returns>The final solved state, or null if not solvable</returns>
    private TangoGrid.CellType[,]? SolveImpl(int i = 0)
    {
        if (i == grid.Size * grid.Size)
            // Terminal condition: all cells are filled, return the final state
            return _state;

        var row = i / grid.Size;
        var col = i % grid.Size;
        var originalCellType = grid.Cells[row, col];

        if (originalCellType != TangoGrid.CellType.Empty)
            // Cell is pre-placed. Cannot modify, move to the next cell
            return SolveImpl(i + 1);

        // Try placing a Moon or a Sun in the current cell
        _state[row, col] = TangoGrid.CellType.Moon;
        _rowMoonCount[row]++;
        _colMoonCount[col]++;
        if (ValidateCellPlacement(row, col))
        {
            // If placing a Moon is valid, continue to the next cell
            var result = SolveImpl(i + 1);
            if (result != null)
                return result;
        }

        _rowMoonCount[row]--;
        _colMoonCount[col]--;

        _state[row, col] = TangoGrid.CellType.Sun;
        _rowSunCount[row]++;
        _colSunCount[col]++;
        if (ValidateCellPlacement(row, col))
        {
            // If placing a Sun is valid, continue to the next cell
            var result = SolveImpl(i + 1);
            if (result != null)
                return result;
        }

        _rowSunCount[row]--;
        _colSunCount[col]--;

        // If neither placement was valid, reset the cell to Empty and backtrack
        _state[row, col] = TangoGrid.CellType.Empty;
        return null;
    }

    /// <summary>
    ///     Validate whether the current cell placement at (row, col) respects the Tango puzzle
    ///     rules
    /// </summary>
    private bool ValidateCellPlacement(int row, int col)
    {
        // Check that the current row and column do not exceed 3 Moons or Suns
        if (_rowMoonCount[row] > 3 || _rowSunCount[row] > 3 ||
            _colMoonCount[col] > 3 || _colSunCount[col] > 3)
            return false;

        // Check whether current cell placement respect the border rules
        var pos = new Pos(row, col);
        if (pos.GetNeighbors().Any(neighbor =>
                grid.IsInBounds(neighbor) && !ValidateAdjacentCellsForPlacement(pos, neighbor)))
            return false;

        for (var r = Math.Max(row - 2, 0); r <= Math.Min(grid.Size - 3, row); r++)
        {
            // Check this column for 3 adjacent Moons or Suns
            var cellType1 = _state[r, col];
            var cellType2 = _state[r + 1, col];
            var cellType3 = _state[r + 2, col];
            if (cellType1 == cellType2 && cellType1 == cellType3 &&
                cellType1 != TangoGrid.CellType.Empty)
                return false;
        }

        for (var c = Math.Max(col - 2, 0); c <= Math.Min(grid.Size - 3, col); c++)
        {
            // Check this row for 3 adjacent Moons or Suns
            var cellType1 = _state[row, c];
            var cellType2 = _state[row, c + 1];
            var cellType3 = _state[row, c + 2];
            if (cellType1 == cellType2 && cellType1 == cellType3 &&
                cellType1 != TangoGrid.CellType.Empty)
                return false;
        }

        return true;
    }

    /// <summary>
    ///     Validate two adjacent cells for placement
    /// </summary>
    private bool ValidateAdjacentCellsForPlacement(Pos pos1, Pos pos2)
    {
        var sortedPair = Pos.GetSortedPair(pos1, pos2);
        if (!grid.Borders.TryGetValue(sortedPair, out var borderType))
            // No border between the cells, so they can be of any type
            return true;
        var cellType1 = _state[pos1.Row, pos1.Col];
        var cellType2 = _state[pos2.Row, pos2.Col];
        return
            // One of the cells is empty, so placement is always valid
            cellType1 == TangoGrid.CellType.Empty ||
            cellType2 == TangoGrid.CellType.Empty ||
            // If there is a border type between the cells, placement must respect the border rules
            (borderType == TangoGrid.BorderType.Equal && cellType1 == cellType2) ||
            (borderType == TangoGrid.BorderType.Opposite && cellType1 != cellType2)
            ;
    }
}
