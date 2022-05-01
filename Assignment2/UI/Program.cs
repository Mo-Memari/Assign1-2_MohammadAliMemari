using DataAccessLayer;
using System.Text.Json;

 
var repo = new Repository();
var employeeList = new List<Employee>{
  new Employee{EmployeeId=1, Name="John", Age= 36, Salary= 10000 },
  new Employee{EmployeeId=2, Name="Joe", Age= 32, Salary= 12000 },
  new Employee{EmployeeId=3, Name="Jeff", Age= 33, Salary= 14000 },
  new Employee{EmployeeId=4, Name="Brad", Age= 34, Salary= 17000 },
  new Employee{EmployeeId=5, Name="Barron", Age= 38, Salary= 20000 },
  new Employee{EmployeeId=6, Name="Bart", Age= 39, Salary= 19000 },
  new Employee{EmployeeId=7, Name="Lily", Age= 41, Salary= 100000 }};


foreach (var employee in employeeList)
{
    repo.Insert(employee);
}



var employees = repo.Select("Employees", new Condition { fieldName = "ID", oper = "<", value = "10" });

var employeesJson = JsonSerializer
    .Serialize<List<Employee>>(employees);

Console.WriteLine(employeesJson);

var updateSucceeded = repo
    .Update(1, new Employee{EmployeeId=1, Age=33, Name="Mohammad Ali", Salary = 5000001});


Console.WriteLine(updateSucceeded? "Item Updated":"Update failed");

var employeesUpdated = repo
    .Select("Employees", new Condition{fieldName="ID",oper="=", value="1"});

var employeesUpdatedJson = JsonSerializer
    .Serialize<List<Employee>>(employeesUpdated);
Console.WriteLine(employeesUpdatedJson);


var deletionSucceeded = repo.Delete(3);
Console.WriteLine(deletionSucceeded?"Item Deleted":"Deletion failed");

var employeesDeleted = repo.Select("Employees",
    new Condition{fieldName="ID", oper="<", value="4"},
    new Condition{fieldName="ID", oper=">", value="2"});


string employeesDeletedJson = JsonSerializer.Serialize<List<Employee>>(employeesDeleted);
Console.WriteLine(employeesDeletedJson);

Console.ReadKey();



