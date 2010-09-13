using System;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Excream {

    public static class NewWorkbookWatcher {

        public static void NewWorkbook(object book) {
            Excel.Workbook wb = (Excel.Workbook) book;
            try {
                ActivateBookWatcher activateWatcher = new ActivateBookWatcher();
                Globals.Workbooks.Add(activateWatcher.Index, wb);
                Globals.Application.WorkbookActivate += new Excel.AppEvents_WorkbookActivateEventHandler(activateWatcher.Activate);
                DeactivateBookWatcher deactivateWatcher = new DeactivateBookWatcher(wb);
                wb.Deactivate += new Excel.WorkbookEvents_DeactivateEventHandler(deactivateWatcher.Deactivate);
            } catch(Exception e) {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
            foreach(Excel.Worksheet ws in wb.Worksheets) {
                NewWorksheetWatcher.NewWorksheet(book, (object) ws);
            }
        }

    }

}
