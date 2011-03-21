using System;

namespace FormulaParser {

  public interface IPrimitiveToken : IComparable, IToken {

    PrimitiveType
    Type {
      get;
    }

    bool
    ToBoolean();

    double
    ToDouble();

  }

}
