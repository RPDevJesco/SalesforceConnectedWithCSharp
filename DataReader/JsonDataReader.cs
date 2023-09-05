using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SalesforceConnectedWithCSharp.SalesforceAPI;

using System.Linq;
using System.Xml.Linq;

namespace SalesforceConnectedWithCSharp.DataReader
{
    public class JsonDataReader
    {
        private readonly SalesforceCRUD _sfCRUD;

        public JsonDataReader()
        {
            
        }

        public JsonDataReader(string token, string instanceUrl)
        {
            _sfCRUD = new SalesforceCRUD(token, instanceUrl);
        }

        public List<Dictionary<string, object>> ParseXml(string filePath)
        {
            var xDocument = XDocument.Load(filePath);

            // Assuming the XML structure is consistent
            var xmlRecords = xDocument.Descendants("record");

            var dataList = new List<Dictionary<string, object>>();
            foreach (var xmlRecord in xmlRecords)
            {
                var data = new Dictionary<string, object>();
                foreach (var element in xmlRecord.Elements())
                {
                    data[element.Name.LocalName] = element.Value;
                }
                dataList.Add(data);
            }

            return dataList;
        }

        public async Task AsyncUpsertXMLData(string filePath, string objectName, string idColumnName = "Id")
        {
            var records = ParseXml(filePath);

            // Get the corresponding DTO type from objectName
            Type dtoType = Type.GetType($"SalesforceConnectedWithCSharp.SalesforceDTO.{objectName}");
            if (dtoType == null)
            {
                Console.WriteLine($"DTO for {objectName} not found!");
                return;
            }

            var dtoProperties = dtoType.GetProperties().Select(p => p.Name).ToList();

            foreach (var record in records)
            {
                var dto = Activator.CreateInstance(dtoType);
                foreach (var property in dtoProperties)
                {
                    if (record.ContainsKey(property))
                    {
                        var propertyValue = record[property]?.ToString();
                        var propertyInfo = dtoType.GetProperty(property);
                        if (propertyInfo != null)
                        {
                            // Check if property type is boolean and handle conversion
                            if (propertyInfo.PropertyType == typeof(bool))
                            {
                                bool boolValue = propertyValue.Trim().ToLower() == "true" || propertyValue == "1";
                                propertyInfo.SetValue(dto, boolValue);
                            }
                            else
                            {
                                propertyInfo.SetValue(dto, Convert.ChangeType(propertyValue, propertyInfo.PropertyType));
                            }
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

            Console.WriteLine("UpsertDataFromXml Complete");
        }
    }
}