namespace CreamCheese {

    class VariableCspItem : ICspItem {

        private Cream.IntVariable _value;

        public Cream.IntVariable Value {
            get {
                return _value;
            }
            set {
                _value = value;
            }
        }

        public VariableCspItem(Cream.IntVariable value) {
            _value = value;
        }

        CspItemType ICspItem.Type {
            get {
                return CspItemType.Variable;
            }
        }

    }

}
