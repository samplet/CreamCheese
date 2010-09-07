namespace CreamCheese.ConstraintParser {

  public class UnknownToken: IToken {

    private string _value;

    public UnknownToken(string value) {
      _value = value;
    }

    public string Value {
      get {
	return _value;
      }
    }

    TokenType IToken.Type {
      get {
	return TokenType.Unknown;
      }
    }

    public override string ToString() {
      return Value;
    }

  }

}
