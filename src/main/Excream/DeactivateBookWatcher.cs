using Excel = Microsoft.Office.Interop.Excel;

namespace Excream {

    class DeactivateBookWatcher {

        private static bool _inDeleteTest = false;
        private int _deactivateCount = 0;
        private Excel.Workbook _wb;

        public DeactivateBookWatcher(Excel.Workbook wb) {
            _deactivateCount = 0;
            _wb = wb;
        }

        public void Deactivate() {
            /*
             * We use the same method that is used for worksheets.
             */
            if(_inDeleteTest == false || _deactivateCount > 0) {
                _deactivateCount++;
            }
            _inDeleteTest = true;

            if(_deactivateCount < 2) {
                ((Excel._Workbook) _wb).Activate();
            }
            if(_deactivateCount == 1) {
                DeleteBookWatcher.Delete(_wb);
            }
            _deactivateCount = 0;
            _inDeleteTest = false;
        }

    }

}
