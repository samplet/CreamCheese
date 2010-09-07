using System;
using System.Collections;
using System.Collections.Generic;

namespace CreamCheese.ConstraintParser {

    public class ExpressionTreeEnumerator : IEnumerator<IToken> {

        public enum Orders {
            Prefix,
            Infix,
            Postfix
        }

        private Orders _order;
        private ExpressionTree _tree;
        private ExpressionTreeNode _current;
        private bool _reset;

        public ExpressionTreeEnumerator(Orders order, ExpressionTree tree) {
            _order = order;
            _tree = tree;
            this.Reset();
        }

        public IToken Current {
            get {
                return _current.Token;
            }
        }

        object IEnumerator.Current {
            get {
                return (object) Current;
            }
        }

        void IDisposable.Dispose() {
        }

        public bool MoveNext() {
            if(_reset) {
                _reset = false;
                if(_current == null) {
                    return false;
                } else {
                    return true;
                }
            } else {
                switch(_order) {
                    case Orders.Prefix:
                        if(_current.Left != null) {
                            _current = _current.Left;
                            return true;
                        } else {
                            while(_current.Parent != null && _current.Parent.Right == _current) {
                                _current = _current.Parent;
                            }
                            if(_current.Parent != null) {
                                _current = _current.Parent;
                                return true;
                            } else {
                                return false;
                            }
                        }
                    case Orders.Infix:
                        if(_current.Right != null) {
                            _current = _current.Right;
                            while(_current.Left != null) {
                                _current = _current.Left;
                            }
                            return true;
                        } else {
                            while(_current.Parent != null && _current.Parent.Right == _current) {
                                _current = _current.Parent;
                            }
                            if(_current.Parent != null) {
                                _current = _current.Parent;
                                return true;
                            } else {
                                return false;
                            }
                        }
                    case Orders.Postfix:
                        if(_current.Parent != null) {
                            if(_current.Parent.Right == _current || _current.Parent.Right == null) {
                                _current = _current.Parent;
                            } else {
                                _current = _current.Parent.Right;
                                while(_current.Left != null || _current.Right != null) {
                                    while(_current.Left != null) {
                                        _current = _current.Left;
                                    }
                                    if(_current.Right != null) {
                                        _current = _current.Right;
                                    }
                                }
                            }
                            return true;
                        } else {
                            return false;
                        }
                }
                return false;
            }
        }

        public void Reset() {
            _current = _tree.Root;
            switch(_order) {
                case Orders.Prefix:
                    break;
                case Orders.Infix:
                    while(_current.Left != null) {
                        _current = _current.Left;
                    }
                    break;
                case Orders.Postfix:
                    while(_current.Left != null || _current.Right != null) {
                        while(_current.Left != null) {
                            _current = _current.Left;
                        }
                        if(_current.Right != null) {
                            _current = _current.Right;
                        }
                    }
                    break;
            }
            _reset = true;
        }

    }

}
