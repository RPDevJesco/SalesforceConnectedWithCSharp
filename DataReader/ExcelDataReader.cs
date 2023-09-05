using OfficeOpenXml;

namespace SalesforceConnectedWithCSharp.DataReader
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
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                // Get the corresponding DTO type from objectName
                Type dtoType = Type.GetType($"SalesforceConnectedWithCSharp.SalesforceDTO.{objectName}");
                if (dtoType == null)
                {
                    Console.WriteLine($"DTO for {objectName} not found!");
                    return;
                }

                var dtoProperties = dtoType.GetProperties().Select(p => p.Name).ToList();

                // Validate Excel headers against DTO properties
                List<string> headersFromExcel = new List<string>();
                for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                {
                    headersFromExcel.Add(worksheet.Cells[1, col].Text);
                }

                List<string> recognizedHeaders = headersFromExcel.Where(h => dtoProperties.Contains(h)).ToList();

                // Identify the column for the ID
                int idColumnIndex = recognizedHeaders.IndexOf(idColumnName);

                for (int row = 2; row <= rowCount; row++)
                {
                    var dto = Activator.CreateInstance(dtoType);
                    foreach (var header in recognizedHeaders)
                    {
                        int col = headersFromExcel.IndexOf(header) + 1;  // +1 because Excel is 1-based indexing
                        var cellValue = worksheet.Cells[row, col].Text;

                        // Check if the cell value is blank or null and continue to the next iteration if it is.
                        if (string.IsNullOrWhiteSpace(cellValue))
                            continue;

                        var property = dtoType.GetProperty(header);
                        if (property != null)
                        {
                            // Check if property type is boolean and handle conversion
                            if (property.PropertyType == typeof(bool))
                            {
                                bool boolValue = cellValue.Trim().ToLower() == "true" || cellValue == "1";
                                property.SetValue(dto, boolValue);
                            }
                            else
                            {
                                property.SetValue(dto, Convert.ChangeType(cellValue, property.PropertyType));
                            }
                        }
                    }

                    // Convert DTO back to dictionary 
                    var data = new Dictionary<string, object>();
                    foreach (var prop in dtoType.GetProperties())
                    {
                        data[prop.Name] = prop.GetValue(dto);
                    }

                    if (data.ContainsKey("Id"))
                    {
                        data.Remove("Id");
                    }

                    if (idColumnIndex != -1)
                    {
                        var recordId = worksheet.Cells[row, idColumnIndex + 1].Text;

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