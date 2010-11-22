using Cream;

namespace CreamCheese.ConstraintParser {

  internal class ConstraintToken: IToken {

    private Constraint _value;

    public ConstraintToken(Constraint value) {
      _value = value;
    }

    public Constraint Value {
      get {
	return _value;
      }
    }

    TokenType IToken.Type {
      get {
	return TokenType.Constraint;
      }
    }

    public override string ToString() {
      return Value.ToString();
    }

  }

}
