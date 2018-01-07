using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.utils
{
    class StringUtils
    {
        public static string ToValidFileName(string text)
        {
            char[] textChars = text.ToCharArray();

            textChars = Array.FindAll<char>(textChars, (c => (
                                                                char.IsLetterOrDigit(c)
                                                                || char.IsWhiteSpace(c)
                                                                || c == '-'
                                                                || c == '_'
                                                                )));

            return new string(textChars);
        }

        public static string ToDatabaseUserString(string text)
        {
            return text.Replace("'", "''");
        }
    }
}
