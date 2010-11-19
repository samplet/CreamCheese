using System;
using System.Collections.Generic;
using CP = CreamCheese.ConstraintParser;

namespace CreamCheese {

  /// <summary>
  ///   Translates spreadsheet actions into CSPs.
  /// </summary>
  public class CreamCheese {

    /// <summary>
    ///   Converts spreadsheet addresses into CreamCheese addresses.
    /// </summary>
    public delegate string
    ConvertAddressDelegate(string baseAddress, string refAddress);

    /// <summary>
    ///   Return the value of the cell at spreadsheet address
    ///   <paramref name="refAddress"/>.
    /// </summary>
    public delegate object
    GetCellValueDelegate(string baseAddress, string refAddress);

    /// <summary>
    ///   Stores the <see cref="ConvertAddressDelegate"/>ConvertAddress
    ///   delegate</see> set by the user.
    /// </summary>
    private ConvertAddressDelegate ConvertAddress;

    /// <summary>
    ///   Stores the <see cref="GetCellValueDelegate"/>GetCellValue
    ///   delegate</see> set by the user.
    /// </summary>
    private GetCellValueDelegate GetCellValue;

    /// <summary>
    ///   Used to remember the last solution given by the solver.
    /// </summary>
    private Cream.Solution _lastSolution;

    /// <summary>
    ///   A store of all cells that are considered constrained.
    /// </summary>
    private Dictionary<string, Cell> _cells;

    /// <summary>
    ///   Constructs and initializes a CreamCheese instance.
    /// </summary>
    /// <param name="cAD">A function that will normalize spreadsheet
    ///   addresses to a form that CreamCheese can work with.</param>
    /// <param name="gCV">A function that will return a cell's value given
    ///   its address.</param>
    public
    CreamCheese(ConvertAddressDelegate cAD, GetCellValueDelegate gCV) {
      ConvertAddress = cAD;
      GetCellValue = gCV;
      _lastSolution = null;
      _cells = new Dictionary<string, Cell>();
    }

    /// <summary>
    ///   To be called when a cell is changed.
    /// </summary>
    /// <param name="key">The CreamCheese address of the cell that has
    ///   changed.</param>
    /// <param name="formula">The cell's formula.</param>
    public void
    Change(string key, string formula) {
      if(_cells.ContainsKey(key)) {
	_cells[key].Formula = formula;
	if(!formula.StartsWith("CONSTRAIN")) {
	  _cells.Remove(key);
	}
	_lastSolution = null;
      }
    }

    /// <summary>
    ///   Add a cell to the list of constrained cells.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     Note that this function expects that the cell is making use of
    ///     "CONSTRAIN" function, and will reject a cell that does not.
    ///   </para>
    /// </remarks>
    /// <param name="key">The CreamCheese address of the cell to be
    ///   added.</param>
    /// <param name="formula">The cell's formula.</param>
    public void
    Add(string key, string formula) {
      if(formula.StartsWith("CONSTRAIN")) {
	if(_cells.ContainsKey(key)) {
	  _cells[key].Formula = formula;
	} else {
	  _cells.Add(key, new Cell(key, formula));
	}
	_lastSolution = null;
      }
    }

    /// <summary>
    ///   Remove a cell from the list of constrained cells.
    /// </summary>
    /// <param name="key">The CreamCheese address of the cell to be
    ///   removed.</param>
    public void
    Remove(string key) {
      _cells.Remove(key);
    }

    /// <summary>
    ///   Remove a set of cells from the list of constrained cells based on
    ///   a partial CreamCheese address.
    /// </summary>
    /// <param name="bookKey">The partial address that represents the
    ///   workbook the cell is from.</param>
    public void
    RemoveBook(string bookKey) {
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

    /// <summary>
    ///   Remove a set of cells from the list of constrained cells based on
    ///   a partial CreamCheese address.
    /// </summary>
    /// <param name="bookKey">The partial address that represents the
    ///   worksheet the cell is from.</param>
    public void
      System.Windows.Forms.MessageBox.Show("RemoveSheet(" + sheetKey + ")");
      List<string> cellsToRemove = new List<string>();
      foreach(KeyValuePair<string, Cell> cellKvp in _cells) {
	string cellSheetKey =
	  cellKvp.Key.Substring(cellKvp.Key.IndexOf('.') + 1);
	if(cellSheetKey.StartsWith(sheetKey)) {
	  cellsToRemove.Add(cellKvp.Key);
	}
      }
      foreach(string key in cellsToRemove) {
	_cells.Remove(key);
      }
    }

    /// <summary>
    ///   Get the value of a constrained cell.
    /// </summary>
    /// <param name="key">The cell's CreamCheese address.</param>
    /// <returns>
    ///   The assignment given to the cell by the solver, if the assignment
    ///   exists.
    /// </returns>
    public object
    GetValue(string key) {
      if(_lastSolution == null) {
	try {
	  CalculateValues();
	} catch(Exception e) {
	  throw;
	}
      }
      try {
	int value = _lastSolution.GetIntValue(_cells[key].Variable);
	return (object) value;
      } catch(Exception e) {
	return null;
      }
    }

    /// <summary>
    ///   Converts the constrained cells to a CSP, and gets the solution
    ///   from the solver.
    /// </summary>
    private void
    CalculateValues() {
      Cream.Network network = new Cream.Network();
      foreach(KeyValuePair<string, Cell> kvp in _cells) {
	kvp.Value.Variable = new Cream.IntVariable(network, kvp.Key);
      }
      foreach(KeyValuePair<string, Cell> kvp in _cells) {
	CP.ExpressionTree eTree = new CP.ExpressionTree(kvp.Value.Formula);
	if(eTree.Root.Token.Type == CP.TokenType.Function
	   && ((CP.FunctionToken) eTree.Root.Token).Name == "CONSTRAIN") {
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

    /// <summary>
    ///   Adds constraints to the CSP based on the parsed string contained
    ///   in an expresstion tree.
    /// </summary>
    /// <param name="network">The constraint network (CSP).</param>
    /// <param name="cell">The cell whose formula is represented by the
    ///   expression tree.</param>
    /// <param name="eTree">The expression tree that contains a parsed string
    ///   of constraints.</param>
    private void
    AddConstraintsFromETree(Cream.Network network,
			    Cell cell, CP.ExpressionTree eTree) {
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

    /// <summary>
    ///   Test whether an expression tree contains the "CONSTRAIN" function.
    /// </summary>
    /// <param name="eTree">The expresstion tree to examine.</param>
    /// <returns>
    ///   <c>true</c> if the expression contains the "CONSTRAIN" functions,
    ///   <c>false</c> otherwise.
    /// </returns>
    private bool
    ContainsConstrainFunction(ConstraintParser.ExpressionTree eTree) {
      foreach(ConstraintParser.IToken token in eTree.Prefix) {
	if(token.Type == ConstraintParser.TokenType.Function) {
	  if(((ConstraintParser.FunctionToken) token).Name == "CONSTRAIN") {
	    return true;
	  } else {
	    foreach(ConstraintParser.ExpressionTree tree in
		    ((ConstraintParser.FunctionToken) token).Args) {
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
