using System;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Excream {

    public static class WorkbookOpenWatcher {

        public static void WorkbookOpen(object book) {
            NewWorkbookWatcher.NewWorkbook(book);
        }

    }

}
