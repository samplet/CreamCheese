using Cream;
using FormulaParser;

namespace CreamCheese.ConstraintParser {

  public class VariableToken: IToken {

    private Variable _value;

    public VariableToken(Variable value) {
      _value = value;
    }

    public Variable Value {
      get {
	return _value;
      }
    }

    public override string ToString() {
      return Value.ToString();
    }

  }

}
