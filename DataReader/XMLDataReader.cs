using System.Xml.Linq;

namespace SalesforceConnectedWithCSharp.DataReader
{
    public class XmlDataReader
    {
        private readonly SalesforceCRUD _sfCRUD;

        public XmlDataReader(string token, string instanceUrl)
        {
            _sfCRUD = new SalesforceCRUD(token, instanceUrl);
        }

        public async Task UpsertDataFromXml(string filePath, string objectName, string idColumnName = "Id")
        {
            var xDocument = XDocument.Load(filePath);

            // Assuming the XML structure is consistent and looks something like:
            // <records>
            //    <record>
            //       <Id>123</Id>
            //       <Name>John</Name>
            //       ...
            //    </record>
            //    ...
            // </records>

            var xmlRecords = xDocument.Descendants("record");

            // Get the corresponding DTO type from objectName
            Type dtoType = Type.GetType($"SalesforceConnectedWithCSharp.SalesforceDTO.{objectName}");
            if (dtoType == null)
            {
                Console.WriteLine($"DTO for {objectName} not found!");
                return;
            }

            var dtoProperties = dtoType.GetProperties().Select(p => p.Name).ToList();

            foreach (var xmlRecord in xmlRecords)
            {
                var dto = Activator.CreateInstance(dtoType);

                foreach (var property in dtoProperties)
                {
                    var xmlElement = xmlRecord.Element(property);
                    if (xmlElement != null)
                    {
                        var propertyInfo = dtoType.GetProperty(property);
                        if (propertyInfo != null)
                        {
                            // Check if property type is boolean and handle conversion
                            if (propertyInfo.PropertyType == typeof(bool))
                            {
                                bool boolValue = xmlElement.Value.Trim().ToLower() == "true" || xmlElement.Value == "1";
                                propertyInfo.SetValue(dto, boolValue);
                            }
                            else
                            {
                                propertyInfo.SetValue(dto, Convert.ChangeType(xmlElement.Value, propertyInfo.PropertyType));
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

                var idElement = xmlRecord.Element(idColumnName);
                if (idElement != null)
                {
                    var recordId = idElement.Value;
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