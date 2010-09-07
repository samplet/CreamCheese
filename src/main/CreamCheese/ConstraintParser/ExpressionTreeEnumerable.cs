using System.Collections;
using System.Collections.Generic;

namespace CreamCheese.ConstraintParser {

    public class ExpressionTreeEnumerable : IEnumerable<IToken> {

        private ExpressionTree _tree;
        private ExpressionTreeEnumerator.Orders _order;

        public ExpressionTreeEnumerable(ExpressionTreeEnumerator.Orders order, ExpressionTree tree) {
            _order = order;
            _tree = tree;
        }

        public IEnumerator<IToken> GetEnumerator() {
            return (IEnumerator<IToken>) new ExpressionTreeEnumerator(_order, _tree);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return (IEnumerator) this.GetEnumerator();
        }

    }

}
