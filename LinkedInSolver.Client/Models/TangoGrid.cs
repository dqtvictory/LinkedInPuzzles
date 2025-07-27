using LinkedInSolver.Client.Models.Solver;

namespace LinkedInSolver.Client.Models;

public class TangoGrid(int size) : Grid(size) {
  private bool[,] isActive = null!;

  protected override void ResetState() {
    solver = new TangoSolver(this);
    isActive = new bool[Size, Size];
  }

  public bool GetIsActive(Pos pos) => isActive[pos.Row, pos.Col];

  public override void OnCellClick(Pos pos) {
    // Tango puzzle logic: toggle cell activation (for now)
    isActive[pos.Row, pos.Col] = !isActive[pos.Row, pos.Col];
  }

  public override void OnBorderClick(Pos pos1, Pos pos2) {
    // Tango puzzle: border logic (to be implemented)
    // For now, just a placeholder
  }

  public override bool HasBorderActions => true;
}
