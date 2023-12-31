﻿using System.Reflection;
using SalesforceConnectedWithCSharp.SalesforceDTO;

namespace SalesforceConnectedWithCSharp.Helpers
{
    public static class SOQLGeneration
    {
        public static string GenerateSoqlForDto<T>(string additionalFields = null)
        {
            Type dtoType = typeof(T);
            PropertyInfo[] properties = dtoType.GetProperties();

            // Convert properties to their string names
            var fieldList = properties.Select(p => p.Name).ToList();

            // If additionalFields is provided, split it by comma and add it to the fieldList
            if (!string.IsNullOrWhiteSpace(additionalFields))
            {
                var extraFields = additionalFields.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                 .Select(f => f.Trim())
                                                 .Where(f => !string.IsNullOrWhiteSpace(f));  // Ensure no empty fields are added

                fieldList.AddRange(extraFields);
            }

            string fields = string.Join(", ", fieldList);
            Console.WriteLine($"fields: {fields}");

            return $"SELECT {fields} FROM {dtoType.Name}";
        }
    }
}
