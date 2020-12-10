using System.Globalization;
using Newtonsoft.Json.Serialization;

namespace ShwasherSys
{
    public class BigCamelCasePropertyNamesContractResolver : CamelCasePropertyNamesContractResolver
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver" /> class.
        /// </summary>
        public BigCamelCasePropertyNamesContractResolver()
        {
            CamelCaseNamingStrategy caseNamingStrategy = new BigCamelCaseNamingStrategy();
            NamingStrategy = caseNamingStrategy;
        }


    }

    public class BigCamelCaseNamingStrategy : CamelCaseNamingStrategy
    {
        protected override string ResolvePropertyName(string name)
        {
            return ToBigCamelCase(name);
        }

        public static string ToBigCamelCase(string s)
        {
            if (string.IsNullOrEmpty(s) || !char.IsLower(s[0]))
                return s;
            char[] charArray = s.ToCharArray();
            for (int index = 0; index < charArray.Length && (index != 1 || char.IsUpper(charArray[index])); ++index)
            {
                bool flag = index + 1 < charArray.Length;
                if (!(index > 0 & flag) || char.IsUpper(charArray[index + 1]))
                {
                    char uper = char.ToUpper(charArray[index], CultureInfo.InvariantCulture);
                    charArray[index] = uper;
                }
                else
                    break;
            }
            return new string(charArray);
        }
    }
}
