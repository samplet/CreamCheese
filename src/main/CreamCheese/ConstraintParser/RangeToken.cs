namespace CreamCheese.ConstraintParser {

  internal class RangeToken: IToken {

    private string _value;

    public RangeToken(string value) {
      _value = value;
    }

    public string Value {
      get {
	return _value;
      }
    }

    TokenType IToken.Type {
      get {
	return TokenType.Range;
      }
    }

    public override string ToString() {
      return Value;
    }

  }

}
