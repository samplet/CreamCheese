using System;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Excream {

    public class ActivateBookWatcher {

        private static int _baseIndex = 0;

        private int _index;
        public int Index {
            get {
                return _index;
            }
        }

        public ActivateBookWatcher() {
            _index = ActivateBookWatcher._baseIndex++;
        }

        public void Activate(Excel.Workbook wb) {
            Globals.WorkbookIndex = _index;
            Globals.WorksheetIndex = Globals.GetWorksheetIndex(wb.ActiveSheet);
        }

    }

}