namespace CreamCheese {

  public class SpreadSheet {

    /// <summary>
    ///   Converts spreadsheet addresses into CreamCheese addresses.
    /// </summary>
    public delegate string
    ConvertAddressDelegate(string baseAddress, string refAddress);

    /// <summary>
    ///   Return the value of the cell at spreadsheet address
    ///   <paramref name="refAddress"/>.
    /// </summary>
    public delegate object
    GetCellValueDelegate(string baseAddress, string refAddress);

    private ConvertAddressDelegate _convertAddress;

    private GetCellValueDelegate _getCellValue;

    public
    SpreadSheet(ConvertAddressDelegate cAD, GetCellValueDelegate gCV) {
      _convertAddress = cAD;
      _getCellValue = gCV;
    }

    public ConvertAddressDelegate
    ConvertAddress {
      get {
        return _convertAddress;
      }
    }

    public GetCellValueDelegate
    GetCellValue {
      get {
        return _getCellValue;
      }
    }

  }

}
