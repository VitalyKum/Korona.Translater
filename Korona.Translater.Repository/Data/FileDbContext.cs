using Korona.Translater.Repository.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Korona.Translater.Repository.Data
{
    public class FileDbContext : IDataContext
    {
        private static FileDbContext _instance;

        public List<TranslateSсhema> TranslateSсhemas { get; set; }
        public List<HandleSchema> HandleSchemas { get; set; }
        public string ConnectionString { get; private set; }

        private FileDbContext()
        {
            TranslateSсhemas = new List<TranslateSсhema>(DataInitializer.GetStaticSchemas());
            HandleSchemas = new List<HandleSchema>();
        }
        private FileDbContext(string connectionString)
        {
            ConnectionString = connectionString;
            TranslateSсhemas = new List<TranslateSсhema>();
            HandleSchemas = new List<HandleSchema>();

            Initialize();
        }
        private void Initialize()
        {
            
            var files = new DirectoryInfo(ConnectionString).GetFiles("*.*");
            
            foreach (var file in files)
            {
                using (StreamReader sr = new StreamReader(file.FullName, Encoding.UTF8))
                {
                    if (file.Extension == ".ts")
                    {                    
                        var schema = new TranslateSсhema
                        {
                            Name = file.Name.Replace(file.Extension, ""),
                            Description = sr.ReadLine(),
                            Dictionary = new List<DictionaryRecord>()
                        };

                        string[] dict = sr.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var rec in dict)
                        {
                            string[] words = rec.Split("->");

                            if (words.Length == 2)
                                schema.Dictionary.Add(new DictionaryRecord(words[0], words[1]));
                        }
                        TranslateSсhemas.Add(schema);
                    }

                    if (file.Extension == ".hs")
                    {
                        var schema = new HandleSchema
                        {
                            Name = file.Name.Replace(file.Extension, ""),
                            Description = sr.ReadLine(),
                            Rules = new List<string>(
                                sr.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                        };
                        HandleSchemas.Add(schema);
                    }
                }               
            }                     
        }                       

        public static FileDbContext GetInstance(string connectionString)
        {
            if (_instance == null)
                _instance = new FileDbContext(connectionString);

            return _instance;
        }
        public static FileDbContext GetDefaultInstance()
        {
            if (_instance == null)
                _instance = new FileDbContext();

            return _instance;
        }
       
        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public List<HandleSchema> GetHandleSchemas()
        {
            return HandleSchemas;
        }
        public List<TranslateSсhema> GetTranslateSchemas()
        {
            return TranslateSсhemas;
        }
    }
}
