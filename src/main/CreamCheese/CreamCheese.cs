using System;
using System.Collections.Generic;
using CP = CreamCheese.ConstraintParser;

namespace CreamCheese {

    public class CreamCheese {

        public delegate string ConvertAddressDelegate(string baseAddress, string refAddress);
        public delegate object GetCellValueDelegate(string baseAddress, string refAddress);
        private ConvertAddressDelegate ConvertAddress;
        private GetCellValueDelegate GetCellValue;
        private Cream.Solution _lastSolution;
        private Dictionary<string, Cell> _cells;

        public CreamCheese(ConvertAddressDelegate cAD, GetCellValueDelegate gCV) {
            ConvertAddress = cAD;
            GetCellValue = gCV;
            _lastSolution = null;
            _cells = new Dictionary<string, Cell>();
        }

        public void Change(string key, string formula) {
            System.Windows.Forms.MessageBox.Show("Change(" + key + ", " + formula + ")");
            if(_cells.ContainsKey(key)) {
                _cells[key].Formula = formula;
                if(!formula.StartsWith("CONSTRAIN")) {
                    _cells.Remove(key);
                }
                _lastSolution = null;
            }
            System.Text.StringBuilder cells = new System.Text.StringBuilder();
            cells.AppendLine("==CELLS==");
            cells.AppendLine("");
            foreach(KeyValuePair<string, Cell> kvp in _cells) {
                cells.AppendLine(kvp.Value.Address + ", " + kvp.Value.Formula);
            }
            System.Windows.Forms.MessageBox.Show(cells.ToString());
        }

        public void Add(string key, string formula) {
            System.Windows.Forms.MessageBox.Show("Add(" + key + ", " + formula + ")");
            if(formula.StartsWith("CONSTRAIN")) {
                if(_cells.ContainsKey(key)) {
                    _cells[key].Formula = formula;
                } else {
                    _cells.Add(key, new Cell(key, formula));
                }
                _lastSolution = null;
            }
            System.Text.StringBuilder cells = new System.Text.StringBuilder();
            cells.AppendLine("==CELLS==");
            cells.AppendLine("");
            foreach(KeyValuePair<string, Cell> kvp in _cells) {
                cells.AppendLine(kvp.Value.Address + ", " + kvp.Value.Formula);
            }
            System.Windows.Forms.MessageBox.Show(cells.ToString());
        }

        public void Remove(string key) {
            System.Windows.Forms.MessageBox.Show("Remove(" + key + ")");
            _cells.Remove(key);
        }

        public void RemoveBook(string bookKey) {
            System.Windows.Forms.MessageBox.Show("RemoveBook(" + bookKey + ")");
            List<string> cellsToRemove = new List<string>();
            foreach(KeyValuePair<string, Cell> cellKvp in _cells) {
                if(cellKvp.Key.StartsWith(bookKey)) {
                    cellsToRemove.Add(cellKvp.Key);
                }
            }
            foreach(string key in cellsToRemove) {
                _cells.Remove(key);
            }
        }

        public void RemoveSheet(string sheetKey) {
            System.Windows.Forms.MessageBox.Show("RemoveSheet(" + sheetKey + ")");
            List<string> cellsToRemove = new List<string>();
            foreach(KeyValuePair<string, Cell> cellKvp in _cells) {
                string cellSheetKey = cellKvp.Key.Substring(cellKvp.Key.IndexOf('.') + 1);
                if(cellSheetKey.StartsWith(sheetKey)) {
                    cellsToRemove.Add(cellKvp.Key);
                }
            }
            foreach(string key in cellsToRemove) {
                _cells.Remove(key);
            }
        }

        public object GetValue(string key) {
            System.Windows.Forms.MessageBox.Show("GetValue(" + key + ")");
            if(_lastSolution == null) {
                try {
                    CalculateValues();
                } catch(Exception e) {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
            }
            try {
                int value = _lastSolution.GetIntValue(_cells[key].Variable);
                return (object) value;
            } catch(Exception e) {
                return null;
            }
        }

