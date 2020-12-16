using System;
using System.Collections.Generic;
using System.Text;

namespace Korona.Translater.Repository.Models
{
    public class HandleSchema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Rules { get; set; }
    }
}
