using OfficeOpenXml;
using SalesforceConnectedWithCSharp.SalesforceAPI;

using System.Text;

namespace SalesforceConnectedWithCSharp.DataReader
{
    public class ExcelDataReader
    {
        private readonly SalesforceCRUD _sfCRUD;

        public ExcelDataReader()
        {
            
        }

        public ExcelDataReader(string token, string instanceUrl)
        {
            _sfCRUD = new SalesforceCRUD(token, instanceUrl);
        }

        public async Task<List<Dictionary<string, object>>> ParseExcel(string filePath, string objectName)
        {
            var recordsList = new List<Dictionary<string, object>>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                Type dtoType = Type.GetType($"SalesforceConnectedWithCSharp.SalesforceDTO.{objectName}");
                if (dtoType == null)
                {
                    Console.WriteLine($"DTO for {objectName} not found!");
                    return recordsList;
                }

                var dtoProperties = dtoType.GetProperties().Select(p => p.Name).ToList();

                List<string> headersFromExcel = new List<string>();
                for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                {
                    headersFromExcel.Add(worksheet.Cells[1, col].Text);
                }

                List<string> recognizedHeaders = headersFromExcel.Where(h => dtoProperties.Contains(h)).ToList();

                for (int row = 2; row <= rowCount; row++)
                {
                    var data = new Dictionary<string, object>();
                    var dto = Activator.CreateInstance(dtoType);

                    foreach (var header in recognizedHeaders)
                    {
                        int col = headersFromExcel.IndexOf(header) + 1;
                        var cellValue = worksheet.Cells[row, col].Text;

                        if (string.IsNullOrWhiteSpace(cellValue))
                            continue;

                        var property = dtoType.GetProperty(header);
                        if (property != null)
                        {
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

                    foreach (var prop in dtoType.GetProperties())
                    {
                        data[prop.Name] = prop.GetValue(dto);
                    }

                    if (data.ContainsKey("Id"))
                    {
                        data.Remove("Id");
                    }

                    recordsList.Add(data);
                }
            }
            return recordsList;
        }

        public async Task UpsertExcelData(string filePath, string objectName, string idColumnName = "Id")
        {
            var parsedRecords = await ParseExcel(filePath, objectName);

            // Identify the column for the ID outside the loop to avoid recalculating
            int idColumnIndex = parsedRecords[0].ContainsKey(idColumnName) ? parsedRecords[0].Keys.ToList().IndexOf(idColumnName) : -1;

            foreach (var record in parsedRecords)
            {
                if (idColumnIndex != -1)
                {
                    var recordId = record[idColumnName].ToString();
                    await _sfCRUD.UpdateAsync(objectName, recordId, record);
                }
                else
                {
                    await _sfCRUD.CreateAsync(objectName, record);
                }
            }

            Console.WriteLine("UpsertDataFromExcel Complete");
        }

    }
}