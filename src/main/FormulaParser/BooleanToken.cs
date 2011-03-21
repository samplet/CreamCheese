using System;

namespace FormulaParser {

  public class BooleanToken : IPrimitiveToken {

    private bool _value;

    public BooleanToken(bool value) {
      _value = value;
    }

    public PrimitiveType
    Type {
      get {
	return PrimitiveType.Boolean;
      }
    }

    public int
    CompareTo(object obj) {
      if(obj is IPrimitiveToken) {
        IPrimitiveToken pToken = (IPrimitiveToken) obj;
        switch(pToken.Type) {
        case PrimitiveType.Boolean:
        case PrimitiveType.Null:
          return _value.CompareTo(pToken.ToBoolean());
        case PrimitiveType.Number:
        case PrimitiveType.String:
          return 1;
        }
      }
      throw new ArgumentException();
    }

    public bool
    ToBoolean() {
      return _value;
    }

    public double
    ToDouble() {
      return Convert.ToDouble(_value);
    }

    public override string ToString() {
      return _value.ToString().ToUpper();
    }

  }

}
