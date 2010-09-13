using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using CC = CreamCheese;

namespace Excream {

    static class Globals {

        private static Excel.Application _application;
        private static CC.CreamCheese _constraintSolver = null;
        private static ReferenceIdKeeper<Excel.Workbook> _workbooks;
        private static ReferenceIdKeeper<Excel.Worksheet> _worksheets;

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

        public static ReferenceIdKeeper<Excel.Workbook> Workbooks {
            get {
                if(_workbooks == null) {
                    _workbooks = new ReferenceIdKeeper<Excel.Workbook>();
                }
                return _workbooks;
            }
        }

        public static ReferenceIdKeeper<Excel.Worksheet> Worksheets {
            get {
                if(_worksheets == null) {
                    _worksheets = new ReferenceIdKeeper<Excel.Worksheet>();
                }
                return _worksheets;
            }
        }

        public static string FixFormula(string formula) {
            if(formula.StartsWith("=")) {
                return formula.Substring(1);
            } else {
                return "\"" + formula + "\"";
            }
        }

        public static string ConvertAddress(string baseAddress, string refAddress) {
            int workbookId;
            string workbook;
            if(refAddress.StartsWith("[")) {
                int end = refAddress.IndexOf(']');
                workbook = refAddress.Substring(1, end - 1);
                workbookId = Globals.Workbooks[Globals.Application.Workbooks[workbook]];
                workbook = workbookId.ToString();
                refAddress = refAddress.Substring(end + 1);
            } else {
                int end = baseAddress.IndexOf('.');
                workbook = baseAddress.Substring(0, end);
                workbookId = int.Parse(workbook);
            }
            string worksheet;
            if(refAddress.Contains("!")) {
                int end = refAddress.LastIndexOf('!');
                worksheet = refAddress.Substring(0, end);
                if(worksheet.StartsWith("'")) {
                    worksheet = worksheet.Substring(1, worksheet.Length - 2);
                }
                worksheet = Globals.Worksheets[Globals.Workbooks[workbookId].Worksheets[worksheet]].ToString();
                refAddress = refAddress.Substring(end + 1);
            } else {
                int beginning = baseAddress.IndexOf('.');
                int end = baseAddress.LastIndexOf('.');
                worksheet = baseAddress.Substring(beginning + 1, end - beginning - 1);
            }
            return workbook + "." + worksheet + "." + refAddress;
        }

        public static object GetCellValue(string baseAddress, string refAddress) {
            int worksheetId;
            string worksheet;
            int beginning = refAddress.IndexOf('.');
            int end = refAddress.LastIndexOf('.');
            worksheet = refAddress.Substring(beginning + 1, end - beginning - 1);
            worksheetId = int.Parse(worksheet);
            refAddress = refAddress.Substring(end + 1);
            return (object) Globals.Worksheets[worksheetId].get_Range(refAddress).Value;
        }

    }

}
