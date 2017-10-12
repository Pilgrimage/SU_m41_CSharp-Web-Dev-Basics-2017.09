namespace MyWebServer.ByTheCakeApplication.Data
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class CakesData
    {
        private const string DatabaseFile = @"ByTheCakeApplication\Data\database.csv";

        public void Add(string name, string price)
        {
            StreamReader streamReader = new StreamReader(DatabaseFile);
            int id = streamReader.ReadToEnd().Split(Environment.NewLine).Length;
            streamReader.Dispose();

            using (StreamWriter streamWriter = new StreamWriter(DatabaseFile, true))
            {
                streamWriter.WriteLine($"{id},{name},{price}");
            }
        }

        public IEnumerable<Cake> All()
            => File
                .ReadAllLines(DatabaseFile)
                .Where(line => line.Contains(','))
                .Select(line => line.Split(','))
                .Select(lp => new Cake
                {
                    Id = int.Parse(lp[0]),
                    Name = lp[1],
                    Price = decimal.Parse(lp[2])
                });

        public Cake Find(int id) => this.All().FirstOrDefault(c => c.Id == id);
    }
}