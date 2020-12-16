using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Korona.Translater.Repository.Models;
using Korona.Translater.Repository.Data;

namespace Korona.Translater.Services
{
    public class ColumnHandler
    {
        private readonly IDataContext _context;
        private int ColumnsCount { get; set; }
        private IEnumerable<string[]> Table { get; set; }        
        private string[] ColumnData { get; set; }

        public ColumnHandler(IDataContext context, IEnumerable<string[]> data)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (data == null || data.Count() == 0)
                throw new ArgumentNullException(nameof(data));

            _context = context;

            Table = data.Select(row => row.Select(x => x.Replace("\n", ", ").Replace("\"", "")).ToArray());
            ColumnsCount = data.First().Length;            
        }
        private ColumnHandler Get(int column)
        {
            //Все столбцы обрабатываются с индекса 1
            ColumnData = GetColumnData(column);

            return this;
        }
        private ColumnHandler AppendColumn(int column)
        {
            if (ColumnData == null)
            {
                throw new ArgumentNullException(
                    "ColumnData",
                    "Initialization ColumnData's required. Before use methods(Get, Translate, etc.)");
            }
            
            var jd = JoinColumnData(new int[] { column });

            if (jd.Length != ColumnData.Length)
            {
                throw new ArgumentOutOfRangeException(
                    "ColumnCoumt",
                    "Handled array of data must be eqals Column's data array");
            }

            for (int i = 0; i < ColumnData.Length; i++)
            {                
                ColumnData[i] = $"{ColumnData[i]}{jd[i]}";
            }
            return this;
        }
        private ColumnHandler AppendString(string val)
        {
            if (ColumnData == null)
            {
                throw new ArgumentNullException(
                    "ColumnData",
                    "Initialization ColumnData's required. Before use methods(Get, Translate, etc.)");
            }

            if (string.IsNullOrEmpty(val))
                throw new ArgumentNullException("val");
            

            ColumnData = ColumnData.Select(x => $"{x}{val}").ToArray();

            return this;
        }
        private ColumnHandler Exclude(int column)
        {
            if (ColumnData == null)
            {
                throw new ArgumentNullException(
                    "ColumnData",
                    "Initialization ColumnData's required. Before use methods(Get, Translate, etc.)");
            }
            //Все столбцы обрабатываются с индекса 1
            if (column < 1 || column > ColumnsCount)
                throw new IndexOutOfRangeException("Value must be between 1 and count of columns");

            var ed = GetColumnData(column);
            
            for (int i = 0; i < ColumnData.Length; i++)
                ColumnData[i] = ColumnData[i].Replace(ed[i], "");
            
            return this;
        }
        private ColumnHandler Join(int column)
        {
            ColumnData = JoinColumnData(new int[]{ column});

            return this;
        }
        private ColumnHandler Translate(TranslateSсhema schema)
        {
            if (schema == null)
                throw new ArgumentNullException("schema", "TranslateSсhema must be initialized");
            
            ColumnData = ColumnData.Select(x => schema.Translate(x)).ToArray();
            
            return this;
        }
        private ColumnHandler Expression(string regex)
        {
            if (ColumnData == null)
            {
                throw new ArgumentNullException(
                    "ColumnData",
                    "Initialization ColumnData's required. Before use methods(Get, Translate, etc.)");
            }
            
            if (string.IsNullOrEmpty(regex))
                throw new ArgumentNullException("regex");

            var reg = new Regex(regex);

            ColumnData = ColumnData.Select(x => 
                string.Join(", ", reg.Matches(x).Select(r => r.Value).ToArray()))
                .ToArray();

            return this;
        }
        private ColumnHandler ExcludeExpression(string regex)
        {
            if (ColumnData == null)
            {
                throw new ArgumentNullException(
                    "ColumnData",
                    "Initialization ColumnData's required. Before use methods(Get, Translate, etc.)");
            }

            if (string.IsNullOrEmpty(regex))
                throw new ArgumentNullException("regex");

            var reg = new Regex(regex);
            for (int i = 0; i < ColumnData.Length; i++)
            {
                string[] excl = reg.Matches(ColumnData[i]).Select(r => r.Value).ToArray();
                foreach (var ex in excl)
                    ColumnData[i] = ColumnData[i].Replace(ex, "");
            }            

            return this;
        }

