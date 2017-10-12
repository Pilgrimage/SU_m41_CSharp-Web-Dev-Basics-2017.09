namespace p01_StudentSystem.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Models;
    using Enums;

    public class StartUp
    {
        // Done

        private static Random random = new Random();

        public static void Main()
        {
            using (StudentSystemDbContext db = new StudentSystemDbContext())
            {
                db.Database.Migrate();

                // SeedData(db);
                // SeedLicenses(db);

                // Task21_PrintStudentsAndHomeworks(db);
                // Task22_PrintCoursesAndResources(db);
                // Task23_PrintCoursesWithMoreThanFiveResources(db);
                // Task24_PrintActiveCourses(db);
                // Task25_PrintStudentsWithPrices(db);

                // Task31_PrintCourseWithResourcesAndLicenses(db);
                // Task32_PrintStudentsWithCoursesAndResourcesAndLicenses(db);
            }
        }


        private static void SeedData(StudentSystemDbContext db)
        {
            const int totalStudents = 30;
            const int totalCourses = 10;
            DateTime currentDate = DateTime.Now;

            // Students
            Console.WriteLine("Adding Students...");

            for (int i = 0; i < totalStudents; i++)
            {
                db.Students.Add(new Student
                {
                    Name = $"Student {i}",
                    RegistrationDate = currentDate.AddDays(i),
                    Birthday = currentDate.AddYears(-22).AddDays(i * i),
                    PhoneNumber = $"Phone number 222-{i * i}-789"
                });
            }

            db.SaveChanges();

            // Courses
            Console.WriteLine("Adding Courses...");

            List<Course> addedCourses = new List<Course>();

            for (int i = 0; i < totalCourses; i++)
            {
                Course course = new Course
                {
                    Name = $"Course 50-{i}",
                    Description = $"Course 50-{i} - Details",
                    Price = 100 * i,
                    StartDate = currentDate.AddDays(i),
                    EndDate = currentDate.AddDays(40 + i)
                };

                addedCourses.Add(course);
                db.Courses.Add(course);
            }

            db.SaveChanges();

            // Students, Resources in Courses
            Console.WriteLine("Adding Students and Resources in Courses...");

            List<int> studentIds = db
                .Students
                .Select(s => s.Id)
                .ToList();

            for (int i = 0; i < totalCourses; i++)
            {
                Course currentCourse = addedCourses[i];

                // Students in Courses
                int studentsInCourse = random.Next(2, totalStudents / 2);

                for (int j = 0; j < studentsInCourse; j++)
                {
                    var studentId = studentIds[random.Next(0, studentIds.Count)];

                    if (!currentCourse.Students.Any(s => s.StudentId == studentId))
                    {
                        currentCourse.Students.Add(new StudentCourse
                        {
                            StudentId = studentId               // EF get CourseId automaticaly !
                        });
                    }
                    else
                    {
                        j--;
                    }
                }

                // Resources in Courses

                int resourcesInCourse = random.Next(2, 20);
                //int[] types = new[] { 0, 1, 2, 999 };     // Simpliest way
                int[] types = Enum.GetValues(typeof(ResourceType)).Cast<int>().ToArray();

                for (int j = 0; j < resourcesInCourse; j++)
                {
                    currentCourse.Resources.Add(new Resource
                    {
                        Name = $"Resource {j} for {currentCourse.Name}",
                        Url = $"URL {j}",
                        Type = (ResourceType)types[random.Next(0, types.Length)]
                    });
                }
            }


            db.SaveChanges();

            // Homeworks
            Console.WriteLine("Adding Homeworks...");

            for (int i = 0; i < totalCourses; i++)
            {
                Course currentCourse = addedCourses[i];
                List<int> studentInCourseIds = currentCourse
                    .Students
                    .Select(s => s.StudentId)
                    .ToList();

                for (int j = 0; j < studentInCourseIds.Count; j++)
                {
                    int totalHomeworks = random.Next(2, 7);

                    for (int k = 0; k < totalHomeworks; k++)
                    {
                        db.Homeworks.Add(new Homework
                        {
                            Content = $"Content of Homework {k}",
                            SubmissionDate = currentDate.AddDays(-10 - i),
                            Type = ContentType.Zip,
                            StudentId = studentInCourseIds[j],
                            CourseId = currentCourse.Id
                        });
                    }
                }
            }

            db.SaveChanges();
        }

        private static void SeedLicenses(StudentSystemDbContext db)
        {
            var resourceIds = db
                .Resources
                .Select(r => r.Id)
                .ToList();

            for (int i = 0; i < resourceIds.Count; i++)
            {
                int totalLicenses = random.Next(2, 7);

                for (int j = 0; j < totalLicenses; j++)
                {
                    db.Licenses.Add(new License
                    {
                        Name = $"License {j} for Course Id={i}",
                        ResourceId = resourceIds[i]
                    });
                }

                db.SaveChanges();
            }
        }


        private static void Task21_PrintStudentsAndHomeworks(StudentSystemDbContext db)
        {
            var taskResult = db
                .Students
                .Select(s => new
                {
                    s.Name,
                    Homeworks = s.Homeworks.Select(h => new
                    {
                        h.Content,
                        h.Type
                    })
                })
                .ToList();

            foreach (var student in taskResult)
            {
                Console.WriteLine(student.Name);

                foreach (var homework in student.Homeworks)
                {
                    Console.WriteLine($"---{homework.Content} - {homework.Type}");
                }
            }
        }

        private static void Task22_PrintCoursesAndResources(StudentSystemDbContext db)
        {
            var taskResult = db
                .Courses
                .OrderBy(c => c.StartDate)
                .ThenByDescending(c => c.EndDate)
                .Select(c => new
                {
                    c.Name,
                    c.Description,
                    Resources = c.Resources.Select(r => new
                    {
                        r.Name,
                        r.Type,
                        r.Url
                    })
                })
                .ToList();

            foreach (var course in taskResult)
            {
                Console.WriteLine($"{course.Name} - {course.Description}");

                foreach (var resource in course.Resources)
                {
                    Console.WriteLine($"--{resource.Name} - {resource.Type} - {resource.Url}");
                }
            }
        }

        private static void Task23_PrintCoursesWithMoreThanFiveResources(StudentSystemDbContext db)
        {
            var taskResult = db
                .Courses
                .Where(c => c.Resources.Count > 5)
                .OrderByDescending(c => c.Resources.Count)
                .ThenByDescending(c => c.StartDate)
                .Select(c => new
                {
                    c.Name,
                    Resources = c.Resources.Count
                })
                .ToList();

            foreach (var course in taskResult)
            {
                Console.WriteLine($"{course.Name} - {course.Resources}");
            }
        }

        private static void Task24_PrintActiveCourses(StudentSystemDbContext db)
        {
            DateTime date = DateTime.Now.AddDays(30);

            var taskResult = db
                .Courses
                .Where(c => c.StartDate < date && date < c.EndDate)
                .Select(c => new
                {
                    c.Name,
                    c.StartDate,
                    c.EndDate,
                    Duration = c.EndDate.Subtract(c.StartDate),
                    Students = c.Students.Count
                })
                .OrderByDescending(c => c.Students)
                .ThenByDescending(c => c.Duration)
                .ToList();

            foreach (var course in taskResult)
            {
                Console.WriteLine($"{course.Name}: {course.StartDate.ToShortDateString()} .. {course.EndDate.ToShortDateString()}");
                Console.WriteLine($"       Duration : {course.Duration.Days} days");
                Console.WriteLine($"       Number of Students: {course.Students}");
                Console.WriteLine(new string('-', 10));
            }

        }

        private static void Task25_PrintStudentsWithPrices(StudentSystemDbContext db)
        {
            var taskResult = db
                .Students
                .Where(s => s.Courses.Any())          // Student must have at least one course
                .Select(s => new
                {
                    s.Name,
                    Courses = s.Courses.Count,
                    TotalPrice = s.Courses.DefaultIfEmpty().Sum(c => c.Course.Price),
                    AveragePrice = s.Courses.DefaultIfEmpty().Average(c => c.Course.Price)
                })
                .OrderByDescending(c => c.TotalPrice)
                .ThenByDescending(c => c.Courses)
                .ThenBy(s => s.Name)
                .ToList();

            foreach (var student in taskResult)
            {
                Console.WriteLine($"{student.Name} - {student.Courses} Courses - {student.TotalPrice:f2} Euro - {student.AveragePrice:f2} Euro");
            }
        }

        private static void Task31_PrintCourseWithResourcesAndLicenses(StudentSystemDbContext db)
        {
            var taskResult = db
                .Courses
                .OrderByDescending(c => c.Resources.Count)
                .ThenBy(c => c.Name)
                .Select(c => new
                {
                    c.Name,
                    Resources = c
                        .Resources
                        .OrderByDescending(r => r.Licenses.Count)
                        .ThenBy(r => r.Name)
                        .Select(r => new
                        {
                            r.Name,
                            Licenses = r.Licenses.Select(l => l.Name)
                        })
                })
                .ToList();

            foreach (var course in taskResult)
            {
                Console.WriteLine(course.Name);

                foreach (var resource in course.Resources)
                {
                    Console.WriteLine($"---{resource.Name}");

                    foreach (string license in resource.Licenses)
                    {
                        Console.WriteLine($"--- * {license}");
                    }
                }
            }
        }

        private static void Task32_PrintStudentsWithCoursesAndResourcesAndLicenses(StudentSystemDbContext db)
        {
            var taskResult = db
                .Students
                .Where(s => s.Courses.Any())
                .Select(s => new
                {
                    s.Name,
                    Courses = s.Courses.Count,
                    Resources = s.Courses.Sum(c => c.Course.Resources.Count),
                    Licenses = s.Courses.Sum(c => c.Course.Resources.Sum(r => r.Licenses.Count))
                })
                .ToList();

            foreach (var student in taskResult)
            {
                Console.WriteLine(student.Name);
                Console.WriteLine($"   Courses: {student.Courses} , Resources: {student.Resources} , Licenses: {student.Licenses}");
            }
        }
    }
}
