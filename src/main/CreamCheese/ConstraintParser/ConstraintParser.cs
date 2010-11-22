using System;
using System.Collections.Generic;
using Cream;

namespace CreamCheese.ConstraintParser {

  internal class ConstraintParser {

    private SpreadSheet _spreadSheet;

    private Cell _baseCell;

    private Dictionary<string, Cell> _cells;

    private Network _network;

    public
    ConstraintParser(SpreadSheet spreadSheet, Cell baseCell,
                     Dictionary<string, Cell> cells, Network network) {
      _spreadSheet = spreadSheet;
      _baseCell = baseCell;
      _cells = cells;
      _network = network;
    }

    public bool
    Parse() {
      string constraints;
      if(_baseCell.Formula.StartsWith("CONSTRAIN(")
         && _baseCell.Formula.EndsWith(")")) {
        constraints = _baseCell.Formula.Substring(10);
        constraints = constraints.Substring(0, constraints.Length - 1);
      } else {
        throw new ArgumentException();
      }
      Parser p = new Parser(constraints);
      Semantics.SpreadSheet = _spreadSheet;
      Semantics.BaseCell = _baseCell;
      Semantics.Cells = _cells;
      Semantics.Network = _network;
      return p.Parse();
    }

  }

}