        private void CalculateValues() {
            Cream.Network network = new Cream.Network();
            foreach(KeyValuePair<string, Cell> kvp in _cells) {
                kvp.Value.Variable = new Cream.IntVariable(network, kvp.Key);
            }
            foreach(KeyValuePair<string, Cell> kvp in _cells) {
                CP.ExpressionTree eTree = new CP.ExpressionTree(kvp.Value.Formula);
                if(eTree.Root.Token.Type == CP.TokenType.Function && ((CP.FunctionToken) eTree.Root.Token).Name == "CONSTRAIN") {
                    CP.FunctionToken fToken = (CP.FunctionToken) eTree.Root.Token;
                    foreach(CP.ExpressionTree tree in fToken.Args) {
                        if(tree.Root.Token.Type == CP.TokenType.Operator) {
                            switch(((CP.OperatorToken) tree.Root.Token).Value) {
                                case CP.OperatorToken.Operator.EqualTo:
                                case CP.OperatorToken.Operator.GreaterThan:
                                case CP.OperatorToken.Operator.LessThan:
                                case CP.OperatorToken.Operator.GreaterThanOrEqualTo:
                                case CP.OperatorToken.Operator.LessThanOrEqualTo:
                                case CP.OperatorToken.Operator.NotEqualTo:
                                    AddConstraintsFromETree(network, kvp.Value, tree);
                                    break;
                                case CP.OperatorToken.Operator.Range:
                                case CP.OperatorToken.Operator.Intersect:
                                case CP.OperatorToken.Operator.Union:
                                    throw new NotImplementedException("Range constraints are not implemented");
                                default:
                                    throw new InvalidOperationException("Invalid constraint list for cell with key \"" + kvp.Key + "\"");
                            }
                        } else if(tree.Root.Token.Type == CP.TokenType.Range) {
                            throw new NotImplementedException("Range constraints are not implemented");
                        } else if(tree.Root.Token.Type == CP.TokenType.Function) {
                            AddConstraintsFromETree(network, kvp.Value, tree);
                        } else {
                            throw new InvalidOperationException("Invalid constraint list for cell with key \"" + kvp.Key + "\"");
                        }
                    }
                }
            }
            Cream.Solver solver = new Cream.DefaultSolver(network);
            _lastSolution = solver.FindFirst();
        }

