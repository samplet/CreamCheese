namespace CreamCheese.ConstraintParser {

    public class ExpressionTreeNode {

        private IToken _token;
        private ExpressionTreeNode _parent;
        private ExpressionTreeNode _left;
        private ExpressionTreeNode _right;

        public ExpressionTreeNode(IToken token, ExpressionTreeNode parent, ExpressionTreeNode left, ExpressionTreeNode right) {
            Token = token;
            Parent = parent;
            Left = left;
            Right = right;
        }

        public ExpressionTreeNode(IToken token, ExpressionTreeNode parent)
            : this(token, parent, null, null) {
        }

        public ExpressionTreeNode(IToken token, ExpressionTreeNode left, ExpressionTreeNode right)
            : this(token, null, left, right) {
        }

        public ExpressionTreeNode(IToken token)
            : this(token, null, null, null) {
        }

        public IToken Token {
            get {
                return _token;
            }
            set {
                _token = value;
            }
        }

        public ExpressionTreeNode Parent {
            get {
                return _parent;
            }
            set {
                _parent = value;
            }
        }

        public ExpressionTreeNode Left {
            get {
                return _left;
            }
            set {
                _left = value;
            }
        }

        public ExpressionTreeNode Right {
            get {
                return _right;
            }
            set {
                _right = value;
            }
        }

        public override string ToString() {
            return Token.ToString();
        }

    }

}
