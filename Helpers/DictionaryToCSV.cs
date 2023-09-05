namespace SalesforceConnectedWithCSharp
{
    public static class DictionaryToCSV
    {
        public static string Convert(Dictionary<string, object> data)
        {
            // Convert keys to CSV header
            var header = string.Join(",", data.Keys);

            // Convert values to CSV data row
            var values = string.Join(",", data.Values);

            // Combine header and data row
            var csv = $"{header}\n{values}";

            return csv;
        }

        public static string Convert(List<Dictionary<string, object>> dataList)
        {
            if (dataList == null || dataList.Count == 0)
                return string.Empty;

            // Assuming all dictionaries have the same keys
            // Convert keys from the first dictionary to CSV header
            var header = string.Join(",", dataList[0].Keys);

            // Convert each dictionary to a row in the CSV
            var rows = new List<string>();
            foreach (var data in dataList)
            {
                var values = string.Join(",", data.Values.Select(v => v == null ? "" : "\"" + v.ToString().Replace("\"", "\"\"") + "\""));
                rows.Add(values);
            }

            // Combine header and rows
            var csv = header + "\n" + string.Join("\n", rows);

            return csv;
        }
    }
}
