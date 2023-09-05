using CsvHelper;
using CsvHelper.Configuration;
using SalesforceConnectedWithCSharp.SalesforceAPI;
using System.Globalization;

namespace SalesforceConnectedWithCSharp.DataReader
{
    public class CsvDataReader
    {
        private readonly SalesforceCRUD _sfCRUD;

        public CsvDataReader()
        {
            
        }

        public CsvDataReader(string token, string instanceUrl)
        {
            _sfCRUD = new SalesforceCRUD(token, instanceUrl);
        }

        public async Task<List<Dictionary<string, object>>> ParseCSV(string filePath, string objectName)
        {
            var recordsList = new List<Dictionary<string, object>>();

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Adjust any configuration if needed
                Delimiter = ","
            });

            var records = csv.GetRecords<Dictionary<string, object>>().ToList();

            Type dtoType = Type.GetType($"SalesforceConnectedWithCSharp.SalesforceDTO.{objectName}");
            if (dtoType == null)
            {
                Console.WriteLine($"DTO for {objectName} not found!");
                return recordsList;
            }

            var dtoProperties = dtoType.GetProperties().Select(p => p.Name).ToList();
            var headersFromCsv = records[0].Keys.ToList();
            var recognizedHeaders = headersFromCsv.Where(h => dtoProperties.Contains(h)).ToList();

            foreach (var record in records)
            {
                var data = new Dictionary<string, object>();
                var dto = Activator.CreateInstance(dtoType);

                foreach (var header in recognizedHeaders)
                {
                    var cellValue = record[header]?.ToString();

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

            return recordsList;
        }

        public async Task AsyncUpsertCSVData(string filePath, string objectName, string idColumnName = "Id")
        {
            var parsedRecords = await ParseCSV(filePath, objectName);

            foreach (var record in parsedRecords)
            {
                if (record.ContainsKey(idColumnName))
                {
                    var recordId = record[idColumnName].ToString();
                    await _sfCRUD.UpdateAsync(objectName, recordId, record);
                }
                else
                {
                    await _sfCRUD.CreateAsync(objectName, record);
                }
            }

            Console.WriteLine("UpsertDataFromCsv Complete");
        }
    }
}