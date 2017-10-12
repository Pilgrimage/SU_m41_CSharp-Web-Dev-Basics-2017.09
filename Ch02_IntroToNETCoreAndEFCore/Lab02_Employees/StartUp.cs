namespace Lab02_Employees
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Lab02_Employees.Models;

    public class StartUp
    {
        public static void Main()
        {
            EmployeesDbContext empContext = new EmployeesDbContext();

            empContext.Database.EnsureDeleted();       // Delete db if existing
            empContext.Database.EnsureCreated();       // create db

            Department department = new Department { Name = "QA Department" };

            for (int i = 0; i < 17; i++)
            {
                department.Employees.Add(new Employee { Name = $"Tester N {i}" });
            }

            empContext.Departments.Add(department);
            //empContext.Add(department);                 // this also works

            empContext.SaveChanges();


            // Exercise for loading data

            int departmentId = 1;

            // Variant 1 - eager loading - Include()
            var readedData1 = empContext
                .Departments
                .Include(d => d.Employees)
                //.ThenInclude( ... )
                .FirstOrDefault(d => d.Id == departmentId);

            // Variant 2 - explicite loading - Load()
            var readedData2 = empContext
                .Departments
                .FirstOrDefault(d => d.Id == departmentId);

            if (departmentId < 100)
            {
                empContext.Entry(readedData2)
                    .Collection(d => d.Employees)
                    .Load();

                // or with filter
                var bEmp = empContext.Entry(readedData2)
                    .Collection(d => d.Employees)
                    .Query()
                    .Where(e => e.Name.StartsWith("B"));
            }

            // Variant 3 - projection - Select()
            var readedData3 = empContext
                    .Departments
                    .Where(d => d.Id == departmentId)
                    .Select(d => new
                    {
                        d.Name,
                        EmpsCount = d.Employees.Count
                    })
                    .FirstOrDefault();
        }
    }
}
