using System;

namespace FormulaParser {

  public class NullToken : IPrimitiveToken {

    public PrimitiveType
    Type {
      get {
	return PrimitiveType.Null;
      }
    }

    public int
    CompareTo(object obj) {
      if(obj is IPrimitiveToken) {
        IPrimitiveToken pToken = (IPrimitiveToken) obj;
        switch(pToken.Type) {
        case PrimitiveType.Null:
          return 0;
        case PrimitiveType.String:
          return ToString().CompareTo(pToken.ToString());
        case PrimitiveType.Boolean:
          return ToBoolean().CompareTo(pToken.ToBoolean());
        case PrimitiveType.Number:
          return ToDouble().CompareTo(pToken.ToDouble());
        }
      }
      throw new ArgumentException();
    }

    public bool
    ToBoolean() {
      return false;
    }

    public double
    ToDouble() {
      return 0.0;
    }

    public override string ToString() {
      return "";
    }

  }

}
