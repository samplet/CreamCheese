using System;
using Excel = Microsoft.Office.Interop.Excel;

namespace Excream {

    static class DeleteSheetWatcher {

        public static void Delete(Excel.Worksheet ws) {
            try {
                int worksheetId = Globals.Worksheets[ws];
                Globals.ConstraintSolver.RemoveSheet(worksheetId.ToString());
                Globals.Worksheets.Remove(worksheetId);
            } catch(Exception e) {
                throw e;
            }
        }

    }

}
