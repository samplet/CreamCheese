using Excel = Microsoft.Office.Interop.Excel;

namespace Excream {

    class DeactivateSheetWatcher {

        private static bool _inDeleteTest = false;
        private static int _deactivateCount = 0;
        private Excel.Worksheet _ws;

        public DeactivateSheetWatcher(Excel.Worksheet ws) {
            _ws = ws;
        }

        public void Deactivate() {
            /*
             * Upon deactivating a sheet, we will increment its deactivate count, and
             * begin a delete test, to see of the sheet is being deleted. Note that
             * this condition will be true only if deactivate count is greater than 0,
             * as this function will run somewhat recursively.
             */
            if(_inDeleteTest == false || _deactivateCount > 0) {
                _deactivateCount++;
            }
            _inDeleteTest = true;

            /*
             * If the sheet is being deleted, trying to activate it will silently fail,
             * letting us verify through the deactivate count that it is being deleted.
             * Otherwise, this deactivate function will run recursively (as a result of
             * activating other sheets), once for the sheet that is being activated by
             * the user (as this sheet gets reactivated), and then a second time for this
             * sheet (as the sheet that user is activating gets activated a second time).
             * By this time, deactivate count will be 2, and we know that the sheet is
             * not being deleted.
             */
            if(_deactivateCount < 2) {
                ((Excel._Worksheet) _ws).Activate();
            }
            if(_deactivateCount == 1) {
                DeleteSheetWatcher.Delete(_ws);
            }
            _deactivateCount = 0;
            _inDeleteTest = false;
        }

    }

}
