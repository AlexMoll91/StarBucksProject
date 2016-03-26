using System.Collections.Generic;
using System.Data;
using GemBox.Spreadsheet;

namespace StarbucksAutomation
{
    public static class Data
    {
        public static List<string> GetTransferInfo(IEnumerable<string[]> transferDict, out List<string> cardTransferTo, out List<string> cardTransferAmount)
        {
            var cardTransferFrom = new List<string>();
            cardTransferTo = new List<string>();
            cardTransferAmount = new List<string>();

            foreach (var transfer in transferDict)
            {
                cardTransferFrom.Add(transfer[0]);
                cardTransferTo.Add(transfer[1]);
                cardTransferAmount.Add(transfer[2]);
            }
            return cardTransferFrom;
        }
        public static ExcelFile CreateDataTable(out ExcelWorksheet ws, out DataTable dataTable)
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var ef = new ExcelFile();

            ws = ef.Worksheets.Add("Card Data");

            dataTable = ws.CreateDataTable(new CreateDataTableOptions
            {
                StartRow = 0,
                StartColumn = 0,
                NumberOfRows = 0,
                ExtractDataOptions = ExtractDataOptions.StopAtFirstEmptyRow
                
            });
            ws.DefaultColumnWidth = 50;
            dataTable.Columns.Add("Card Name");
            dataTable.Columns.Add("Card Amount");
            return ef;
        }
       

    }

}

