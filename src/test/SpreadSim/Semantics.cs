using System;
using System.Collections;
using System.Collections.Generic;
using FormulaParser;

namespace SpreadSim {

  internal class Semantics : SemanticsBase {

    private Cell _baseCell;
    private HashSet<RangeToken> _dependencies;

    public
    Semantics(Cell baseCell, HashSet<RangeToken> dependencies) {
      _baseCell = baseCell;
      _dependencies = dependencies;
    }

    public
    Semantics(Cell baseCell) : this(baseCell, null) {}

    protected override IToken
    EvaluateComplexOperation(IToken x, Tokens o) {
      throw new NotImplementedException();
    }

    protected override IToken
    EvaluateComplexOperation(IToken x, IToken y, Tokens o) {
      throw new NotImplementedException();
    }

    protected override IToken
    ResolveRange(RangeToken r) {
      if(_dependencies != null) {
        _dependencies.Add(r);
        return new NullToken();
      }
      return _baseCell.Worksheet[r.Value];
    }

  }

}
