namespace FormulaParser {

  public class IdToken: IToken {

    private string _value;

    public IdToken(string value) {
      _value = value;
    }

    public string Value {
      get {
	return _value;
      }
    }

    public override string ToString() {
      return Value;
    }

  }

}
