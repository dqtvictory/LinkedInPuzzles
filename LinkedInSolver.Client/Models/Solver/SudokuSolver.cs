namespace LinkedInSolver.Client.Models.Solver;

public class SudokuSolver(SudokuGrid grid) : PuzzleSolver
{
    private readonly int _halfSize = grid.Size / 2; // 3
    private readonly int _thirdSize = grid.Size / 3; // 2

    /// <summary>
    ///     The boxes in the grid, used for faster number validation
    /// </summary>
    private int[,] _boxes = null!;

    /// <summary>
    ///     The state of the puzzle grid, frequently updated while solving.
    /// </summary>
    private int[,] _state = null!;

    public override string? Validate()
    {
        _state = grid.State;

        // Populate the boxes value
        _boxes = new int[grid.Size, grid.Size + 1]; // +1 because the puzzle uses 1-based numbering
        for (var row = 0; row < grid.Size; row++)
        for (var col = 0; col < grid.Size; col++)
        {
            var num = _state[row, col];
            if (num == 0)
                continue;
            var boxIndex = GetBoxIndex(new Pos(row, col));
            _boxes[boxIndex, num]++;
        }

        // For each assigned number in the initial state, check that the same number does not appear
        // in the same row, column, or box.
        for (var row = 0; row < grid.Size; row++)
        for (var col = 0; col < grid.Size; col++)
        {
            var pos = new Pos(row, col);
            if (_state[row, col] == 0)
                continue;

            if (!ValidRow(pos))
                return $"Invalid row: {row}";
            if (!ValidCol(pos))
                return $"Invalid column: {col}";
            if (!ValidBox(pos))
                return $"Invalid box at {pos}";
        }

        return null;
    }

    /// <summary>
    ///     Solve the Mini Sudoku puzzle
    /// </summary>
    /// <returns>
    ///     To conform with the common return type (list of positions) of all solvers, the solution
    ///     will contain list of positions for 1's, then 2's, and so on. Between each number, the
    ///     invalid pos is inserted to indicate the end of a number's sequence
    /// </returns>
    public override List<Pos> Solve()
    {
        // Clone the state. We need the original state to validate the solution while solving
        _state = (int[,])grid.State.Clone();
        var finalState = SolveImpl();

        if (finalState == null)
            // If the final state is null, the puzzle is not solvable
            return [];

        var originalState = grid.State;
        var numToPos = new List<List<Pos>>(grid.Size + 1);
        for (var i = 0; i <= grid.Size; i++)
            numToPos.Add(new List<Pos>(grid.Size)); // Each number can appear at most Size times

        for (var row = 0; row < grid.Size; row++)
        for (var col = 0; col < grid.Size; col++)
        {
            var num = finalState[row, col];
            // If the number is not in the original state, it is a solution
            if (originalState[row, col] == 0)
                numToPos[num].Add(new Pos(row, col));
        }

        var result = new List<Pos>();
        foreach (var allPosForNum in numToPos[1..])
        {
            result.Add(Pos.Invalid);
            result.AddRange(allPosForNum);
        }

        return result;
    }

    /// <summary>
    ///     Recursively solve the Mini Sudoku puzzle by backtracking through the grid state
    /// </summary>
    /// <param name="i">Index of the current cell being evaluated in the state array</param>
    /// <returns>The final solved state, or null if not solvable</returns>
    private int[,]? SolveImpl(int i = 0)
    {
        if (i == grid.Size * grid.Size)
            // Terminal condition: all cells are filled, return the final state
            return _state;

        var row = i / grid.Size;
        var col = i % grid.Size;
        if (_state[row, col] != 0)
            // Cell is already filled, move to the next cell
            return SolveImpl(i + 1);

        var pos = new Pos(row, col);
        var boxIndex = GetBoxIndex(pos);

        // Try all possible numbers
        for (var num = 1; num <= grid.Size; num++)
        {
            _state[row, col] = num;
            _boxes[boxIndex, num]++;
            if (ValidRow(pos) && ValidCol(pos) && ValidBox(pos))
            {
                var result = SolveImpl(i + 1);
                if (result != null)
                    return result;
            }

            _boxes[boxIndex, num]--;
        }

        // Solution not possible with current state, backtrack
        _state[pos.Row, pos.Col] = 0;
        return null;
    }

    /// <summary>
    ///     Get the index of the 3x2 box that contains the specified position
    /// </summary>
    private int GetBoxIndex(Pos pos)
    {
        return pos.Row / _thirdSize * _thirdSize + pos.Col / _halfSize;
    }

    private bool ValidRow(Pos pos)
    {
        var num = _state[pos.Row, pos.Col];
        for (var col = 0; col < grid.Size; col++)
            if (col != pos.Col && _state[pos.Row, col] == num)
                return false;

        return true;
    }

    private bool ValidCol(Pos pos)
    {
        var num = _state[pos.Row, pos.Col];
        for (var row = 0; row < grid.Size; row++)
            if (row != pos.Row && _state[row, pos.Col] == num)
                return false;

        return true;
    }

    private bool ValidBox(Pos pos)
    {
        var num = _state[pos.Row, pos.Col];
        var boxIndex = GetBoxIndex(pos);
        // Valid if the number is present only once in the box
        return _boxes[boxIndex, num] == 1;
    }
}
