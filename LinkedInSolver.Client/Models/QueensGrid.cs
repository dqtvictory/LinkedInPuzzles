using LinkedInSolver.Client.Models.Solver;

namespace LinkedInSolver.Client.Models;

public class QueensGrid(int size) : Grid(size) {
  private bool[,] isActive = null!;

  protected override void ResetState() {
    solver = new QueensSolver(this);
    isActive = new bool[Size, Size];
  }

  public bool GetIsActive(Pos pos) => isActive[pos.Row, pos.Col];

  public override void OnCellClick(Pos pos) {
    // Queens puzzle logic: toggle cell activation
    isActive[pos.Row, pos.Col] = !isActive[pos.Row, pos.Col];
  }

  public override void OnBorderClick(Pos pos1, Pos pos2) {
    // Queens: no border interactions
    // (border actions are disabled for Queens)
  }

  public override bool HasBorderActions => false;
}