        private void AddConstraintsFromETree(Cream.Network network, Cell cell, CP.ExpressionTree eTree) {
            Stack<ICspItem> valueStack = new Stack<ICspItem>();
            foreach(CP.IToken token in eTree.Postfix) {
                switch(token.Type) {
                    case CP.TokenType.Operator:
                        if(((CP.OperatorToken) token).Unary) {
                            ICspItem operand = valueStack.Pop();
                            switch(((CP.OperatorToken) token).Value) {
                                case CP.OperatorToken.Operator.Negate:
                                    if(operand.Type == CspItemType.Number) {
                                        valueStack.Push(new NumberCspItem(-((NumberCspItem) operand).Value));
                                    } else if(operand.Type == CspItemType.Variable) {
                                        Cream.IntVariable negateResult = new Cream.IntVariable(network);
                                        new Cream.IntFunc(network, Cream.IntFunc.Negate, negateResult, ((VariableCspItem) operand).Value);
                                        valueStack.Push(new VariableCspItem(negateResult));
                                    }
                                    break;
                                case CP.OperatorToken.Operator.Percent:
                                    valueStack.Push(operand);
                                    throw new NotImplementedException("The percent operator is not implemented");
                                default:
                                    valueStack.Push(operand);
                                    throw new InvalidOperationException("Unknown unary operator");
                            }
                        } else {
                            ICspItem rhs = valueStack.Pop();
                            ICspItem lhs = valueStack.Pop();
                            if(lhs.Type == CspItemType.Number && rhs.Type == CspItemType.Number) {
                                switch(((CP.OperatorToken) token).Value) {
                                    case CP.OperatorToken.Operator.Plus:
                                        valueStack.Push(new NumberCspItem(((NumberCspItem) lhs).Value + ((NumberCspItem) rhs).Value));
                                        break;
                                    case CP.OperatorToken.Operator.Minus:
                                        valueStack.Push(new NumberCspItem(((NumberCspItem) lhs).Value - ((NumberCspItem) rhs).Value));
                                        break;
                                    case CP.OperatorToken.Operator.Multiply:
                                        valueStack.Push(new NumberCspItem(((NumberCspItem) lhs).Value * ((NumberCspItem) rhs).Value));
                                        break;
                                    default:
                                        throw new InvalidOperationException("The operator '" + token.ToString() + "' is not supported with numeric arguments");
                                }
                            } else {
                                Cream.IntVariable v0, v1;
                                if(lhs.Type == CspItemType.Number) {
                                    v0 = new Cream.IntVariable(network, ((NumberCspItem) lhs).Value);
                                } else if(lhs.Type == CspItemType.Variable) {
                                    v0 = ((VariableCspItem) lhs).Value;
                                } else {
                                    throw new Exception("Cannot place a constraint on a constraint");
                                }
                                if(rhs.Type == CspItemType.Number) {
                                    v1 = new Cream.IntVariable(network, ((NumberCspItem) rhs).Value);
                                } else if(rhs.Type == CspItemType.Variable) {
                                    v1 = ((VariableCspItem) rhs).Value;
                                } else {
                                    throw new Exception("Cannot place a constraint on a constraint");
                                }
                                switch(((CP.OperatorToken) token).Value) {
                                    case CP.OperatorToken.Operator.EqualTo:
                                        valueStack.Push(new ConstraintCspItem(new Cream.Equals(network, v0, v1)));
                                        break;
                                    case CP.OperatorToken.Operator.NotEqualTo:
                                        valueStack.Push(new ConstraintCspItem(new Cream.NotEquals(network, v0, v1)));
                                        break;
                                    case CP.OperatorToken.Operator.GreaterThan:
                                        valueStack.Push(new ConstraintCspItem(new Cream.IntComparison(network, Cream.IntComparison.Gt, v0, v1)));
                                        break;
                                    case CP.OperatorToken.Operator.GreaterThanOrEqualTo:
                                        valueStack.Push(new ConstraintCspItem(new Cream.IntComparison(network, Cream.IntComparison.Ge, v0, v1)));
                                        break;
                                    case CP.OperatorToken.Operator.LessThan:
                                        valueStack.Push(new ConstraintCspItem(new Cream.IntComparison(network, Cream.IntComparison.Lt, v0, v1)));
                                        break;
                                    case CP.OperatorToken.Operator.LessThanOrEqualTo:
                                        valueStack.Push(new ConstraintCspItem(new Cream.IntComparison(network, Cream.IntComparison.Le, v0, v1)));
                                        break;
                                    case CP.OperatorToken.Operator.Plus:
                                        Cream.IntVariable plusResult = new Cream.IntVariable(network);
                                        new Cream.IntArith(network, Cream.IntArith.Add, plusResult, v0, v1);
                                        valueStack.Push(new VariableCspItem(plusResult));
                                        break;
                                    case CP.OperatorToken.Operator.Minus:
                                        Cream.IntVariable minusResult = new Cream.IntVariable(network);
                                        new Cream.IntArith(network, Cream.IntArith.Subtract, minusResult, v0, v1);
                                        valueStack.Push(new VariableCspItem(minusResult));
                                        break;
                                    case CP.OperatorToken.Operator.Multiply:
                                        Cream.IntVariable multiplyResult = new Cream.IntVariable(network);
                                        new Cream.IntArith(network, Cream.IntArith.MULTIPLY, multiplyResult, v0, v1);
                                        valueStack.Push(new VariableCspItem(multiplyResult));
                                        break;
                                    default:
                                        throw new InvalidOperationException("The operator '" + token.ToString() + "' is not supported with variable arguments");
                                }
                            }
                        }
                        break;
                    case CP.TokenType.Range:
                        string key = ConvertAddress(cell.Address, ((CP.RangeToken) token).Value);
                        if(_cells.ContainsKey(key)) {
                            valueStack.Push(new VariableCspItem(_cells[key].Variable));
                        } else {
                            System.Windows.Forms.MessageBox.Show("gCV(" + cell.Address + ", " + key + ") = " + GetCellValue(cell.Address, key).ToString());
                            valueStack.Push(new NumberCspItem(int.Parse(GetCellValue(cell.Address, key).ToString())));
                        }
                        break;
                    case CP.TokenType.Number:
                            valueStack.Push(new NumberCspItem(((CP.NumberToken) token).Value));
                        break;
                    case CP.TokenType.Function:
                        if(((CP.FunctionToken) token).Name == "NEQ") {
                            List<Cream.IntVariable> nEqList = new List<Cream.IntVariable>();
                            foreach(CP.ExpressionTree arg in ((CP.FunctionToken) token).Args) {
                                if(arg.Root.Token.Type != CP.TokenType.Range) {
                                    throw new InvalidOperationException("Only single cell ranges are supported for the NEQ function");
                                } else {
                                    key = ConvertAddress(cell.Address, ((CP.RangeToken) arg.Root.Token).Value);
                                    if(_cells.ContainsKey(key)) {
                                        nEqList.Add(_cells[key].Variable);
                                    } else {
                                        nEqList.Add(new Cream.IntVariable(network, int.Parse(GetCellValue(cell.Address, key).ToString())));
                                    }
                                }
                            }
                            valueStack.Push(new ConstraintCspItem(new Cream.NotEquals(network, nEqList.ToArray())));
                        } else {
                            throw new InvalidOperationException("Only the NEQ function is permitted");
                        }
                        break;
                    default:
                        throw new InvalidOperationException("Only ranges, numbers, and operators are permitted");
                }
            }
        }

        private bool ContainsConstrainFunction(ConstraintParser.ExpressionTree eTree) {
            foreach(ConstraintParser.IToken token in eTree.Prefix) {
                if(token.Type == ConstraintParser.TokenType.Function) {
                    if(((ConstraintParser.FunctionToken) token).Name == "CONSTRAIN") {
                        return true;
                    } else {
                        foreach(ConstraintParser.ExpressionTree tree in ((ConstraintParser.FunctionToken) token).Args) {
                            if(ContainsConstrainFunction(tree)) {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

    }

}
