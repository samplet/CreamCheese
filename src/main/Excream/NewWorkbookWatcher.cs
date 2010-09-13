using System;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Excream {

    public static class NewWorkbookWatcher {

        public static void NewWorkbook(object book) {
            Excel.Workbook wb = (Excel.Workbook) book;
            Globals.Workbooks.Add(wb);
            DeactivateBookWatcher deactivateWatcher = new DeactivateBookWatcher(wb);
            wb.Deactivate += new Excel.WorkbookEvents_DeactivateEventHandler(deactivateWatcher.Deactivate);
            foreach(Excel.Worksheet ws in wb.Worksheets) {
                NewWorksheetWatcher.NewWorksheet(book, (object) ws);
            }
        }

    }

}
