namespace CreamCheese.ConstraintParser {

  internal class BooleanToken: IToken {

    private bool _value;

    public BooleanToken(bool value) {
      _value = value;
    }

    public bool Value {
      get {
	return _value;
      }
    }

    TokenType IToken.Type {
      get {
	return TokenType.Boolean;
      }
    }

    public override string ToString() {
      return Value.ToString().ToUpper();
    }

  }

}
