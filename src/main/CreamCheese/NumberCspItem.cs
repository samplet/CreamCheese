namespace CreamCheese {

    class NumberCspItem : ICspItem {

        private int _value;

        public int Value {
            get {
                return _value;
            }
            set {
                _value = value;
            }
        }

        public NumberCspItem(int value) {
            _value = value;
        }

        CspItemType ICspItem.Type {
            get {
                return CspItemType.Number;
            }
        }

    }

}
