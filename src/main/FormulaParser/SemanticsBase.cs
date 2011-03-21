using System;
using System.Collections;
using System.Collections.Generic;

namespace FormulaParser {

  public abstract class SemanticsBase {

    public IToken
    Function(string name, params IToken[] args) {
      return new StringToken("f(" + name + ")");
    }

    public IToken
    InterpretExpression(IToken n) {
      if(n is RangeToken) {
        n = ResolveRange((RangeToken) n);
      }
      return n;
    }

    public IToken
    InterpretExpression(IToken n, Tokens o) {
      n = InterpretExpression(n);
      if(n is IPrimitiveToken) {
        return EvaluateOperation((IPrimitiveToken) n, o);
      } else {
        throw new ArgumentException();
      }
    }

    public IToken
    InterpretExpression(IToken x, IToken y, Tokens o) {
      x = InterpretExpression(x);
      y = InterpretExpression(y);
      if(x is IPrimitiveToken && y is IPrimitiveToken) {
        return EvaluateOperation((IPrimitiveToken) x,
                                 (IPrimitiveToken) y, o);
      } else {
        throw new ArgumentException();
      }
    }

    protected IPrimitiveToken
    EvaluateOperation(IPrimitiveToken x, Tokens o) {
      switch(o) {
      case Tokens.PERCENT:
        return new NumberToken(x.ToDouble() * 0.01);
      case Tokens.UMINUS:
        return new NumberToken(x.ToDouble() * -1);
      default:
        throw new ArgumentException("Unknown Operator");
      }
    }

    protected IPrimitiveToken
    EvaluateOperation(IPrimitiveToken x, IPrimitiveToken y, Tokens o) {
      switch(o) {
      case Tokens.EQ:
        return new BooleanToken(x.CompareTo(y) == 0);
      case Tokens.NEQ:
        return new BooleanToken(x.CompareTo(y) != 0);
      case Tokens.GT:
        return new BooleanToken(x.CompareTo(y) > 0);
      case Tokens.GEQ:
        return new BooleanToken(x.CompareTo(y) >= 0);
      case Tokens.LT:
        return new BooleanToken(x.CompareTo(y) < 0);
      case Tokens.LEQ:
        return new BooleanToken(x.CompareTo(y) <= 0);
      case Tokens.PLUS:
        return new NumberToken(x.ToDouble() + y.ToDouble());
      case Tokens.MINUS:
        return new NumberToken(x.ToDouble() - y.ToDouble());
      case Tokens.MULT:
        return new NumberToken(x.ToDouble() * y.ToDouble());
      case Tokens.DIV:
        return new NumberToken(x.ToDouble() / y.ToDouble());
      case Tokens.EXP:
        return new NumberToken(Math.Pow(x.ToDouble(), y.ToDouble()));
      case Tokens.CONCAT:
        return new StringToken(x.ToString() + y.ToString());
      default:
        throw new ArgumentException("Unknown Operator");
      }
    }

    protected abstract IToken
    ResolveRange(RangeToken r);

  }

}
