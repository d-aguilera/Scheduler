using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Common
{
    public class Helpers
    {
        public static string GetFullExceptionMessage(string title, Exception ex, IDictionary<string, object> args = null)
        {
            var sb = new StringBuilder();
            sb.AppendLine(title);

            sb.Append("Arguments: ");
            if (null != args && args.Count > 0)
            {
                var argsJson = JsonConvert.SerializeObject(args, Formatting.Indented);
                sb.AppendLine();
                sb.AppendLine(argsJson);
            }
            else
                sb.AppendLine("None.");

            var inner = ex;
            while (null != inner)
            {
                sb.AppendLine();
                sb.AppendLine("Message: " + inner.Message);
                sb.AppendLine("Stack trace:");
                sb.AppendLine(inner.StackTrace);
                inner = inner.InnerException;
            }

            return sb.ToString();
        }

        public static string ToBase64(string plain)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plain));
        }

        public static string FromBase64(string encoded)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
        }

        public static string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
