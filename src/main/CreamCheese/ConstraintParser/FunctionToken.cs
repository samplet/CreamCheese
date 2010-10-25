using System.Collections.Generic;

namespace CreamCheese.ConstraintParser {

  public class FunctionToken: IToken {

    private string _name;
    private List<ExpressionTree> _args;

    public FunctionToken(string name, List<ExpressionTree> args) {
      _name = name;
      _args = args;
    }

    public FunctionToken(string name)
    :this(name, null) {}

    public string Name {
      get {
	return _name;
      }
    }

    public List<ExpressionTree> Args {
      get {
	return _args;
      }
    }

    TokenType IToken.Type {
      get {
	return TokenType.Function;
      }
    }

    public override string ToString() {
      string retString = Name + "(";
      if(Args != null) {
          foreach(ExpressionTree eTree in Args) {
              retString += eTree.ToString() + ", ";
          }
          if(retString.EndsWith(", ")) {
              retString = retString.Remove(retString.Length - 2);
          }
      }
      retString += ")";
      return retString;
    }

  }

}
