using Cream;
using FormulaParser;

namespace CreamCheese.ConstraintParser {

  public class ConstraintToken: IToken {

    private Constraint _value;

    public ConstraintToken(Constraint value) {
      _value = value;
    }

    public Constraint Value {
      get {
	return _value;
      }
    }

    public override string ToString() {
      return Value.ToString();
    }

  }

}
