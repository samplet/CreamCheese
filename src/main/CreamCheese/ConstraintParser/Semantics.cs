using System;
using System.Collections.Generic;
using FormulaParser;
using Cream;

namespace CreamCheese.ConstraintParser {

  internal class Semantics : SemanticsBase {

    private Cell _baseCell;
    private Dictionary<string, Cell> _cells;
    private Network _network;
    private SpreadSheet _spreadSheet;

    public
    Semantics(Cell baseCell, Dictionary<string, Cell> cells,
              Network network, SpreadSheet spreadSheet) {
      _baseCell = baseCell;
      _cells = cells;
      _network = network;
      _spreadSheet = spreadSheet;
    }

    private Variable
    ConvertTokenToVariable(IToken n) {
      if(n is VariableToken) {
        return ((VariableToken) n).Value;
      } else if(n is IPrimitiveToken) {
        try {
          int i = Convert.ToInt32(((IPrimitiveToken) n).ToDouble());
          return new IntVariable(_network, i, i.ToString());
        } catch(Exception) {
          throw;
        }
      } else {
        throw new ArgumentException();
      }
    }

    private IToken
    EvaluateVariableOperation(IToken n, Tokens o) {
      Variable vr;
      Variable vn = ConvertTokenToVariable(n);
      switch(o) {
      case Tokens.UMINUS:
        vr = new IntVariable(_network, "-" + vn.ToString());
        new IntFunc(_network, IntFunc.Negate, vr, vn);
        return new VariableToken(vr);
      default:
        throw new ArgumentException();
      }
    }

    private IToken
    EvaluateVariableOperation(IToken x, IToken y, Tokens o) {
      Variable vx = ConvertTokenToVariable(x);
      Variable vy = ConvertTokenToVariable(y);
      Variable vr;
      string vname;
      Constraint c;
      switch(o) {
      case Tokens.EQ:
        return new ConstraintToken(new Equals(_network, vx, vy));
      case Tokens.NEQ:
        return new ConstraintToken(new NotEquals(_network, vx, vy));
      case Tokens.GT:
        c = new IntComparison(_network, IntComparison.Gt, vx, vy);
        return new ConstraintToken(c);
      case Tokens.GEQ:
        c = new IntComparison(_network, IntComparison.Ge, vx, vy);
        return new ConstraintToken(c);
      case Tokens.LT:
        c = new IntComparison(_network, IntComparison.Lt, vx, vy);
        return new ConstraintToken(c);
      case Tokens.LEQ:
        c = new IntComparison(_network, IntComparison.Le, vx, vy);
        return new ConstraintToken(c);
      case Tokens.PLUS:
        vname = "(" + vx.ToString() + " + " + vy.ToString() + ")";
        vr = new IntVariable(_network, vname);
        new IntArith(_network, IntArith.Add, vr, vx, vy);
        return new VariableToken(vr);
      case Tokens.MINUS:
        vname = "(" + vx.ToString() + " - " + vy.ToString() + ")";
        vr = new IntVariable(_network, vname);
        new IntArith(_network, IntArith.Subtract, vr, vx, vy);
        return new VariableToken(vr);
      case Tokens.MULT:
        vname = "(" + vx.ToString() + " * " + vy.ToString() + ")";
        vr = new IntVariable(_network, vname);
        new IntArith(_network, IntArith.MULTIPLY, vr, vx, vy);
        return new VariableToken(vr);
      default:
        throw new ArgumentException();
      }
    }

    protected override IToken
    ResolveRange(RangeToken r) {
      string key = _spreadSheet.ConvertAddress(_baseCell.Address, r.Value);
      if(_cells.ContainsKey(key)) {
        return new VariableToken(_cells[key].Variable);
      }
      object range = _spreadSheet.GetCellValue(_baseCell.Address, key);
      if(range.GetType().IsArray) {
        throw new ArgumentException();
      }
      if(range.GetType() == typeof(bool)) {
        return new BooleanToken((bool) range);
      } else if(range.GetType() == typeof(string)) {
        return new StringToken((string) range);
      } else if(range.GetType() == typeof(double)) {
        return new NumberToken((double) range);
      } else {
        throw new ArgumentException();
      }
    }

  }

}
