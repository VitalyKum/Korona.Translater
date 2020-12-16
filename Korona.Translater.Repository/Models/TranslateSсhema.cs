using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Korona.Translater.Repository.Models
{ 
    public class TranslateSсhema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public List<DictionaryRecord> Dictionary { get; set; }

        public string Translate(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                throw new ArgumentNullException("expression", "String must have characters" );
            
            if (Dictionary == null)
                throw new ArgumentNullException("Dictionary", "Sсhema's dictionary must be initialized");

            foreach (var rec in this.Dictionary)
                expression = expression.Replace(rec.Original, rec.Translate);
            
            return expression;
        }
    }
}
