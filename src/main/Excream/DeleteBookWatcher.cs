using System;
using Excel = Microsoft.Office.Interop.Excel;

namespace Excream {

    static class DeleteBookWatcher {

        public static void Delete(Excel.Workbook wb) {
            try {
                int workbookId = Globals.Workbooks[wb];
                Globals.ConstraintSolver.RemoveBook(workbookId.ToString());
                Globals.Workbooks.Remove(workbookId);
            } catch(Exception e) {
                throw e;
            }
        }

    }

}
