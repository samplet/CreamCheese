namespace FormulaParser {

  public class RangeToken: IToken {

    private string _value;

    public RangeToken(string value) {
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
