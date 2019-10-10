namespace Server_WEB_Programming.Lab2.Dal.Migrations
{
    using Bogus;
    using Server_WEB_Programming.Lab2.Dal.Entities;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Server_WEB_Programming.Lab2.Dal.DataBase.SageBookDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Server_WEB_Programming.Lab2.Dal.DataBase.SageBookDbContext context)
        {
            var sageFaker = new Faker<Sage>()
                .RuleFor(x => x.Age, f => f.Random.Number(18, 70))
                .RuleFor(x => x.City, f => f.Address.City())
                .RuleFor(x => x.Name, f => f.Name.FullName())
                .RuleFor(x => x.Books, f => new List<Book>());

            var sages = sageFaker.Generate(100);

            var bookFaker = new Faker<Book>()
                .RuleFor(x => x.Name, f => f.Lorem.Sentence(1, 4))
                .RuleFor(x => x.Description, f => f.Lorem.Lines())
                .RuleFor(x => x.Sages, f => new List<Sage>());

            var books = bookFaker.Generate(100);

            var j = 0;
            var i = 100;
            foreach (var s in sages)
            {
                s.Books.Add(books[j++]);

                s.Books.Add(books[--i]);
            }

            j = 0;
            i = 100;
            foreach (var b in books)
            {
                b.Sages.Add(sages[j++]);

                b.Sages.Add(sages[--i]);
            }

            context.Sages.AddRange(sages);
            context.Books.AddRange(books);

            context.SaveChanges();
        }
    }
}
