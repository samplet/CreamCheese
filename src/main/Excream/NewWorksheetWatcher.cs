using System;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Excream {

    public static class NewWorksheetWatcher {

        public static void NewWorksheet(object book, object sheet) {
            Excel.Worksheet ws = (Excel.Worksheet) sheet;
            Globals.Worksheets.Add(ws);
            ws.Change += new Excel.DocEvents_ChangeEventHandler(ChangeWatcher.Change);
            DeactivateSheetWatcher deactivateWatcher = new DeactivateSheetWatcher(ws);
            ws.Deactivate += new Excel.DocEvents_DeactivateEventHandler(deactivateWatcher.Deactivate);
            Excel.Range constrainedCell = ws.Cells.Find("CONSTRAIN", Type.Missing, Excel.XlFindLookIn.xlFormulas, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, true, Type.Missing, Type.Missing);
            Excel.Range firstFind = constrainedCell;
            while(constrainedCell != null) {
                constrainedCell.Calculate();
                System.Windows.Forms.MessageBox.Show("Inside the loop!");
                constrainedCell = ws.Cells.FindNext(constrainedCell);
                if(constrainedCell.get_Address(Type.Missing, Type.Missing, Excel.XlReferenceStyle.xlA1, Type.Missing, Type.Missing) ==
                   firstFind.get_Address(Type.Missing, Type.Missing, Excel.XlReferenceStyle.xlA1, Type.Missing, Type.Missing)) {
                    break;
                }
            }
        }

    }

}
