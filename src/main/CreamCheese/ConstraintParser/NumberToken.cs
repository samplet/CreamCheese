namespace CreamCheese.ConstraintParser {

  internal class NumberToken: IToken {

    private double _value;

    public NumberToken(double value) {
      _value = value;
    }

    public double Value {
      get {
	return _value;
      }
    }

    TokenType IToken.Type {
      get {
	return TokenType.Number;
      }
    }

    public override string ToString() {
      return Value.ToString();
    }

  }

}
