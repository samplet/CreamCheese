using System;
using System.Collections.Generic;
using CP = CreamCheese.ConstraintParser;

namespace CreamCheese {

  /// <summary>
  ///   Translates spreadsheet actions into CSPs.
  /// </summary>
  public class CreamCheese {

    /// <summary>
    ///   Remember the given spreadsheet functionality.
    /// </summary>
    private SpreadSheet _spreadSheet;

    /// <summary>
    ///   Used to remember the last solution given by the solver.
    /// </summary>
    private Cream.Solution _lastSolution;

    /// <summary>
    ///   A store of all cells that are considered constrained.
    /// </summary>
    private Dictionary<string, Cell> _cells;

    /// <summary>
    ///   Constructs and initializes a CreamCheese instance.
    /// </summary>
    /// <param name="spreadSheet">An instance of
    ///   <see cref="SpreadSheet"/>.</param>
    public
    CreamCheese(SpreadSheet spreadSheet) {
      _spreadSheet = spreadSheet;
      _lastSolution = null;
      _cells = new Dictionary<string, Cell>();
    }

    /// <summary>
    ///   To be called when a cell is changed.
    /// </summary>
    /// <param name="key">The CreamCheese address of the cell that has
    ///   changed.</param>
    /// <param name="formula">The cell's formula.</param>
    public void
    Change(string key, string formula) {
      if(_cells.ContainsKey(key)) {
	_cells[key].Formula = formula;
	if(!formula.StartsWith("CONSTRAIN")) {
	  _cells.Remove(key);
	}
	_lastSolution = null;
      }
    }

    /// <summary>
    ///   Add a cell to the list of constrained cells.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     Note that this function expects that the cell is making use of
    ///     "CONSTRAIN" function, and will reject a cell that does not.
    ///   </para>
    /// </remarks>
    /// <param name="key">The CreamCheese address of the cell to be
    ///   added.</param>
    /// <param name="formula">The cell's formula.</param>
    public void
    Add(string key, string formula) {
      if(formula.StartsWith("CONSTRAIN")) {
	if(_cells.ContainsKey(key)) {
	  _cells[key].Formula = formula;
	} else {
	  _cells.Add(key, new Cell(key, formula));
	}
	_lastSolution = null;
      }
    }

    /// <summary>
    ///   Remove a cell from the list of constrained cells.
    /// </summary>
    /// <param name="key">The CreamCheese address of the cell to be
    ///   removed.</param>
    public void
    Remove(string key) {
      _cells.Remove(key);
    }

    /// <summary>
    ///   Remove a set of cells from the list of constrained cells based on
    ///   a partial CreamCheese address.
    /// </summary>
    /// <param name="bookKey">The partial address that represents the
    ///   workbook the cell is from.</param>
    public void
    RemoveBook(string bookKey) {
      List<string> cellsToRemove = new List<string>();
      foreach(KeyValuePair<string, Cell> cellKvp in _cells) {
	if(cellKvp.Key.StartsWith(bookKey)) {
	  cellsToRemove.Add(cellKvp.Key);
	}
      }
      foreach(string key in cellsToRemove) {
	_cells.Remove(key);
      }
    }

    /// <summary>
    ///   Remove a set of cells from the list of constrained cells based on
    ///   a partial CreamCheese address.
    /// </summary>
    /// <param name="sheetKey">The partial address that represents the
    ///   worksheet the cell is from.</param>
    public void
    RemoveSheet(string sheetKey) {
      List<string> cellsToRemove = new List<string>();
      foreach(KeyValuePair<string, Cell> cellKvp in _cells) {
	string cellSheetKey =
	  cellKvp.Key.Substring(cellKvp.Key.IndexOf('.') + 1);
	if(cellSheetKey.StartsWith(sheetKey)) {
	  cellsToRemove.Add(cellKvp.Key);
	}
      }
      foreach(string key in cellsToRemove) {
	_cells.Remove(key);
      }
    }

    /// <summary>
    ///   Get the value of a constrained cell.
    /// </summary>
    /// <param name="key">The cell's CreamCheese address.</param>
    /// <returns>
    ///   The assignment given to the cell by the solver, if the assignment
    ///   exists.
    /// </returns>
    public object
    GetValue(string key) {
      if(_lastSolution == null) {
        CalculateValues();
      }
      try {
	int value = _lastSolution.GetIntValue(_cells[key].Variable);
	return (object) value;
      } catch(Exception) {
	return null;
      }
    }

    /// <summary>
    ///   Converts the constrained cells to a CSP, and gets the solution
    ///   from the solver.
    /// </summary>
    private void
    CalculateValues() {
      Cream.Network network = new Cream.Network();
      foreach(KeyValuePair<string, Cell> kvp in _cells) {
	kvp.Value.Variable = new Cream.IntVariable(network, kvp.Key);
      }
      foreach(KeyValuePair<string, Cell> kvp in _cells) {
        CP.ConstraintParser cp =
          new CP.ConstraintParser(_spreadSheet, kvp.Value, _cells, network);
        cp.Parse();
      }
      Cream.Solver solver = new Cream.DefaultSolver(network);
      _lastSolution = solver.FindFirst();
    }

  }

}
