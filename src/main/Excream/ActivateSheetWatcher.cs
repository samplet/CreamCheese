using System;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Excream {

    public class ActivateSheetWatcher {

        private static int _baseIndex = 0;

        private int _index;
        public int Index {
            get {
                return _index;
            }
        }

        public ActivateSheetWatcher() {
            _index = ActivateSheetWatcher._baseIndex++;
        }

        public void Activate() {
            Globals.WorksheetIndex = _index;
        }

    }

}
