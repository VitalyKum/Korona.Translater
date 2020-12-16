using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Korona.Translater.Repository.Models;
using Korona.Translater.Repository.Data;
using Korona.Translater.Services;

namespace Korona.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var outFile = @"D:\MyData\newKorona\Data2\outcsv.csv";
            var procName = @"C:\Program Files (x86)\Microsoft Office\Office14\excel.exe";
            
            var data = new List<string[]>();

            using (StreamReader sr = new StreamReader(@"D:\MyData\newKorona\Data2\inputcsv.csv", 
                CodePagesEncodingProvider.Instance.GetEncoding(1251)))
            {
                int columns = sr.ReadLine().Split(";").Length;
                string[] rows = sr.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);                

                for (int i = 0; i < rows.Length; i++)
                {
                    string[] values = rows[i].Split(";");

                    if (values.Length == columns)
                        data.Add(values);
                    else
                        throw new InvalidDataException($"Количество значений в строке {i} не соответствуют количеству колонок.") ;
                }         
            }

            var rules = new string[] {
                @"ADDRESSLINE=Get(11)->AppendString(', ')->AppendColumn(9)->Exclude(8)->Translate(Adress_schema)->ExcludeExpression(' ,')->ExcludeExpression(',9\d{9}')->ExcludeExpression(',89\d{9}')->ExcludeExpression(',79\d{9}')->ExcludeExpression(',+79\d{9}')",
                "ADRESAT=Get(8)->Translate('FIO_schema')",
                "MASS=0,32",
                "VALUE",
                "PAYMENT",
                "COMMENT=Get(3)->Expression('.{6}$')->AppendString(' ')->AppendColumn(4)->AppendString(' ')->AppendColumn(6)",
                @"TELADDRESS=Get(9)->Expression('9\d{9}')",
                "MAILTYPE=23",
                "MAILCATEGORY",
                "INDEXFROM=664961",
                "VLENGTH",
                "VWIDTH",
                "VHEIGHT",
                "FRAGILE",
                "ENVELOPETYPE",
                "NOTIFICATIONTYPE",
                "COURIER",
                "SMSNOTICERECIPIENT",
                "WOMAILRANK",
                "PAYMENTMETHOD",
                "NOTICEPAYMENTMETHOD",
                "COMPLETENESSCHECKING",
                "NORETURN=1",
                "VSD",
                "TRANSPORTMODE"
            };

            var schema = new HandleSchema
            {
                Name = "test",
                Description = "No description",
                Rules = new List<string>(rules)
            };

            var ch = new ColumnHandler(null, data);

            var outData = ch.ExecuteHandleSchema(schema);           
            
            using (StreamWriter sw = new StreamWriter(outFile, false,
                CodePagesEncodingProvider.Instance.GetEncoding(1251)))
            {
                sw.WriteLine(string.Join(";", outData.Select(x => x.Name).ToArray()));

                int count = outData.FirstOrDefault().Data.Length;
                for (int i = 0; i < count; i++)
                    sw.WriteLine(string.Join(";", outData.Select(x => x.Data[i]).ToArray()));
            }
            if (File.Exists(outFile))
                Process.Start(procName, outFile);
            else
                Console.WriteLine("Output file creating error.");
        }        
    }
}