        private string[] JoinColumnData(int[] columns)
        {
            if (columns == null || columns.Length == 0)
                throw new ArgumentException("Array must be initialized", "columns");
            
            //Все столбцы обрабатываются с индекса 1
            if (columns.Any(x => x < 1 || x > ColumnsCount))
                throw new IndexOutOfRangeException("All values in array must be between 1 and count of columns");

            return Table.Select(x =>
            {
                var sb = new StringBuilder();
                foreach (int c in columns)
                    sb.Append(x[c-1]);
                return sb.ToString();
            }).ToArray();
        }
        private string[] GetColumnData(int column)
        {
            //Все столбцы обрабатываются с индекса 1
            if (column < 1 || column > ColumnsCount)
                throw new IndexOutOfRangeException("Value must be between 1 and count of columns");

            return Table.Select(x => x[column-1]).ToArray();
        }
        private (string, string[]) HandleByRule(string rule)
        {
            if (!rule.Contains("="))
                return (rule, new string[Table.Count()]);
            
            var name = rule.Split("=")[0];
            var handlers = rule.Split("=")[1].Split("->");
            
            foreach (var h in handlers)
            {
                int paramIndx = h.IndexOf("(") + 1;
                var stParam = h.Substring(paramIndx, h.Length - 1 - paramIndx);
                stParam = stParam.Replace("'", "");

                if (h.Contains("Get"))
                {
                    if (int.TryParse(stParam, out int param))
                        this.Get(param);
                    else
                        throw new ArgumentException("Invalid parameter in Get(int columnNumber)");
                }
                else if (h.Contains("AppendString"))
                {
                    if (!string.IsNullOrEmpty(stParam))
                        this.AppendString(stParam);
                    else
                        throw new ArgumentException("Invalid parameter in AppendString(string val)");
                }
                else if (h.Contains("AppendColumn"))
                {
                    if (int.TryParse(stParam, out int param))
                        this.AppendColumn(param);
                    else
                        throw new ArgumentException("Invalid parameter in Append(int columnNumber)");
                }
                else if (h.Contains("ExcludeExpression"))
                {
                    if (string.IsNullOrEmpty(stParam))
                        throw new ArgumentException("Invalid parameter in Expression(string regularExpression)");
                    this.ExcludeExpression(stParam);
                }
                else if (h.Contains("Exclude"))
                {
                    if (int.TryParse(stParam, out int param))
                        this.Exclude(param);
                    else
                        throw new ArgumentException("Invalid parameter in Exclude(int columnNumber)");
                }
                else if (h.Contains("Join"))
                {
                    if (int.TryParse(stParam, out int param))
                        this.Join(param);
                    else
                        throw new ArgumentException("Invalid parameter in Join(int columnNumber)");
                }
                else if (h.Contains("Translate"))
                {
                    var schema = _context.GetTranslateSchemas().FirstOrDefault(x => x.Name == stParam);
                    if (schema == null)
                        throw new ArgumentException("Invalid parameter in Translate(string schemaName)");
                    this.Translate(schema);
                }
                else if (h.Contains("Expression"))
                {
                    if (string.IsNullOrEmpty(stParam))
                        throw new ArgumentException("Invalid parameter in Expression(string regularExpression)");
                    this.Expression(stParam);
                }               
                else
                {
                    this.ColumnData = new string[Table.Count()].Select(x => { return handlers[0]; }).ToArray();
                }
            }

            return (name, this.ColumnData);
        }

        public IEnumerable<ColumnRecord> ExecuteHandleSchema(HandleSchema schema)
        {
            var outData = new List<ColumnRecord>();

            foreach (var rule in schema.Rules)
            {
                (var cName, var cData) = HandleByRule(rule);
                outData.Add(new ColumnRecord { Name = cName, Data = cData });
            }

            return outData;
        }
    }
}
