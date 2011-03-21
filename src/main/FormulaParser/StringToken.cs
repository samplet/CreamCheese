using System;

namespace FormulaParser {

  public class StringToken : IPrimitiveToken {

    private string _value;

    public StringToken(string value) {
      _value = value;
    }

    public PrimitiveType
    Type {
      get {
	return PrimitiveType.String;
      }
    }

    public int
    CompareTo(object obj) {
      if(obj is IPrimitiveToken) {
        IPrimitiveToken pToken = (IPrimitiveToken) obj;
        switch(pToken.Type) {
        case PrimitiveType.Null:
        case PrimitiveType.String:
          return _value.CompareTo(pToken.ToString());
        case PrimitiveType.Boolean:
          return -1;
        case PrimitiveType.Number:
          return 1;
        }
      }
      throw new ArgumentException();
    }

    public bool
    ToBoolean() {
      if(_value.ToUpper() == "TRUE") {
        return true;
      } else if(_value.ToUpper() == "FALSE") {
        return false;
      } else {
        throw new FormatException();
      }
    }

    public double
    ToDouble() {
      return Convert.ToDouble(_value);
    }

    public override string ToString() {
      return _value;
    }

  }

}
