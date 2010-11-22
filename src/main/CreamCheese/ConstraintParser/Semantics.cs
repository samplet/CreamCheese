using System;
using System.Collections;
using System.Collections.Generic;
using Cream;

namespace CreamCheese.ConstraintParser {

  internal static class Semantics {

    public static SpreadSheet SpreadSheet;

    public static Cell BaseCell;

    public static Dictionary<string, Cell> Cells;

    public static Network Network;

    public static void
    Constraint(IToken constraint) {
      Console.WriteLine(constraint);
    }

    public static IToken
    Function(string name, params IToken[] args) {
      return new StringToken("f(" + name + ")");
    }

    public static IToken
    InterpretExpression(IToken n, Tokens o) {
      if(n.Type == TokenType.Range) {
        n = ResolveRange((RangeToken) n);
        InterpretExpression(n, o);
      }
      if(n.Type == TokenType.Variable) {
          return InterpretVariableExpression(n, o);
      } else {
        switch(o) {
        case Tokens.PERCENT:
          return Percent(n);
        case Tokens.UMINUS:
          return Negate(n);
        default:
          throw new ArgumentException();
        }
      }
    }

    public static IToken
    InterpretExpression(IToken x, IToken y, Tokens o) {
      if(x.Type == TokenType.Range) {
        x = ResolveRange((RangeToken) x);
        InterpretExpression(x, y, o);
      }
      if(y.Type == TokenType.Range) {
        y = ResolveRange((RangeToken) y);
        InterpretExpression(x, y, o);
      }
      if(x.Type == TokenType.Variable
         || y.Type == TokenType.Variable) {
          return InterpretVariableExpression(x, y, o);
      } else {
        switch(o) {
        case Tokens.EQ:
          return EqualTo(x, y);
        case Tokens.NEQ:
          return NotEqualTo(x, y);
        case Tokens.GT:
          return GreaterThan(x, y);
        case Tokens.GEQ:
          return GreaterThanOrEqualTo(x, y);
        case Tokens.LT:
          return LessThan(x, y);
        case Tokens.LEQ:
          return LessThanOrEqualTo(x, y);
        case Tokens.PLUS:
          return Plus(x, y);
        case Tokens.MINUS:
          return Subtract(x, y);
        case Tokens.MULT:
          return Multiply(x, y);
        case Tokens.DIV:
          return Divide(x, y);
        case Tokens.EXP:
          return Exponentiate(x, y);
        case Tokens.CONCAT:
          return Concatenate(x, y);
        default:
          throw new ArgumentException();
        }
      }
    }


    private static int
    Compare(IToken x, IToken y) {
      if(x.Type != y.Type) {
        switch(x.Type) {
        case TokenType.Boolean:
          return 1;
        case TokenType.String:
          if(y.Type == TokenType.Number) {
            return 1;
          } else {
            return -1;
          }
        case TokenType.Number:
          return -1;
        default:
          throw new ArgumentException();
        }
      } else {
        switch(x.Type) {
        case TokenType.Boolean:
          return
            ((BooleanToken) x).Value.CompareTo(((BooleanToken) y).Value);
        case TokenType.String:
          return ((StringToken) x).Value.CompareTo(((StringToken) y).Value);
        case TokenType.Number:
          return ((NumberToken) x).Value.CompareTo(((NumberToken) y).Value);
        default:
          throw new ArgumentException();
        }
      }
    }

    private static IToken
    Concatenate(IToken x, IToken y) {
      // TODO: Type checking
      return new StringToken(x.ToString() + y.ToString());
    }

    private static double
    ConvertTokenToDouble(IToken n) {
      double nv;
      switch(n.Type) {
      case TokenType.Boolean:
        nv = Convert.ToDouble(((BooleanToken) n).Value);
        break;
      case TokenType.String:
        try {
          nv = Convert.ToDouble(((StringToken) n).Value);
        } catch(Exception) {
          throw;
        }
        break;
      case TokenType.Number:
        nv = ((NumberToken) n).Value;
        break;
      default:
        throw new ArgumentException();
      }
      return nv;
    }

    private static Variable
    ConvertTokenToVariable(IToken n) {
      Variable v;
      switch(n.Type) {
      case TokenType.Variable:
        v = ((VariableToken) n).Value;
        break;
      case TokenType.Number:
        try {
          int i = Convert.ToInt32(((NumberToken) n).Value);
          v = new IntVariable(Network, i, i.ToString());
        } catch(Exception) {
          throw;
        }
        break;
      default:
        throw new ArgumentException();
      }
      return v;
    }

    private static IToken
    Divide(IToken x, IToken y) {
      double xv = ConvertTokenToDouble(x);
      double yv = ConvertTokenToDouble(y);
      return new NumberToken(xv / yv);
    }

    private static IToken
    EqualTo(IToken x, IToken y) {
      int comp = Compare(x, y);
      if(comp == 0) {
        return new BooleanToken(true);
      } else {
        return new BooleanToken(false);
      }
    }

