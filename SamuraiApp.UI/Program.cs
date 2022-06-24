using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SamuraiApp.UI
{
    internal class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        static void Main(string[] args)
        {
            InsertNewSamurai();
            FindSamuraiIncludeQuotes();
            GetSamurais("Hello");
            Console.Write("Press Any Key...");
            Console.ReadKey();

        }
        public static void AddSamuraisByName(params string[] names)    
        {
            foreach (string name in names)
            {
                _context.Samurais.Add(new Samurai { Name = name});
            }

            _context.SaveChanges();
        }

        private static void AddVariousTypes()
        {
            _context.AddRange(
                new Samurai { Name = "Chingis Xan" },
                new Samurai { Name = "Qwertis Xan" },
                new Battle { Name = "Battle of Seb" },
                new Battle { Name = "Battle of LA" }
                );
            _context.SaveChanges();
        }
        private static void GetSamurais(string text)
        {
            var samurais = _context.Samurais
                .TagWith($"This is seb's comment added")
                .ToList();
            Console.WriteLine($"{text}: Samurai Count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        private static void FilterSamurais(string name)
        {
            var samurais = _context.Samurais.Where(x => x.Name.Contains(name)).ToList();
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }
        private static void FirstOrDefault(string name)
        {
            //var samurai = _context.Samurais.FirstOrDefault(x => x.Name.Contains(name));
            var samurai = _context.Samurais.Find(2);

            Console.WriteLine(samurai.Name);
        }

        private static void RetrieveAndUpdateSamurai()
        {
            var samurais = _context.Samurais.Skip(8).Take(5).ToList();
            samurais.ForEach(x => x.Name += "Xan Li 2");
            _context.SaveChanges();
        }

        private static void QueryAndUpdateBattles_Disconnected()
        {
            List<Battle> disconnectedBattles;
            using (var context1 = new SamuraiContext())
            {
                disconnectedBattles = context1.Battles.ToList();
            }
            disconnectedBattles.ForEach(b =>
            {
                b.StartDate = new DateTime(1530, 02, 05);
                b.EndDate = new DateTime(1471, 02, 09);
            });
            using (var context2 = new SamuraiContext())
            {
                context2.UpdateRange(disconnectedBattles);
                context2.SaveChanges();
            }
        }

        private static void InsertNewSamurai()
        {
            var samurai = new Samurai
            {
                Name = "SpeakingSebNinja123",
                Quotes = new List<Quote>
                {
                    new Quote {Text = "can I park you car ching chang?"}
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void InsertToExisting(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote
            {
                Text = "I repair your iphone"
            });
            _context.SaveChanges();
        }

        private static void InsertNewSamuraiUntracked(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote
            {
                Text = "Manufacturing Ching Chang"
            });

            using (var newContext = new SamuraiContext())
            {
                newContext.Samurais.Attach(samurai);
                newContext.SaveChanges();
            }
        }
        private static void FindSamuraiIncludeQuotes()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes.Where(q => q.Text.Contains("iphone"))).ToList();

            //var samurai = _context.Samurais.Include(s => s.Quotes).ToList();

        }
    }
}
