namespace CreamCheese.ConstraintParser {

  public class StringToken: IToken {

    private string _value;

    public StringToken(string value) {
      _value = value;
    }

    public string Value {
      get {
	return _value;
      }
    }

    TokenType IToken.Type {
      get {
	return TokenType.String;
      }
    }

    public override string ToString() {
      return Value;
    }

  }

}
