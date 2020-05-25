using log4net;
using Milstone2;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace KanbanProjectWPF
{
    class DataLayerColumn2
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(DataLayerColumn2));

        public static Boolean save(string name, int maxCapacity, string author, int location, string boardName)
        {
            if (existColumn(name, maxCapacity, author, location, boardName))
            {
                return updateColumn(name, maxCapacity, author, location, boardName);
            }
            else
            {
                return saveNewColumn(name, maxCapacity, author, location, boardName);
            }
        }

        public static LinkedList<ColumnD> Open(string author, string boardName)
        {
            string connetion_string = null;
            string database_name = "MyDatabase.sqlite";
            SQLiteConnection connection;
            SQLiteCommand command;
            connetion_string = $"Data Source={database_name};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            SQLiteDataReader dataReader;

            LinkedList<ColumnD> ListcolumnD = new LinkedList<ColumnD>();
            ColumnD columnD = null;

            try
            {
                connection.Open();

                string sql = "select * from ColumnTable WHERE BoardName = '" + boardName + "' AND Author ='" + author + "'";
                command = new SQLiteCommand(sql, connection);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {

                        string ColumnName = (string)dataReader.GetValue(0);
                        int maxCapacity = (int)((long)dataReader.GetValue(1));
                        int location = (int)((long)dataReader.GetValue(3));
                        string boardName2 = (string)dataReader.GetValue(4);

                        columnD = new ColumnD(ColumnName, maxCapacity,location, boardName2);
                        ListcolumnD.AddLast(columnD);
                }
                dataReader.Close();
                command.Dispose();
                ListcolumnD = sort(ListcolumnD);
                Log.Info("Open the columns of the board: "+boardName+" of the user: "+author+"succeeded");
                return ListcolumnD;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to open the columns of the board: " + boardName + " of the user: " + author);
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public static Boolean saveNewColumn(string name, int maxCapacity, string author, int location, string boardName)
        {
            string connetion_string = null;
            string database_name = "MyDatabase.sqlite";

            SQLiteConnection connection;
            SQLiteCommand command;

            connetion_string = $"Data Source={database_name};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            connection.Open();
            try
            {

                command = new SQLiteCommand(null, connection);
                command.CommandText =
                    "INSERT INTO ColumnTable (Name,MaxCapacity,Author,Location,BoardName) " +
                    "VALUES (@Name, @MaxCapacity,@Author,@Location,@BoardName)";
                SQLiteParameter Name_param = new SQLiteParameter(@"Name", name);
                SQLiteParameter MaxCapacity_param = new SQLiteParameter(@"MaxCapacity", maxCapacity);
                SQLiteParameter Author_param = new SQLiteParameter(@"Author", author);
                SQLiteParameter Location_param = new SQLiteParameter(@"Location", location);
                SQLiteParameter BoardName_param = new SQLiteParameter(@"BoardName", boardName);


                command.Parameters.Add(Name_param);
                command.Parameters.Add(MaxCapacity_param);
                command.Parameters.Add(Author_param);
                command.Parameters.Add(Location_param);
                command.Parameters.Add(BoardName_param);

                // Call Prepare after setting the Commandtext and Parameters.
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();
                Log.Info("Save column: "+name+" to the board: " + boardName + " of the user: " + author + "succeeded");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to Save column: " + name + " to the board: " + boardName + " of the user: " + author);
                return false;

            }
            finally
            {
                connection.Close();
            }
        }

        public static Boolean updateColumn(string name, int maxCapacity, string author, int location, string boardName)
        {
            string connetion_string = null;
            string database_name = "MyDatabase.sqlite";

            SQLiteConnection connection;
            SQLiteCommand command;

            connetion_string = $"Data Source={database_name};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            connection.Open();
            try
            {
                command = new SQLiteCommand("UPDATE ColumnTable set MaxCapacity=@MaxCapacity, Location=@Location WHERE Author='" + author + "' AND Name='" + name + "' AND BoardName= '" + boardName + "'", connection);
                using (command)
                {
                    SQLiteParameter MaxCapacity_param = new SQLiteParameter(@"MaxCapacity", maxCapacity);
                    SQLiteParameter Location_param = new SQLiteParameter(@"Location", location);
                    command.Parameters.Add(MaxCapacity_param);
                    command.Parameters.Add(Location_param);
                    command.Prepare();
                    command.ExecuteNonQuery();
                    command.Dispose();
                    return true;
                }

            }

            catch (Exception ex)
            {
                return false;

            }
            finally
            {
                connection.Close();
            }
        }

        public static Boolean existColumn(string name, int maxCapacity, string author, int location, string boardName)
        {
            string connetion_string = null;
            string database_name = "MyDatabase.sqlite";

            SQLiteConnection connection;

            connetion_string = $"Data Source={database_name};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            SQLiteCommand c = null;
            SQLiteDataReader reader = null;
            connection.Open();
            try
            {
                string sql = "select * from ColumnTable WHERE Name='" + name + "' AND Author='" + author + "' AND BoardName='" + boardName + "'";
                c = new SQLiteCommand(sql, connection);
                reader = c.ExecuteReader();
                if (reader.Read())
                {
                    reader.Close();
                    return true;
                }
                reader.Close();
                return false;
            }

            catch (Exception ex)
            {
                return false;

            }
            finally
            {
                c.Dispose();
                connection.Close();
            }
        }

        public static Boolean deleteColumn(String email, String columnName, String boardName)
        {
            string connetion_string = null;
            string database_name = "MyDatabase.sqlite";

            SQLiteConnection connection;
            SQLiteCommand command;

            connetion_string = $"Data Source={database_name};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            connection.Open();
            command = new SQLiteCommand("DELETE FROM ColumnTable WHERE Name=@Name AND Author=@Author AND BoardName=@BoardName", connection);
            command.Parameters.AddWithValue(@"Name", columnName);
            command.Parameters.AddWithValue(@"Author", email);
            command.Parameters.AddWithValue(@"BoardName", boardName);
            command.Prepare();
            try
            {
                int i= command.ExecuteNonQuery();
                command.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;

            }
            finally
            {
                connection.Close();
            }
        }

        public static LinkedList<ColumnD> sort(LinkedList<ColumnD> list)
        {
            LinkedList<ColumnD> sortedList = new LinkedList<ColumnD>();
            int count = 1;
            while(count<= list.Count)
            {
                foreach(ColumnD c in list)
                {
                    if (c.getLocation() == count)
                    {
                        sortedList.AddLast(c);
                        count = count + 1;
                    }
                }
            }
            return sortedList;
        }
    }
}
