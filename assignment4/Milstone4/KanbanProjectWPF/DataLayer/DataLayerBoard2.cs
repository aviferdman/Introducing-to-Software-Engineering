using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanProjectWPF
{
    class DataLayerBoard2
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(DataLayerBoard2));

        public static Boolean save(string name, string author, int numOfTasks)
        {
            if (existBoard(name, author, numOfTasks))
            {
                return updateBoard(name, author, numOfTasks);
            }
            else
            {
                return saveNewBoard(name, author, numOfTasks);
            }
        }

        public static LinkedList<BoardD> Open(string author)
        {
            string connetion_string = null;
            string database_name = "MyDatabase.sqlite";
            SQLiteConnection connection;
            SQLiteCommand command;
            connetion_string = $"Data Source={database_name};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            SQLiteDataReader dataReader;

            LinkedList<BoardD> ListBoardD = new LinkedList<BoardD>();
            BoardD boardD = null;

            try
            {
                connection.Open();

                string sql = "select * from BoardTable WHERE Author = '" + author + "'";
                command = new SQLiteCommand(sql, connection);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    string name = (string)dataReader.GetValue(0);
                    int numOfTasks = (int)((long)dataReader.GetValue(2));
                    boardD = new BoardD(name, author,numOfTasks);
                    ListBoardD.AddLast(boardD);                  
                }
                dataReader.Close();
                command.Dispose();
                Log.Info("Open the Boards of the user: " + author);
                return ListBoardD;
            }
            catch (Exception ex)
            {
                Log.Error("Cant Open the Boards of the user: "+author);
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public static Boolean saveNewBoard(string name,string author, int numOfTasks)
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
                    "INSERT INTO BoardTable (Name,Author,NumberOfTasks) " +
                    "VALUES (@Name,@Author,@NumberOfTasks)";
                SQLiteParameter Name_param = new SQLiteParameter(@"Name", name);
                SQLiteParameter Author_param = new SQLiteParameter(@"Author", author);
                SQLiteParameter NumberOfTasks_param = new SQLiteParameter(@"NumberOfTasks", numOfTasks);

                command.Parameters.Add(Name_param);
                command.Parameters.Add(Author_param);
                command.Parameters.Add(NumberOfTasks_param);

                // Call Prepare after setting the Commandtext and Parameters.
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();
                Log.Info("Save the Board " + name);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Cant save the Board " + name);
                return false;

            }
            finally
            {
                connection.Close();
            }
        }

        public static Boolean updateBoard(string name, string author, int numOfTasks)
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
                command = new SQLiteCommand("UPDATE BoardTable set NumberOfTasks=@NumberOfTasks WHERE Author='" + author + "' AND Name='" + name + "'", connection);
                using (command)
                {
                    SQLiteParameter NumberOfTasks_param = new SQLiteParameter(@"NumberOfTasks", numOfTasks);
                    command.Parameters.Add(NumberOfTasks_param);
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

        public static Boolean existBoard(string name, string author, int numOfTasks)
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
                string sql = "select * from BoardTable WHERE Name='" + name + "' AND Author='" + author + "'";
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

        public static Boolean deleteBoard(string name, string author, int numOfTasks)
        {
            string connetion_string = null;
            string database_name = "MyDatabase.sqlite";

            SQLiteConnection connection;
            SQLiteCommand command;

            connetion_string = $"Data Source={database_name};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            connection.Open();
            command = new SQLiteCommand("DELETE FROM BoardTable WHERE Name=@Name AND Author=@Author", connection);
            command.Parameters.AddWithValue(@"Name", name);
            command.Parameters.AddWithValue(@"Author", author);
            command.Prepare();
            try
            {
                int i = command.ExecuteNonQuery();
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
    }
}
