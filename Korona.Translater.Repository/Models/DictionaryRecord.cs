using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Korona.Translater.Repository.Models
{
    public class DictionaryRecord
    {
        public DictionaryRecord(string original, string translate)
        {
            Original = original;
            Translate = translate;
        }
        public int Id { get; set; }
        public string Original { get; set; }
        public string Translate { get; set; }
        public int Order { get; set; }
        public int TranslateSchemaId { get; set; }
    }
}
