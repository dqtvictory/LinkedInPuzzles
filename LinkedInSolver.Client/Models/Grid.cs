using LinkedInSolver.Client.Models.Solver;

namespace LinkedInSolver.Client.Models;

/// <summary>
/// Represents a square grid of cells with a specified size
/// </summary>
public abstract class Grid {
  // Some default constants
  public const int
    DEFAULT_SIZE = 8,
    MIN_SIZE = 4,
    MAX_SIZE = 16;

  /// <summary>
  /// Size of the square grid
  /// </summary>
  public int Size { get; set; } = DEFAULT_SIZE;

  /// <summary>
  /// The solver that will be used to solve the grid
  /// </summary>
  protected PuzzleSolver solver = null!;

  /// <summary>
  /// Construct a new grid with the specified size
  /// </summary>
  public Grid(int size) {
    Reset(size);
  }

  /// <summary>
  /// Resize the grid to a new size then reset its state
  /// </summary>
  /// <param name="newSize"></param>
  public virtual void Reset(int newSize) {
    Size = newSize;
    ResetState();
  }

  /// <summary>
  /// Reset grid's state
  /// </summary>
  protected abstract void ResetState();

  /// <summary>
  /// Handle cell click
  /// </summary>
  /// <param name="pos">Position of cell</param>
  public abstract void OnCellClick(Pos pos);

  /// <summary>
  /// Handle border click between two adjacent cells
  /// </summary>
  /// <param name="pos1">Position of cell 1</param>
  /// <param name="pos2">Position of cell 2</param>
  public abstract void OnBorderClick(Pos pos1, Pos pos2);

  /// <summary>
  /// Validate the grid's current state
  /// </summary>
  /// <returns>Validation message if invalid, null if valid</returns>
  public string? Validate() => solver.Validate();

  /// <summary>
  /// Solve the grid's current state, assuming it is valid
  /// </summary>
  /// <returns>The solution to the current puzzle's grid</returns>
  public List<Pos> Solve() => solver.Solve();

  /// <summary>
  /// Whether the grid should handle border actions
  /// </summary>
  public virtual bool HasBorderActions => true;
}
