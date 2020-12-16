using Korona.Translater.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Korona.Translater.Repository.Data
{
    public class SQLDbContext : DbContext, IDataContext
    {
        public SQLDbContext(DbContextOptions<SQLDbContext> options) : base(options) { }

        public DbSet<DictionaryRecord> DictionaryRecords { get; set; }
        public DbSet<TranslateSсhema> TranslateSchemas { get; set; }
        public DbSet<HandleSchema> HandleSchemas { get; set; }
        
        public List<HandleSchema> GetHandleSchemas()
        {
            return HandleSchemas.ToList();
        }
        public List<TranslateSсhema> GetTranslateSchemas()
        {
            return TranslateSchemas.ToList();
        }
    }
}
