using System;
using System.Collections.Generic;

namespace CreamCheese {

  /// <summary>
  ///   A structure to contain necessary cell information.
  /// </summary>
  internal class Cell {

    private string _address;
    private string _formula;
    private Cream.IntVariable _variable;

    /// <summary>
    ///   The cell's CreamCheese address.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     Storing the cell's CreamCheese address prevents reverse lookup
    ///     in the cell dictionary.
    ///   </para>
    /// </remarks>
    public string Address {
      get {
	return _address;
      }
      set {
	_address = value;
      }
    }

    /// <summary>
    ///   The cell's formula.
    /// </summary>
    public string Formula {
      get {
	return _formula;
      }
      set {
	_formula = value;
      }
    }

    /// <summary>
    ///   The cell as a CSP variable.
    /// </summary>
    public Cream.IntVariable Variable {
      get {
	return _variable;
      }
      set {
	_variable = value;
      }
    }

    /// <summary>
    ///   Convenience constructor for Cell with an empty formula.
    /// </summary>
    /// <param name="address">The CreamCheese address to associate with the
    ///   cell.</param>
    public Cell(string address) {
      _address = address;
      _formula = "";
      _variable = null;
    }

    /// <summary>
    ///   Construct a Cell.
    /// </summary>
    /// <param name="address">The cell's CreamCheese address.</param>
    /// <param name="formula">The cell's formula.</param>
    public Cell(string address, string formula)
    : this(address) {
      _formula = formula;
    }

  }

}
