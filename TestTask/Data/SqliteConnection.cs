using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;
namespace TestTask.Data
{
    internal class SqliteConnection
    {
        protected SQLiteConnection connection;
        public SqliteConnection()
        {
            using (this.connection = new SQLiteConnection("Data Source = Workersdata.db"))
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "CREATE TABLE IF NOT EXISTS Workers (_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                      " Name TEXT NOT NULL,BirthDate TEXT, Gender TEXT NOT NULL," +
                                      " NameDiv TEXT NOT NULL, Posts TEXT NOT NULL, UniqueInfo TEXT NOT NULL )";
                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Table Workers create");
                }
                catch
                {
                    Console.WriteLine("Table already create");
                }
                connection.Close();
            }
            Console.Read();
            
        }
        public bool Edit(DataGridViewRow dgvr, int i, int j)
        {
            String text = dgvr.Cells[j].Value.ToString();
            using (this.connection = new SQLiteConnection("Data Source = Workersdata.db"))
            {
                connection.Open();
                SQLiteCommand cmd = connection.CreateCommand();
                cmd.CommandText = $"UPDATE Workers ";
                switch (j)
                {
                    case 1:
                        cmd.CommandText += $"SET name = '{text}'";
                        break;
                    case 2:
                        cmd.CommandText += $"SET BirthDay = '{text}'";
                        break;
                    case 3:
                        cmd.CommandText += $"SET Gender = '{text}'";
                        break;
                    case 4:
                        SQLiteCommand readcmd = new SQLiteCommand($"Select Name, NameDiv, Posts From Workers" +
                                                         $" WHERE NameDiv == '{text}' " +
                                                         $" AND Posts == 'Руководитель подразделения';", connection);
                        using (var reader = readcmd.ExecuteReader())
                        {
                            reader.Read();
                            if (!reader.HasRows)
                            {
                                cmd.CommandText += $"SET NameDiv = '{text}'";
                            }
                            else if(dgvr.Cells[5].Value.ToString() != "Руководитель подразделения" &&
                                dgvr.Cells[5].Value.ToString() != "Директор")
                            {
                                cmd.CommandText += $"SET NameDiv = '{text}'";
                            }
                        }
                        
                        break;
                    case 5:
                        readcmd = new SQLiteCommand($"Select Name, NameDiv, Posts From Workers" +
                                                         $" WHERE NameDiv == '{dgvr.Cells[4].Value.ToString()}' " +
                                                         $" AND Posts == '{text}';", connection);
                        using (var reader = readcmd.ExecuteReader())
                        {
                            reader.Read();
                            if (!reader.HasRows)
                            {
                                cmd.CommandText += $"SET Posts = '{text}'";
                            }
                            else if (text != "Руководитель подразделения" &&
                                text != "Директор")
                            {
                                cmd.CommandText += $"SET Posts = '{text}'";
                            }
                        }
                        break;
                    case 6:
                        cmd.CommandText += $"SET UniqueInfo = '{text}'";
                        break;

                }
                cmd.CommandText += $" WHERE _id = {i}";
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
        }
        

        public bool Add(Worker worker)
        {
            string str = worker.Name;
            int count = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Length;
            if (count != 3)
            {
                return false;
            }

            using (var connection = new SQLiteConnection("Data Source=Workersdata.db"))
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                SQLiteCommand readcmd = new SQLiteCommand($"Select Name, NameDiv, Posts From Workers" +
                                                          $" WHERE NameDiv == '{worker.NameDiv}' " +
                                                          $" AND Posts == 'Руководитель подразделения';", connection);
                if(worker.Post == "Руководитель подразделения" )
                {
                    using (var reader = readcmd.ExecuteReader())
                    {
                        reader.Read();
                        if (!reader.HasRows)
                        {
                            command.CommandText = $"INSERT INTO Workers (Name, BirthDate, Gender, NameDiv, Posts, UniqueInfo) " +
                         $"VALUES ('{worker.Name}','{worker.Birthday}'," +
                         $"'{worker.Gender}','{worker.NameDiv}','{worker.Post}','Руководит {worker.NameDiv}')";
                            command.ExecuteNonQuery();
                            return true;
                        }
                        else 
                            return false;
                    }
                }
                else if (worker.Post == "Директор")
                {
                    readcmd.CommandText = $"Select Name, NameDiv, Posts From Workers" +
                                                          $" WHERE NameDiv == '-' " +
                                                          $" AND Posts == 'Директор';";
                    using (var reader = readcmd.ExecuteReader())
                    {
                        reader.Read();
                        if (!reader.HasRows)
                        {
                            command.CommandText = $"INSERT INTO Workers (Name, BirthDate, Gender, NameDiv, Posts, UniqueInfo) " +
                        $"VALUES ('{worker.Name}','{worker.Birthday}'," +
                        $"'{worker.Gender}','-','{worker.Post}','Владеет компанией')";
                            command.ExecuteNonQuery();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                
                    using (var reader = readcmd.ExecuteReader())
                    {
                        reader.Read();
                        if (reader.HasRows)
                        {
                            command.CommandText = $"INSERT INTO Workers (Name, BirthDate, Gender, NameDiv, Posts, UniqueInfo) " +
                            $"VALUES ('{worker.Name}','{worker.Birthday}'," +
                            $"'{worker.Gender}','{worker.NameDiv}','{worker.Post}','{reader.GetString(0)}')";
                            command.ExecuteNonQuery();
                            return true;
                        }
                        else
                        {
                            return false;        
                        }
                    }
                }
                
            }
        }

        public bool Delete(int i)
        {
            using (var connection = new SQLiteConnection("Data Source=Workersdata.db"))
            {
                connection.Open();
                try
                {
                    SQLiteCommand deleteCommand = new SQLiteCommand($"DELETE FROM Workers" +
                                                                    $" WHERE _id = {i};", connection);
                    deleteCommand.ExecuteNonQuery();
                }
                catch
                {
                    return false;
                }
                connection.Close();
                return true;
            }
        }

        public DataTable SelectData(String query)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var connection = new SQLiteConnection("Data Source = Workersdata.db"))
                {
                    connection.Open();
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter();
                    adapter.SelectCommand = new SQLiteCommand(query, connection);
                    adapter.Fill(dt);
                    connection.Close();
                    return dt;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Not selected" + ex.Message);
                return null;
            }
        }
    }
}
