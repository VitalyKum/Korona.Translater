using Korona.Translater.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Korona.Translater.Repository.Data
{
    public static class DataInitializer
    {
        public static async void InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var context = new SQLDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<SQLDbContext>>()))
            {
                if (context.TranslateSchemas.Any())
                {
                    return;
                }               
                
                context.TranslateSchemas.Add(GetBaseSchema());
                await context.SaveChangesAsync();
                
                context.TranslateSchemas.Add(GetAdressSchema());
                await context.SaveChangesAsync();

                context.TranslateSchemas.Add(GetFIOSchema());
                context.SaveChanges();

                context.TranslateSchemas.Add(GetPunctiationSchema());
                context.SaveChanges();
            }
        }

        public static TranslateSсhema GetBaseSchema()
        {
            var schema = new TranslateSсhema
            {
                Id = 0,
                Name = "base_schema",
                Description = "Основная схема перевода",
                Dictionary = new List<DictionaryRecord>
                {
                    new DictionaryRecord("Ty", "Ты"),
                    new DictionaryRecord("Sy", "Сы"),
                    new DictionaryRecord("Ry", "Ры"),
                    new DictionaryRecord("By", "Бы"),
                    new DictionaryRecord("Dy", "Ды"),
                    new DictionaryRecord("ty", "ты"),
                    new DictionaryRecord("sy", "сы"),
                    new DictionaryRecord("ry", "ры"),
                    new DictionaryRecord("by", "бы"),
                    new DictionaryRecord("dy", "ды"),
                    new DictionaryRecord("Ch", "Ч"),
                    new DictionaryRecord("Sh", "Ш"),
                    new DictionaryRecord("Kh", "Х"),
                    new DictionaryRecord("H", "Х"),
                    new DictionaryRecord("Eh", "Э"),
                    new DictionaryRecord("A", "А"),
                    new DictionaryRecord("B", "Б"),
                    new DictionaryRecord("V", "В"),
                    new DictionaryRecord("G", "Г"),
                    new DictionaryRecord("D", "Д"),
                    new DictionaryRecord("E", "Е"),
                    new DictionaryRecord("Jo", "Ё"),
                    new DictionaryRecord("Zh", "Ж"),
                    new DictionaryRecord("Schc", "Щ"),
                    new DictionaryRecord("Shh", "Щ"),
                        new DictionaryRecord("Sch", "Щ"),
                        new DictionaryRecord("Oye", "ое"),
                        new DictionaryRecord("Sc", "Ск"),
                        new DictionaryRecord("Zw", "Цв"),
                        new DictionaryRecord("Yu", "Ю"),
                        new DictionaryRecord("Ju", "Ю"),
                        new DictionaryRecord("Ya", "Я"),
                        new DictionaryRecord("Ch", "Ч"),
                        new DictionaryRecord("Sh", "Ш"),
                        new DictionaryRecord("Kh", "Х"),
                        new DictionaryRecord("H", "Х"),
                        new DictionaryRecord("Eh", "Э"),
                        new DictionaryRecord("A", "А"),
                        new DictionaryRecord("B", "Б"),
                        new DictionaryRecord("V", "В"),
                        new DictionaryRecord("G", "Г"),
                        new DictionaryRecord("D", "Д"),
                        new DictionaryRecord("E", "Е"),
                        new DictionaryRecord("Jo", "Ё"),
                        new DictionaryRecord("Zh", "Ж"),
                        new DictionaryRecord("Z", "З"),
                        new DictionaryRecord("I", "И"),
                        new DictionaryRecord("Y", "Й"),
                        new DictionaryRecord("K", "К"),
                        new DictionaryRecord("L", "Л"),
                        new DictionaryRecord("M", "М"),
                        new DictionaryRecord("N", "Н"),
                        new DictionaryRecord("O", "О"),
                        new DictionaryRecord("P", "П"),
                        new DictionaryRecord("R", "Р"),
                        new DictionaryRecord("S", "С"),
                        new DictionaryRecord("T", "Т"),
                        new DictionaryRecord("U", "У"),
                        new DictionaryRecord("F", "Ф"),
                        new DictionaryRecord("C", "Ц"),
                        new DictionaryRecord("'", "Ъ"),
                        new DictionaryRecord("I", "Ы"),
                        new DictionaryRecord("'", "Ь"),
                        new DictionaryRecord("J", ""),
                        new DictionaryRecord("schc", "Щ"),
                        new DictionaryRecord("shh", "Щ"),
                        new DictionaryRecord("sch", "щ"),
                        new DictionaryRecord("oye", "ое"),
                        new DictionaryRecord("sc", "ск"),
                        new DictionaryRecord("Zw", "цв"),
                        new DictionaryRecord("yu", "ю"),
                        new DictionaryRecord("ju", "ю"),
                        new DictionaryRecord("ya", "я"),
                        new DictionaryRecord("yy ", "ый "),
                        new DictionaryRecord("yo", "ё"),
                        new DictionaryRecord("ye ", "ье "),
                        new DictionaryRecord("ch", "ч"),
                        new DictionaryRecord("sh", "ш"),
                        new DictionaryRecord("kh", "х"),
                        new DictionaryRecord("iy ", "ий "),
                        new DictionaryRecord("ey ", "ей "),
                        new DictionaryRecord("oy ", "ой "),
                        new DictionaryRecord("ay ", "ай "),
                        new DictionaryRecord("ia ", "я "),
                        new DictionaryRecord("jo", "ё"),
                        new DictionaryRecord("zh", "ж"),
                        new DictionaryRecord("eh", "э"),
                        new DictionaryRecord("h", "х"),
                        new DictionaryRecord("a", "а"),
                        new DictionaryRecord("b", "б"),
                        new DictionaryRecord("v", "в"),
                        new DictionaryRecord("g", "г"),
                        new DictionaryRecord("d", "д"),
                        new DictionaryRecord("e", "е"),
                        new DictionaryRecord("z", "з"),
                        new DictionaryRecord("i", "и"),
                        new DictionaryRecord("y", "й"),
                        new DictionaryRecord("k", "к"),
                        new DictionaryRecord("l", "л"),
                        new DictionaryRecord("m", "м"),
                        new DictionaryRecord("n", "н"),
                        new DictionaryRecord("o", "о"),
                        new DictionaryRecord("p", "п"),
                        new DictionaryRecord("r", "р"),
                        new DictionaryRecord("s", "с"),
                        new DictionaryRecord("t", "т"),
                        new DictionaryRecord("u", "у"),
                        new DictionaryRecord("f", "ф"),
                        new DictionaryRecord("c", "ц"),
                        new DictionaryRecord("'", "ъ"),
                        new DictionaryRecord("i", "ы"),
                        new DictionaryRecord("'", "ь"),
                        new DictionaryRecord("j", ""),
                        new DictionaryRecord("Ци", "Цы"),
                        new DictionaryRecord("ци", "цы")
                }
            };

            int index = 0;
            foreach (var rec in schema.Dictionary)
                rec.Order = index++;

            return schema;
        }
        public static TranslateSсhema GetAdressSchema()
        {
            var schema = new TranslateSсhema
            {
                Id = 1,
                Name = "adress_schema",
                Description = "Схема перевода адресов",
                Dictionary = new List<DictionaryRecord>
                    {
                        new DictionaryRecord("kv.", "кв. "),
                        new DictionaryRecord("kv ", "кв. "),
                        new DictionaryRecord("Room ", "кв. "),
                        new DictionaryRecord("room ", "кв. "),
                        new DictionaryRecord("flat ", "кв. "),
                        new DictionaryRecord("Flat ", "кв. "),
                        new DictionaryRecord("kvartira ", "кв. "),
                        new DictionaryRecord("Kvartira ", "кв. "),
                        new DictionaryRecord("street ", "ул. "),
                        new DictionaryRecord("Street ", "ул. "),
                        new DictionaryRecord("ulitsa ", "ул. "),
                        new DictionaryRecord("Ulitsa ", "ул. "),
                        new DictionaryRecord("pereulok", "пер. "),
                        new DictionaryRecord("Pereulok", "пер. "),
                        new DictionaryRecord("dom ", "д. "),
                        new DictionaryRecord("Dom ", "д. "),
                        new DictionaryRecord("house ", "д. "),
                        new DictionaryRecord("House ", "д. "),
                        new DictionaryRecord("prospect", "пр."),
                        new DictionaryRecord("prospekt", "пр."),
                        new DictionaryRecord(" kray ", " кр. "),
                        new DictionaryRecord("Olga ", "Ольга "),
                        new DictionaryRecord("Moscow", "Москва"),
                        new DictionaryRecord("moscow", "Москва"),
                        new DictionaryRecord("Saint Petersburg", "Санкт-Петербург"),
                        new DictionaryRecord("Saint-Petersburg", "Санкт-Петербург"),

                        new DictionaryRecord("RUSSIA", ""),
                        new DictionaryRecord("Russia", ""),
                        new DictionaryRecord("russia", ""),
                        new DictionaryRecord("oblast", "обл.,"),
                        new DictionaryRecord("Oblast", "обл.,"),
                        new DictionaryRecord("autonomus", ""),
                        new DictionaryRecord("avtonomnyy", ""),
                        new DictionaryRecord("autonomous", ""),
                        new DictionaryRecord("republic", "респ."),
                        new DictionaryRecord("respublika", "респ."),
                        new DictionaryRecord("okrug", "АО"),
                        new DictionaryRecord("schoolroom", "Школьная")
                    }
            };
            
            int index = 0;
            foreach (var rec in schema.Dictionary)
                rec.Order = index++;

            return schema;
        }
        public static TranslateSсhema GetFIOSchema()
        {
            var schema = new TranslateSсhema
            {
                Id = 2,
                Name = "fio_schema",
                Description = "Схема перевода ФИО",
                Dictionary = new List<DictionaryRecord> {new DictionaryRecord("y ", "ий ") }
            };

            int index = 0;
            foreach (var rec in schema.Dictionary)
                rec.Order = index++;

            return schema;
        }
        public static TranslateSсhema GetPunctiationSchema()
        {
            var schema = new TranslateSсhema
            {
                Id = 3,
                Name = "punctuation_schema",
                Description = "Схема замены знаков препинания",
                Dictionary = new List<DictionaryRecord>
                {
                    new DictionaryRecord(",,", ","),
                    new DictionaryRecord(", ,", ",")
                }
            };

            int index = 0;
            foreach (var rec in schema.Dictionary)
                rec.Order = index++;

            return schema;
        }
        public static TranslateSсhema[] GetStaticSchemas()
        {
            return new List<TranslateSсhema>
            {
                GetBaseSchema(),
                GetAdressSchema(),
                GetFIOSchema(),
                GetPunctiationSchema()
            }.ToArray();
        }
    }
}
