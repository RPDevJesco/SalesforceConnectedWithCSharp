using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesforceConnectedWithCSharp
{
    public static class BatchOperations
    {
        public enum Operations
        {
            [Description("insert")]
            Insert,
            [Description("update")]
            Update,
            [Description("upsert")]
            Upsert,
            [Description("delete")]
            Delete
        }

        public static string GetOperationDescription(Operations operation)
        {
            var type = typeof(Operations);
            var memInfo = type.GetMember(operation.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? ((DescriptionAttribute)attributes[0]).Description : operation.ToString();
        }
    }
}
