using System.Collections;
using System.Collections.Generic;

namespace CreamCheese.ConstraintParser {

    public class ExpressionTree {

        private ExpressionTreeNode _root;

        public ExpressionTree(ExpressionTreeNode root) {
            _root = root;
        }

        public ExpressionTree(IToken token, ExpressionTree left, ExpressionTree right) {
            _root = new ExpressionTreeNode(token, left.Root, right.Root);
            left.Root.Parent = _root;
            right.Root.Parent = _root;
        }

        public ExpressionTree(IToken token) {
            _root = new ExpressionTreeNode(token);
        }

        public ExpressionTree(string expression) {
            Parser p = new Parser(expression);
            p.Parse();
            this._root = p.Tree.Root;
        }

        public ExpressionTreeNode Root {
            get {
                return _root;
            }
            private set {
                _root = value;
            }
        }

        public ExpressionTreeEnumerable Prefix {
            get {
                return new ExpressionTreeEnumerable(ExpressionTreeEnumerator.Orders.Prefix, this);
            }
        }

        public ExpressionTreeEnumerable Infix {
            get {
                return new ExpressionTreeEnumerable(ExpressionTreeEnumerator.Orders.Infix, this);
            }
        }

        public ExpressionTreeEnumerable Postfix {
            get {
                return new ExpressionTreeEnumerable(ExpressionTreeEnumerator.Orders.Postfix, this);
            }
        }

        public override string ToString() {
            string returnString = "";
            foreach(IToken token in this.Infix) {
                // Remove space before percent sign.
                if(token.Type == TokenType.Operator && ((OperatorToken) token).Value == OperatorToken.Operator.Percent) {
                    returnString = returnString.Trim();
                }
                returnString += token.ToString();
                // Make sure no space comes after negation.
                if(!(token.Type == TokenType.Operator && ((OperatorToken) token).Value == OperatorToken.Operator.Negate)) {
                    returnString += " ";
                }
            }
            return returnString.Trim();
        }

    }

}
