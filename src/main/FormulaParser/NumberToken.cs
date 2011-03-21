using System;

namespace FormulaParser {

  public class NumberToken : IPrimitiveToken {

    private double _value;

    public NumberToken(double value) {
      _value = value;
    }

    public PrimitiveType
    Type {
      get {
	return PrimitiveType.Number;
      }
    }

    public int
    CompareTo(object obj) {
      if(obj is IPrimitiveToken) {
        IPrimitiveToken pToken = (IPrimitiveToken) obj;
        switch(pToken.Type) {
        case PrimitiveType.Null:
        case PrimitiveType.Number:
          return _value.CompareTo(pToken.ToDouble());
        case PrimitiveType.Boolean:
        case PrimitiveType.String:
          return -1;
        }
      }
      throw new ArgumentException();
    }

    public bool
    ToBoolean() {
      return Convert.ToBoolean(_value);
    }

    public double
    ToDouble() {
      return _value;
    }

    public override string ToString() {
      return _value.ToString();
    }

  }

}
