namespace CreamCheese.ConstraintParser {

  public class NumberToken: IToken {

    private int _value;

    public NumberToken(int value) {
      _value = value;
    }

    public int Value {
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
