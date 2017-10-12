namespace Lab01_SchoolCompetition
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            Dictionary<string, int> scores = new Dictionary<string, int>();
            Dictionary<string, SortedSet<string>> categories = new Dictionary<string, SortedSet<string>>();

            while (true)
            {
                string inputLine = Console.ReadLine();

                if (inputLine == "END")
                {
                    break;
                }

                string[] parts = inputLine.Split(' ');
                string name = parts[0];
                string category = parts[1];
                int score = int.Parse(parts[2]);

                if (!scores.ContainsKey(name))
                {
                    //scores[name] = 0;
                    scores.Add(name, 0);
                }

                if (!categories.ContainsKey(name))
                {
                    //categories[name]= new SortedSet<string>();
                    categories.Add(name, new SortedSet<string>());
                }

                scores[name] += score;
                categories[name].Add(category);
            }

            var orderedStudents = scores
                .OrderByDescending(kvp => kvp.Value)
                .ThenBy(kvp => kvp.Key);

            foreach (var studentKvp in orderedStudents)
            {
                string name = studentKvp.Key;
                int score = studentKvp.Value;
                SortedSet<string> studentCategories = categories[name];

                string categoriesText = $"[{string.Join(", ", studentCategories)}]";

                Console.WriteLine($"{name}: {score} {categoriesText}");
            }
        }
    }
}