    private static IToken
    Exponentiate(IToken x, IToken y) {
      double xv = ConvertTokenToDouble(x);
      double yv = ConvertTokenToDouble(y);
      return new NumberToken(Math.Pow(xv, yv));
    }

    private static IToken
    GreaterThan(IToken x, IToken y) {
      int comp = Compare(x, y);
      if(comp > 0) {
        return new BooleanToken(true);
      } else {
        return new BooleanToken(false);
      }
    }

    private static IToken
    GreaterThanOrEqualTo(IToken x, IToken y) {
      return new BooleanToken(!((BooleanToken) LessThan(x, y)).Value);
    }

    private static IToken
    InterpretVariableExpression(IToken n, Tokens o) {
      Variable vr;
      Variable vn = ConvertTokenToVariable(n);
      switch(o) {
      case Tokens.UMINUS:
        vr = new IntVariable(Network, "-" + vn.ToString());
        new IntFunc(Network, IntFunc.Negate, vr, vn);
        return new VariableToken(vr);
      default:
        throw new ArgumentException();
      }
    }

    private static IToken
    InterpretVariableExpression(IToken x, IToken y, Tokens o) {
      Variable vx = ConvertTokenToVariable(x);
      Variable vy = ConvertTokenToVariable(y);
      Variable vr;
      string vname;
      Constraint c;
      switch(o) {
      case Tokens.EQ:
        return new ConstraintToken(new Equals(Network, vx, vy));
      case Tokens.NEQ:
        return new ConstraintToken(new NotEquals(Network, vx, vy));
      case Tokens.GT:
        c = new IntComparison(Network, IntComparison.Gt, vx, vy);
        return new ConstraintToken(c);
      case Tokens.GEQ:
        c = new IntComparison(Network, IntComparison.Ge, vx, vy);
        return new ConstraintToken(c);
      case Tokens.LT:
        c = new IntComparison(Network, IntComparison.Lt, vx, vy);
        return new ConstraintToken(c);
      case Tokens.LEQ:
        c = new IntComparison(Network, IntComparison.Le, vx, vy);
        return new ConstraintToken(c);
      case Tokens.PLUS:
        vname = "(" + vx.ToString() + " + " + vy.ToString() + ")";
        vr = new IntVariable(Network, vname);
        new IntArith(Network, IntArith.Add, vr, vx, vy);
        return new VariableToken(vr);
      case Tokens.MINUS:
        vname = "(" + vx.ToString() + " - " + vy.ToString() + ")";
        vr = new IntVariable(Network, vname);
        new IntArith(Network, IntArith.Subtract, vr, vx, vy);
        return new VariableToken(vr);
      case Tokens.MULT:
        vname = "(" + vx.ToString() + " * " + vy.ToString() + ")";
        vr = new IntVariable(Network, vname);
        new IntArith(Network, IntArith.MULTIPLY, vr, vx, vy);
        return new VariableToken(vr);
      default:
        throw new ArgumentException();
      }
    }

    private static IToken
    LessThan(IToken x, IToken y) {
      int comp = Compare(x, y);
      if(comp < 0) {
        return new BooleanToken(true);
      } else {
        return new BooleanToken(false);
      }
    }

    private static IToken
    LessThanOrEqualTo(IToken x, IToken y) {
      return new BooleanToken(!((BooleanToken) GreaterThan(x, y)).Value);
    }

    private static IToken
    Multiply(IToken x, IToken y) {
      double xv = ConvertTokenToDouble(x);
      double yv = ConvertTokenToDouble(y);
      return new NumberToken(xv * yv);
    }

    private static IToken
    Negate(IToken n) {
      return Multiply(n, new NumberToken(-1));
    }

    private static IToken
    NotEqualTo(IToken x, IToken y) {
      return new BooleanToken(!((BooleanToken) EqualTo(x, y)).Value);
    }

    private static IToken
    Percent(IToken n) {
      return Multiply(n, new NumberToken(0.01));
    }

    private static IToken
    Plus(IToken x, IToken y) {
      double xv = ConvertTokenToDouble(x);
      double yv = ConvertTokenToDouble(y);
      return new NumberToken(xv + yv);
    }

    private static IToken
    ResolveRange(RangeToken r) {
      object range = SpreadSheet.GetCellValue(BaseCell.Address, r.Value);
      if(range.GetType().IsArray) {
        throw new ArgumentException();
      }
      string key = SpreadSheet.ConvertAddress(BaseCell.Address, r.Value);
      if(Cells.ContainsKey(key)) {
        return new VariableToken(Cells[key].Variable);
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

    private static IToken
    Subtract(IToken x, IToken y) {
      double xv = ConvertTokenToDouble(x);
      double yv = ConvertTokenToDouble(y);
      return new NumberToken(xv - yv);
    }

  }

}
