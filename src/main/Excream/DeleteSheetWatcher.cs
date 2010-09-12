using System;
using Excel = Microsoft.Office.Interop.Excel;

namespace Excream {

    static class DeleteSheetWatcher {

        public static void Delete(Excel.Worksheet ws) {
            try {
                int worksheetIndex = Globals.GetWorksheetIndex(ws);
                Globals.ConstraintSolver.RemoveSheet(worksheetIndex.ToString());
            } catch(Exception e) {
                throw e;
            }
        }

    }

}
