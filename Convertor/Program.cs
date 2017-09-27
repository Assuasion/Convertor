using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            else
            {
                var delimiter = ", ";
                var filePath = args[0];
                var firstItemsNo = args.Length == 3 ? Convert.ToInt32(args[1]) : default(int);
                var lasttItemsNo = args.Length == 3 ? Convert.ToInt32(args[2]) : default(int);
                
                var hexaResult = ConvertFileToHexString(filePath, delimiter, firstItemsNo, lasttItemsNo);
                Clipboard.SetText(hexaResult);
            }                     
        }

        public static string ConvertFileToHexString(string filePath, string delimiter, int firstItemsNo, int lasttItemsNo)
        {
            IEnumerable<byte> byteArray = File.ReadAllBytes(filePath);
            if (firstItemsNo > 0 && lasttItemsNo > 0)
            {
                var rightArray = byteArray.Take(firstItemsNo);
                var leftArray = byteArray.Skip(byteArray.Count() - lasttItemsNo);
                byteArray = rightArray.Concat(leftArray);
            }

            var stringToList = byteArray.Select(p => string.Format("0x{0:X2}", p));
            var chunks = stringToList.ChunkBy(16);
            var lines = chunks.Select(chunk => string.Join(delimiter, chunk));
            var singleLine = string.Join($",{Environment.NewLine}", lines.ToArray());

            return $"{{{singleLine}}}";
        }
    }

    public static class ListExtensions
    {
        public static List<List<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
