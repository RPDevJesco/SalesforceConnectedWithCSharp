using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SalesforceConnectedWithCSharp.DataReader
{
    public class JsonDataReader
    {
        private readonly SalesforceCRUD _sfCRUD;

        public JsonDataReader(string token, string instanceUrl)
        {
            _sfCRUD = new SalesforceCRUD(token, instanceUrl);
        }

        public async Task UpsertDataFromJson(string filePath, string objectName, string idColumnName = "Id")
        {
            var jsonData = File.ReadAllText(filePath);
            var records = JsonConvert.DeserializeObject<List<JObject>>(jsonData);

            // Get the corresponding DTO type from objectName
            Type dtoType = Type.GetType($"SalesforceConnectedWithCSharp.SalesforceDTO.{objectName}");
            if (dtoType == null)
            {
                Console.WriteLine($"DTO for {objectName} not found!");
                return;
            }

            var dtoProperties = dtoType.GetProperties().Select(p => p.Name).ToList();

            // Assuming the first record can be used to derive the keys (headers)
            var headersFromJson = records[0].Properties().Select(p => p.Name).ToList();
            var recognizedHeaders = headersFromJson.Where(h => dtoProperties.Contains(h)).ToList();

            foreach (var record in records)
            {
                var dto = Activator.CreateInstance(dtoType);
                foreach (var header in recognizedHeaders)
                {
                    var cellValue = record[header]?.ToString();

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

                if (record.ContainsKey(idColumnName))
                {
                    var recordId = record[idColumnName].ToString();
                    await _sfCRUD.UpdateAsync(objectName, recordId, data);
                }
                else
                {
                    await _sfCRUD.CreateAsync(objectName, data);
                }
            }

            Console.WriteLine("UpsertDataFromJson Complete");
        }
    }
}