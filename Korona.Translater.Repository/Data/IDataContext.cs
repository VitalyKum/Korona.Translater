using Korona.Translater.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Korona.Translater.Repository.Data
{
    public interface IDataContext
    {
        public List<HandleSchema> GetHandleSchemas();
        public List<TranslateSсhema> GetTranslateSchemas();
    }
}
