using System;
using System.Collections.Generic;

namespace CreamCheese {

    class Cell {

        private string _address;
        private string _formula;
        private Cream.IntVariable _variable;

        public string Address {
            get {
                return _address;
            }
            set {
                _address = value;
            }
        }

        public string Formula {
            get {
                return _formula;
            }
            set {
                _formula = value;
            }
        }

        public Cream.IntVariable Variable {
            get {
                return _variable;
            }
            set {
                _variable = value;
            }
        }

        public Cell(string address) {
            _address = address;
            _formula = "";
            _variable = null;
        }

        public Cell(string address, string formula)
            : this(address) {
            _formula = formula;
        }

    }

}
