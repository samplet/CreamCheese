using System;
using Excel = Microsoft.Office.Interop.Excel;

namespace Excream {

    static class DeleteBookWatcher {

        public static void Delete(Excel.Workbook wb) {
            try {
                int workbookIndex = Globals.GetWorkbookIndex(wb);
                Globals.ConstraintSolver.RemoveBook(workbookIndex.ToString());
            } catch(Exception e) {
                throw e;
            }
        }

    }

}
