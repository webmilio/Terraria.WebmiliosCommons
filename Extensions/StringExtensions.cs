using System.Collections.Generic;
using System.Text;

namespace WebmilioCommons.Extensions
{
    public static class StringExtensions
    {
        public static List<string> ParseLine(this string line, char seperator = '"', char classicSeperator = ' ')
        {
            List<string> args = new List<string>();

            StringBuilder sb = new StringBuilder();
            bool dividedArg = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == seperator)
                    dividedArg = !dividedArg;
                else if (c == classicSeperator && !dividedArg)
                {
                    args.Add(sb.ToString());
                    sb.Clear();
                }
                else
                    sb.Append(c);
            }

            args.Add(sb.ToString());

            return args;
        }
    }
}