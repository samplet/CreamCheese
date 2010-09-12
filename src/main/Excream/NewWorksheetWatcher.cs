using System;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Excream {

    public static class NewWorksheetWatcher {

        public static void NewWorksheet(object book, object sheet) {
            Excel.Worksheet ws = (Excel.Worksheet) sheet;
            ((Excel.DocEvents_Event) ws).Change += new Excel.DocEvents_ChangeEventHandler(ChangeWatcher.Change);
            ActivateSheetWatcher activateWatcher = new ActivateSheetWatcher();
            Globals.Worksheets.Add(activateWatcher.Index, ws);
            ((Excel.DocEvents_Event) ws).Activate += new Excel.DocEvents_ActivateEventHandler(activateWatcher.Activate);
            DeactivateSheetWatcher deactivateWatcher = new DeactivateSheetWatcher(ws);
            ((Excel.DocEvents_Event) ws).Deactivate += new Excel.DocEvents_DeactivateEventHandler(deactivateWatcher.Deactivate);

            foreach(Excel.Range r in ws.UsedRange) {
                if((string) r.Formula != "") {
                    System.Windows.Forms.MessageBox.Show((string) r.Formula);
                }
            }
        }

    }

}
