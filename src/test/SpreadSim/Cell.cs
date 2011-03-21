using System.Collections.Generic;
using FP = FormulaParser;

namespace SpreadSim {

  internal class Cell {

    private Worksheet _worksheet;
    private HashSet<string> _parents;
    private HashSet<string> _children;
    private string _address;
    private string _formula;
    private bool _dirty;
    private FP.IToken _value;

    public
    Cell(Worksheet ws, string address, string formula) {
      _parents = new HashSet<string>();
      _children = new HashSet<string>();
      _worksheet = ws;
      Address = address;
      Formula = formula;
    }

    public string
    Address {
      get {
        return _address;
      }
      set {
        _address = value;
      }
    }

    public HashSet<string>
    Children {
      get {
        return _children;
      }
    }

    public bool
    Dirty {
      get {
        return _dirty;
      }
      set {
        if(value == true && _dirty == false) {
          foreach(string c in _children) {
            _worksheet.GetCell(c).Dirty = true;
          }
        }
        _dirty = value;
      }
    }

    public string
    Formula {
      get {
        return _formula;
      }
      set {
        if(value == _formula) {
          return;
        }
        _formula = value;
        Parents.Clear();
        FindParents();
        foreach(string p in Parents) {
          _worksheet.GetCell(p).Children.Add(this.Address);
        }
        this.Dirty = true;
      }
    }

    public HashSet<string>
    Parents {
      get {
        return _parents;
      }
    }

    public bool
    SelfDependent {
      get {
        foreach(string p in Parents) {
          if(_worksheet.GetCell(p).DependentOn(this)) {
            return true;
          }
        }
        return false;
      }
    }

    public FP.IToken
    Value {
      get {
        if(Dirty) {
          this.Calculate();
        }
        return _value ?? new FP.NullToken();
      }
    }

    public Worksheet Worksheet {
      get {
        return _worksheet;
      }
    }

    public void
    Calculate() {
      Semantics semantics = new Semantics(this);
      FormulaParser.Parser fp = new FP.Parser(Formula, semantics);
      fp.Parse();
      _value = fp.Result;
      Dirty = false;
    }

    public bool
    DependentOn(Cell goal) {
      if(this == goal) {
        return true;
      } else {
        foreach(string p in Parents) {
          if(_worksheet.GetCell(p).DependentOn(goal)) {
            return true;
          }
        }
        return false;
      }
    }

    private void
    FindParents() {
      HashSet<FP.RangeToken> dependencies = new HashSet<FP.RangeToken>();
      Semantics semantics = new Semantics(this, dependencies);
      FP.Parser fp = new FormulaParser.Parser(Formula, semantics);
      fp.Parse();
      foreach(FP.RangeToken r in dependencies) {
        _parents.Add(r.Value);
      }
    }

  }

}
