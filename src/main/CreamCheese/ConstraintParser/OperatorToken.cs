namespace CreamCheese.ConstraintParser {

  public class OperatorToken: IToken {

    public enum Operator {
      EqualTo,
      GreaterThan,
      LessThan,
      GreaterThanOrEqualTo,
      LessThanOrEqualTo,
      NotEqualTo,
      Plus,
      Minus,
      Negate,
      Multiply,
      Divide,
      Percent,
      Exponentiate,
      Concatenate,
      Range,
      Union,
      Intersect
    }

    private Operator _value;

    public OperatorToken(Operator value) {
      _value = value;
    }

    public Operator Value {
      get {
	return _value;
      }
    }

    TokenType IToken.Type {
      get {
	return TokenType.Operator;
      }
    }

    public override string ToString() {
      switch(Value) {
      case Operator.EqualTo:
	return "=";
      case Operator.GreaterThan:
	return ">";
      case Operator.LessThan:
	return "<";
      case Operator.GreaterThanOrEqualTo:
	return ">=";
      case Operator.LessThanOrEqualTo:
	return "<=";
      case Operator.NotEqualTo:
	return "<>";
      case Operator.Plus:
	return "+";
      case Operator.Minus:
	return "-";
      case Operator.Negate:
	return "-";
      case Operator.Multiply:
	return "*";
      case Operator.Divide:
	return "/";
      case Operator.Percent:
	return "%";
      case Operator.Exponentiate:
	return "^";
      case Operator.Concatenate:
	return "&";
      case Operator.Range:
	return ":";
      case Operator.Union:
	return ",";
      case Operator.Intersect:
	return "!";
      default:
	return "";
      }
    }

  }

}
