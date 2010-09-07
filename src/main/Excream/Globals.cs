using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using CC = CreamCheese;

namespace Excream {

    public static class Globals {

        private static Excel.Application _application;
        private static CC.CreamCheese _constraintSolver = null;
        private static int _workbookIndex = 0;
        private static Dictionary<int, Excel.Workbook> _workbooks = null;
        private static int _worksheetIndex = 0;
        private static Dictionary<int, Excel.Worksheet> _worksheets = null;

        public static Excel.Application Application {
            get {
                return _application;
            }
            set {
                _application = value;
            }
        }

        public static CC.CreamCheese ConstraintSolver {
            get {
                if(_constraintSolver == null) {
                    _constraintSolver = new CC.CreamCheese(Globals.ConvertAddress, Globals.GetCellValue);
                }
                return _constraintSolver;
            }
        }

        public static int WorkbookIndex {
            get {
                return _workbookIndex;
            }
            set {
                _workbookIndex = value;
            }
        }

        public static Dictionary<int, Excel.Workbook> Workbooks {
            get {
                if(_workbooks == null) {
                    _workbooks = new Dictionary<int, Excel.Workbook>();
                }
                return _workbooks;
            }
        }

        public static int WorksheetIndex {
            get {
                return _worksheetIndex;
            }
            set {
                _worksheetIndex = value;
            }
        }

        public static Dictionary<int, Excel.Worksheet> Worksheets {
            get {
                if(_worksheets == null) {
                    _worksheets = new Dictionary<int, Excel.Worksheet>();
                }
                return _worksheets;
            }
        }

        public static int GetWorkbookIndex(Excel.Workbook wb) {
            foreach(KeyValuePair<int, Excel.Workbook> kvp in Globals.Workbooks) {
                if(wb == kvp.Value) {
                    return kvp.Key;
                }
            }
            throw new KeyNotFoundException("Could not find workbook");
        }

        public static int GetWorkbookIndex(string name) {
            foreach(KeyValuePair<int, Excel.Workbook> kvp in Globals.Workbooks) {
                if(name == kvp.Value.Name) {
                    return kvp.Key;
                }
            }
            throw new KeyNotFoundException("Could not find workbook");
        }

        public static int GetWorksheetIndex(Excel.Worksheet ws) {
            foreach(KeyValuePair<int, Excel.Worksheet> kvp in Globals.Worksheets) {
                if(ws == kvp.Value) {
                    return kvp.Key;
                }
            }
            throw new KeyNotFoundException("Could not find worksheet");
        }

        public static int GetWorksheetIndex(int workbookIndex, string name) {
            foreach(Excel.Worksheet ws in Globals.Workbooks[workbookIndex].Worksheets) {
                if(name == ws.Name) {
                    return GetWorksheetIndex(ws);
                }
            }
            throw new KeyNotFoundException("Worksheet name does not exist in workbook");
        }

        public static string FixFormula(string formula) {
            if(formula.StartsWith("=")) {
                return formula.Substring(1);
            } else {
                return "\"" + formula + "\"";
            }
        }

        public static string ConvertAddress(string baseAddress, string refAddress) {
            int workbookIndex;
            string workbook;
            if(refAddress.StartsWith("[")) {
                int end = refAddress.IndexOf(']');
                workbook = refAddress.Substring(1, end - 1);
                workbookIndex = GetWorkbookIndex(workbook);
                workbook = workbookIndex.ToString();
                refAddress = refAddress.Substring(end + 1);
            } else {
                int end = baseAddress.IndexOf('.');
                workbook = baseAddress.Substring(0, end);
                workbookIndex = int.Parse(workbook);
            }
            string worksheet;
            if(refAddress.Contains("!")) {
                int end = refAddress.LastIndexOf('!');
                worksheet = refAddress.Substring(0, end);
                if(worksheet.StartsWith("'")) {
                    worksheet = worksheet.Substring(1, worksheet.Length - 2);
                }
                worksheet = GetWorksheetIndex(workbookIndex, worksheet).ToString();
                refAddress = refAddress.Substring(end + 1);
            } else {
                int beginning = baseAddress.IndexOf('.');
                int end = baseAddress.LastIndexOf('.');
                worksheet = baseAddress.Substring(beginning + 1, end - beginning - 1);
            }
            return workbook + "." + worksheet + "." + refAddress;
        }

        public static object GetCellValue(string baseAddress, string refAddress) {
            int worksheetIndex;
            string worksheet;
            int beginning = refAddress.IndexOf('.');
            int end = refAddress.LastIndexOf('.');
            worksheet = refAddress.Substring(beginning + 1, end - beginning - 1);
            worksheetIndex = int.Parse(worksheet);
            refAddress = refAddress.Substring(end + 1);
            return (object) Worksheets[worksheetIndex].get_Range(refAddress).Value;
        }

    }

}
