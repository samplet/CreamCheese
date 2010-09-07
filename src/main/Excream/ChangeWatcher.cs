using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Excream {

    public class ChangeWatcher {

        public static void Change(Excel.Range target) {
            string address = target.get_Address((object) false, (object) false, Excel.XlReferenceStyle.xlA1, (object) false, Type.Missing);
            string key = Globals.WorkbookIndex + "." + Globals.WorksheetIndex + "." + address;
            string formula = Globals.FixFormula((string) target.Formula);
            if(formula.StartsWith("CONSTRAIN")) {
                formula = formula.Substring(11, formula.Length - 13);
                formula = "CONSTRAIN(" + formula + ")";
            }
            Globals.ConstraintSolver.Change(key, formula);
        }

    }

}
