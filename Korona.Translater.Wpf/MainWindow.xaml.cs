using Korona.Translater.Repository.Data;
using Korona.Translater.Repository.Models;
using Korona.Translater.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Korona.Translater.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly FileDbContext _context;
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                _context = FileDbContext.GetInstance($"{AppContext.BaseDirectory}\\data");
                WriteLog($"Initializing FileDbContext: OK!");
                
            }
            catch (Exception)
            {
                WriteLog($"FileDbContext error: {Environment.NewLine}" +
                    $"not found system files (*.ts,*.hs) in {AppContext.BaseDirectory}\\data");

                _context = FileDbContext.GetDefaultInstance();
                
                if(_context != null)
                    WriteLog($"Warning! Loaded default translate schemas!");
                else
                    WriteLog($"Error! Can not to load default translate schemas!");
            }
            
            WriteLog($"Loading {_context.TranslateSсhemas.Count} translate's schemas: {string.Join(", ", _context.TranslateSсhemas.Select(x => x.Name))}");
            WriteLog($"Loading {_context.HandleSchemas.Count} handle's chemas: {string.Join(", ", _context.HandleSchemas.Select(x => x.Name))}");

            ComboBoxRules.ItemsSource = _context.HandleSchemas;
            ComboBoxRules.DisplayMemberPath = "Description";
            ComboBoxRules.SelectedItem = _context.HandleSchemas.FirstOrDefault();
        }

        private void ButtonChooseFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "CSV Files(*.csv)| *.csv";

            if (d.ShowDialog(this) == true)
                TextBoxsourceFile.Text = d.FileName;            
        }

        private void WriteLog(string msg)
        {
            TextBoxLog.Text += $"{msg}{Environment.NewLine}";
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            var sourceFile = TextBoxsourceFile.Text;
            var outFile = sourceFile.Replace(".csv", "_out.csv");

            var schema = ComboBoxRules.SelectedItem as HandleSchema;
            
            TextBoxLog.Text = string.Empty;

            if (string.IsNullOrEmpty(sourceFile))
            {
                WriteLog("Choose source file");
                return;
            }

            if (!File.Exists(sourceFile))
            {
                WriteLog("Source file not found");
                return;
            }
            if (schema == null)
            {
                WriteLog("Choose handle's schema");
                return;
            }
            if (_context.TranslateSсhemas == null || _context.TranslateSсhemas.Count() == 0)
            {
                WriteLog("Not found translate schemas");
                return;
            }

            if (_context.HandleSchemas == null || _context.HandleSchemas.Count() == 0)
            {
                WriteLog("Not found handle schemas");
                return;
            }
            
            var inputData = new List<string[]>();

            using (StreamReader sr = new StreamReader(TextBoxsourceFile.Text,
                CodePagesEncodingProvider.Instance.GetEncoding(1251)))
            {
                int columns = sr.ReadLine().Split(";").Length;
                string[] rows = sr.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < rows.Length; i++)
                {
                    string[] values = rows[i].Split(";");

                    if (values.Length == columns)
                        inputData.Add(values);
                    else
                    {
                        WriteLog($"Out of range in row {i}");
                        return;
                    }
                }
            }
            try
            {
                var handler = new ColumnHandler(_context, inputData);                                
                var outData = handler.ExecuteHandleSchema(schema);
                
                if (outData == null || outData.Count() == 0)
                {
                    WriteLog("Handler error.");
                    return;
                }

                using (StreamWriter sw = new StreamWriter(outFile, false,
                CodePagesEncodingProvider.Instance.GetEncoding(1251)))
                {
                    sw.WriteLine(string.Join(";", outData.Select(x => x.Name).ToArray()));

                    int count = outData.FirstOrDefault().Data.Length;
                    for (int i = 0; i < count; i++)
                        sw.WriteLine(string.Join(";", outData.Select(x => x.Data[i]).ToArray()));
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
                return;
            }

            var procName = @"C:\Program Files (x86)\Microsoft Office\Office14\excel.exe";

            if (File.Exists(outFile))
            {
                Process.Start(procName, outFile);
                WriteLog($"Handle {sourceFile} with schema {schema.Name} complete succesfully.");
            }
            else
                Console.WriteLine("Output file creating error.");

        }

        private void ButtonEditHandleSchema_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxRules.SelectedIndex != -1)
            {
                var file = $"{_context.ConnectionString}\\" +
                    $"{(ComboBoxRules.SelectedItem as HandleSchema).Name}.hs";
                Process.Start("notepad.exe",file);
            }
        }
    }
}
