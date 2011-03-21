using System;
using System.Collections.Generic;
using FP = FormulaParser;

namespace SpreadSim {

  internal class Worksheet {

    private HashSet<Cell> _roots;
    private Dictionary<string, Cell> _cells;

    public
    Worksheet() {
      _roots = new HashSet<Cell>();
      _cells = new Dictionary<string, Cell>();
    }


    public FP.IToken
    this[string address] {
      get {
        if(_cells.ContainsKey(address)) {
          return _cells[address].Value;
        } else {
          return new FP.NullToken();
        }
      }
    }

    public Cell
    GetCell(string address) {
      if(!_cells.ContainsKey(address)) {
        SetBlank(address);
      }
      return _cells[address];
    }

    public void
    Set(string address, string formula) {
      if(_cells.ContainsKey(address)) {
        Cell c = _cells[address];
        Remove(address);
        c.Formula = formula;
        Add(address, c);
      } else {
        Cell c = new Cell(this, address, formula);
        Add(address, c);
      }
      if(formula == "" && _cells[address].Children.Count == 0) {
        Remove(address);
      }
    }

    private void
    Add(string address, Cell cell) {
      if(address != cell.Address) {
        cell.Address = address;
      }
      _cells[address] = cell;
      if(cell.Parents.Count == 0) {
        _roots.Add(cell);
      }
    }

    private bool
    Remove(string key) {
      if(_cells.ContainsKey(key)) {
        Cell cell = _cells[key];
        if(cell.Parents.Count == 0) {
          _roots.Remove(cell);
        } else {
          foreach(string p in cell.Parents) {
            GetCell(p).Children.Remove(cell.Address);
          }
        }
        _cells.Remove(key);
        return true;
      } else {
        return false;
      }
    }

    private void
    SetBlank(string address) {
      Cell c = new Cell(this, address, "");
      Add(address, c);
    }

  }

}
