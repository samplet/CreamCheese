using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Extensibility;
using Excel = Microsoft.Office.Interop.Excel;
using CreamCheese;

namespace Excream {

    [Guid("F4389BD9-A9C7-496f-926A-BD8730A43A8A")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Functions : IFunctions, IDTExtensibility2 {

        public Functions() {
        }

        #region IFunctions

/*        object IFunctions.CONSTRAIN(object constraint1, [Optional] object constraint2, [Optional] object constraint3,
                         [Optional] object constraint4, [Optional] object constraint5, [Optional] object constraint6,
                         [Optional] object constraint7, [Optional] object constraint8, [Optional] object constraint9,
                         [Optional] object constraint10, [Optional] object constraint11, [Optional] object constraint12,
                         [Optional] object constraint13, [Optional] object constraint14, [Optional] object constraint15,
                         [Optional] object constraint16, [Optional] object constraint17, [Optional] object constraint18,
                         [Optional] object constraint19, [Optional] object constraint20, [Optional] object constraint21,
                         [Optional] object constraint22, [Optional] object constraint23, [Optional] object constraint24,
                         [Optional] object constraint25, [Optional] object constraint26, [Optional] object constraint27,
                         [Optional] object constraint28, [Optional] object constraint29, [Optional] object constraint30,
                         [Optional] object constraint31, [Optional] object constraint32) {*/
        object IFunctions.CONSTRAIN(string constraints) {
            object caller = Globals.Application.get_Caller(Type.Missing);
            if(!(caller is Excel.Range)) {
                return null;
            }

            Excel.Range range = (Excel.Range) caller;
            string address = range.get_Address((object) false, (object) false, Excel.XlReferenceStyle.xlA1, (object) false, Type.Missing);
            string key = Globals.WorkbookIndex + "." + Globals.WorksheetIndex + "." + address;
            //string formula = Globals.FixFormula((string) range.Formula);
            string formula = "CONSTRAIN(" + constraints + ")";
            Globals.ConstraintSolver.Add(key, formula);
            object returnValue = Globals.ConstraintSolver.GetValue(key);
            if(returnValue != null) {
                return returnValue;
            } else {
                return "-?-";
            }
        }

        #endregion

        #region IDTExtensibility2

        /* This code grabs a copy of the application object for future use.
           It is from [ http://www.time4tea.net/wiki/display/MAIN/Writing+an+Excel+COM+AddIn ]. */

        public void OnAddInsUpdate(ref Array custom) {
        }

        public void OnBeginShutdown(ref Array custom) {
        }

        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom) {
            Globals.Application = (Excel.Application) application;

            Globals.Application.WorkbookNewSheet += new Excel.AppEvents_WorkbookNewSheetEventHandler(NewWorksheetWatcher.NewWorksheet);
            ((Excel.AppEvents_Event) Globals.Application).NewWorkbook += new Excel.AppEvents_NewWorkbookEventHandler(NewWorkbookWatcher.NewWorkbook);
            Globals.Application.WorkbookOpen += new Excel.AppEvents_WorkbookOpenEventHandler(WorkbookOpenWatcher.WorkbookOpen);
            foreach(Excel.Workbook wb in Globals.Application.Workbooks) {
                NewWorkbookWatcher.NewWorkbook((object) wb);
            }
        }

        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom) {
        }

        public void OnStartupComplete(ref Array custom) {
        }

        #endregion

        #region COM Register Code

        /* Here we register the class for Excel to discover. See
           [ http://blogs.msdn.com/b/eric_carter/archive/2004/12/01/273127.aspx ] for
           details. */

        [ComRegisterFunctionAttribute]
        public static void RegisterFunction(Type type) {
            Registry.ClassesRoot.CreateSubKey(GetSubKeyName(type, "Programmable"));
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(GetSubKeyName(type, "InprocServer32"), true);
            key.SetValue("", System.Environment.SystemDirectory + @"\mscoree.dll", RegistryValueKind.String);
        }

        [ComUnregisterFunctionAttribute]
        public static void UnregisterFunction(Type type) {
            Registry.ClassesRoot.DeleteSubKey(GetSubKeyName(type, "Programmable"), false);
        }

        private static string GetSubKeyName(Type type, string subKeyName) {
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            s.Append(@"CLSID\{");
            s.Append(type.GUID.ToString().ToUpper());
            s.Append(@"}\");
            s.Append(subKeyName);
            return s.ToString();
        }

        #endregion

    }

}
