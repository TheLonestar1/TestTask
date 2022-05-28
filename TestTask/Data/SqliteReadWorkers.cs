using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
namespace TestTask.Data
{
    internal class SqliteReadWorkers
    {
        List<Worker> workers;
        public void ReadWorkers(String sqlexp)
        {
            String sqlExpression = sqlexp;
            
            using (var connection = new SQLiteConnection("Data Source = Workersdata.db")) 
            {
                connection.Open();
                workers = new List<Worker>();
                SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);
                using(var reader = command.ExecuteReader())
                {

                    if (reader.HasRows)
                    {
                        
                        int i = 0;
                        while (reader.Read())
                        {
                            workers.Add(new Worker());
                            workers[i].Id = reader.GetInt32(0);
                            workers[i].Name = reader.GetString(1);
                            workers[i].Birthday = reader.GetString(2);
                            workers[i].Gender = reader.GetString(3);
                            workers[i].NameDiv = reader.GetString(4);
                            workers[i].Post = reader.GetString(5);
                            workers[i].Info = reader.GetString(6);
                            i++;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Not rows");
                    }
                }

            }
            
        }
        public Worker[] Worker { get { return workers.ToArray(); } }
    }
}
