using Cream;

namespace CreamCheese.ConstraintParser {

  internal class VariableToken: IToken {

    private Variable _value;

    public VariableToken(Variable value) {
      _value = value;
    }

    public Variable Value {
      get {
	return _value;
      }
    }

    TokenType IToken.Type {
      get {
	return TokenType.Variable;
      }
    }

    public override string ToString() {
      return Value.ToString();
    }

  }

}
