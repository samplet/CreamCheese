namespace CreamCheese {

    class ConstraintCspItem : ICspItem {

        private Cream.Constraint _value;

        public Cream.Constraint Value {
            get {
                return _value;
            }
            set {
                _value = value;
            }
        }

        public ConstraintCspItem(Cream.Constraint value) {
            _value = value;
        }

        CspItemType ICspItem.Type {
            get {
                return CspItemType.Constraint;
            }
        }

    }

}
