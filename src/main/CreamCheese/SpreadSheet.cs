namespace CreamCheese {

  /// <summary>
  ///   Excapsulates spreadsheet functionality needed by CreamCheese.
  /// </summary>
  /// <remarks>
  ///   <para>
  ///     This class is for spreadsheet functionality that CreamCheese assumes
  ///     exists, but does not necessarily know how to access. It is intended
  ///     that the spreadsheet code will provide delegates and properties so
  ///     that CreamCheese can access the spreadsheet via this class.
  ///   </para>
  /// </remarks>
  public class SpreadSheet {

    /// <summary>
    ///   Converts spreadsheet addresses into CreamCheese addresses.
    /// </summary>
    /// <param name="baseAddress">The CreamCheese address of the cell that is
    ///   makeing a reference.</param>
    /// <param name="refAddress">The spreadsheet address to convert.</param>
    /// <returns>The CreamCheese address of
    ///   <paramref name="refAddress"/>.</returns>
    public delegate string
    ConvertAddressDelegate(string baseAddress, string refAddress);

    /// <summary>
    ///   Get the value of a cell.
    /// </summary>
    /// <param name="baseAddress">The CreamCheese address of the cell that is
    ///   makeing a reference.</param>
    /// <param name="refAddress">The spreadsheet address of the cell that is
    ///   being referred to.</param>
    /// <returns>The value of the cell at spreadsheet address
    ///   <paramref name="refAddress"/>.</returns>
    public delegate object
    GetCellValueDelegate(string baseAddress, string refAddress);

    private ConvertAddressDelegate _convertAddress;
    private GetCellValueDelegate _getCellValue;

    /// <summary>
    ///   Construct a SpreadSheet.
    /// </summary>
    /// <param name="cAD">A function that converts addresses in accordance to
    ///   <see cref="ConvertAddressDelegate"/>.</param>
    /// <param name="gCV">A function that gets a given cell's value in
    ///   accordance to <see cref="GetCellValueDelegate"/></param>
    public
    SpreadSheet(ConvertAddressDelegate cAD, GetCellValueDelegate gCV) {
      _convertAddress = cAD;
      _getCellValue = gCV;
    }

    /// <summary>
    ///   Access the SpreadSheet's ConvertAddress function.
    /// </summary>
    public ConvertAddressDelegate
    ConvertAddress {
      get {
        return _convertAddress;
      }
    }

    /// <summary>
    ///   Access the SpreadSheet's GetCellValue function.
    /// </summary>
    public GetCellValueDelegate
    GetCellValue {
      get {
        return _getCellValue;
      }
    }

  }

}
