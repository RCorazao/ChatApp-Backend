using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Persistence.Helpers
{
    public static class PropertyExtensions
    {
        public static string ToSnakeCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(char.ToLower(str[0]));

            for (int i = 1; i < str.Length; i++)
            {
                if (char.IsUpper(str[i]))
                {
                    stringBuilder.Append('_');
                    stringBuilder.Append(char.ToLower(str[i]));
                }
                else
                {
                    stringBuilder.Append(str[i]);
                }
            }

            return stringBuilder.ToString();
        }
    }
}
