namespace CreamCheese.ConstraintParser {

  internal class IdToken: IToken {

    private string _value;

    public IdToken(string value) {
      _value = value;
    }

    public string Value {
      get {
	return _value;
      }
    }

    TokenType IToken.Type {
      get {
	return TokenType.Id;
      }
    }

    public override string ToString() {
      return Value;
    }

  }

}
