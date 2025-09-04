using System.Diagnostics;
using LinkedInSolver.Client.Models.Solver;

namespace LinkedInSolver.Client.Models;

public class SudokuGrid(int size) : Grid(size)
{
    /// <summary>
    ///     Solution to the original state (State) output by the solver. When solution is available,
    ///     0 means the corresponding cell is non-empty in State.
    /// </summary>
    private int[,] _solution = null!;

    /// <summary>
    ///     State array where each cell indicates which number it contains. 0 means empty.
    /// </summary>
    public int[,] State { get; private set; } = null!;

    public override string? ValidateInputSize(int inputSize)
    {
        return inputSize == 6 ? null : "Size must be 6";
    }

    protected override void ResetState()
    {
        Solver = new SudokuSolver(this);

        _solution = new int[Size, Size];
        State = new int[Size, Size];
        for (var row = 0; row < Size; row++)
        for (var col = 0; col < Size; col++)
        {
            // Initialize all cells to 0 (empty)
            _solution[row, col] = 0;
            State[row, col] = 0;
        }
    }

    /// <summary>
    ///     Whether the cell at the specified position has a number assigned to it
    /// </summary>
    public bool HasNumber(Pos pos)
    {
        return GetCellNumber(pos) > 0;
    }

    /// <summary>
    ///     Get the number assigned to the cell at the specified position
    /// </summary>
    public int GetCellNumber(Pos pos)
    {
        return State[pos.Row, pos.Col];
    }

    /// <summary>
    ///     Check if solution is available at the specified position
    /// </summary>
    public bool HasSolutionAt(Pos pos)
    {
        return _solution[pos.Row, pos.Col] > 0;
    }

    /// <summary>
    ///     Get the number at the specified position in the solution
    /// </summary>
    public int GetCellNumberFromSolution(Pos pos)
    {
        return _solution[pos.Row, pos.Col];
    }

    /// <summary>
    ///     Assign a number to the cell at the specified position
    /// </summary>
    public void SetCellNumber(Pos pos, int number)
    {
        State[pos.Row, pos.Col] = number;
    }

    /// <summary>
    ///     Unassign a number to the cell at the specified position
    /// </summary>
    public void ClearCellNumber(Pos pos)
    {
        SetCellNumber(pos, 0);
    }

    /// <summary>
    ///     Parse the solver's solution of the grid to internal final state
    /// </summary>
    /// <param name="solution">Solution to the original state output by the solver</param>
    public void ParseSolution(List<Pos> solution)
    {
        var num = 0;
        foreach (var pos in solution)
            if (pos == Pos.Invalid)
            {
                // Invalid position is the delimiter separating the numbers
                num++;
            }
            else
            {
                // Existing numbers in State must not be in the solution
                Debug.Assert(State[pos.Row, pos.Col] == 0);
                // Solution should not contain duplicated positions
                Debug.Assert(_solution[pos.Row, pos.Col] == 0);

                // Place current number to position
                _solution[pos.Row, pos.Col] = num;
            }
    }
}
