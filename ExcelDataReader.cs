using OfficeOpenXml;

namespace SalesforceConnectedWithCSharp
{
    public class ExcelDataReader
    {
        private readonly SalesforceCRUD _sfCRUD;

        public ExcelDataReader(string token, string instanceUrl)
        {
            _sfCRUD = new SalesforceCRUD(token, instanceUrl);
        }

        public async Task UpsertDataFromExcel(string filePath, string objectName, string idColumnName = "Id")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                // assuming data is in the first worksheet
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                // Find the column index for the ID column
                int idColumnIndex = 0;
                for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                {
                    if (worksheet.Cells[1, col].Text.Equals(idColumnName, StringComparison.OrdinalIgnoreCase))
                    {
                        idColumnIndex = col;
                        break;
                    }
                }

                // starting from 2 to skip header row
                for (int row = 2; row <= rowCount; row++)
                {
                    var data = new Dictionary<string, object>();

                    for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                    {
                        // header value
                        var key = worksheet.Cells[1, col].Text; 
                        var value = worksheet.Cells[row, col].Text;
                        data[key] = value;
                    }

                    if (idColumnIndex != 0)
                    {
                        // Get the ID or external ID for upsert from the identified column
                        var recordId = worksheet.Cells[row, idColumnIndex].Text;
                        // Before updating, remove the Id field from the data to avoid an Id field error.
                        if (data.ContainsKey("Id"))
                        {
                            data.Remove("Id");
                        }
                        await _sfCRUD.UpdateAsync(objectName, recordId, data);
                    }
                    else
                    {
                        await _sfCRUD.CreateAsync(objectName, data);
                    }
                }
            }
            Console.WriteLine("UpsertDataFromExcel Complete");
        }
    }
}