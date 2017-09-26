using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Convertor
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter the file path.");
            }

            var filePath = args[0];
            var hexaResult= ConvertFileToHexString(filePath, ",");
            Clipboard.SetText(hexaResult);
            
            Console.Read();
        }

        public static string ConvertFileToHexString(string filePath, string delimiter)
        {
            byte[] byteArray = File.ReadAllBytes(filePath);
            string[] stringArray = byteArray.Select(p => p.ToString("x2")).ToArray();

            var stringBuilder = new StringBuilder(stringArray.Length * 2);
            foreach (var value in stringArray)
            {
                stringBuilder.AppendFormat("{0}{1}", "0x", value);
                stringBuilder.Append(delimiter);
            }
            return stringBuilder.ToString();
        }
    }
}
