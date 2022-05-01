using System.Data;
using System.Text;
using Microsoft.Data.Sqlite;

namespace DataAccessLayer
{
    public class Repository 
    {
        private readonly SqliteConnection _connection;
        public Repository()
        {
            /*Create a connection to an in memory database, 
             * The connection string should be inject from a cofig file 
             * but this is outside the scope of this assignment*/

            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();
            #region //create a table
            
            var sqlCreatetable ="CREATE TABLE Employees" +
                "(ID INTEGER PRIMARY KEY NOT NULL ," +
                "NAME   TEXT NOT NULL," +
                "AGE INT NOT NULL," +
                "SALARY REAL" +
                ");";
            using var cmd = new SqliteCommand(sqlCreatetable, _connection);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            #endregion
        }
        public bool Delete(int id)
        {
            string sql = $"DELETE FROM Employees" +
                $" WHERE ID == {id};";
            return Execute(sql);
        }


        public bool Insert(Employee employee)
        {
            string sql ="INSERT INTO Employees(NAME, AGE, SALARY)" +
                $"VALUES('{employee.Name}', {employee.Age}, {employee.Salary});";
            return Execute(sql);
        }

        public List<Employee> Select(string tableName, params Condition[] conditions)
        {
            var employess = new List<Employee>();
            
            var sb = new StringBuilder(1024);
            for (int i = 1; i < conditions.Length; i++)
            {
                sb.Append($"AND {conditions[i].fieldName} {conditions[i].oper} {conditions[i].value}"+" ");
            }
            string sql = $"SELECT * FROM {tableName}" +
                $" WHERE {conditions[0].fieldName} {conditions[0].oper} {conditions[0].value} {sb};";
            

            using var cmd = new SqliteCommand(sql, _connection);
            using (SqliteDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    employess.Add(new Employee{
                        EmployeeId=(long)dr["ID"],
                        Name=(string)dr["NAME"],
                        Age=(long)dr["AGE"],
                        Salary=(double)dr["SALARY"]});
                }
            }
            return employess;
        }

        public bool Update(int id, Employee current)
        {
            string sql = $"UPDATE Employees" +
                $" SET NAME = '{current.Name}'," +
                $" AGE = {current.Age}," +
                $" SALARY = {current.Salary}" +
                $" WHERE Employees.ID = {id};";
                            
                return Execute(sql);
        }
        
        
        private bool Execute(string sql)
        {
            // Sqlite object will get disposed when this function returns.
            using var cmd = new SqliteCommand(sql, _connection);
            cmd.CommandType = CommandType.Text;
            var result = cmd.ExecuteNonQuery();
            return result != 0;
        }


       

        ~Repository()
        {
            /* connection to the database is an external resource 
             * and is not handled by the garbage collector,
             * we should explictly close and delete it in the finalizer.
            */
            _connection.Close();
            _connection.Dispose();
        }
    }
}
