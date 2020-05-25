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
    class DataLayerTask2
    {

        static readonly ILog Log = LogManager.GetLogger(typeof(DataLayerTask2));

        public static Boolean save(string title, string description, int status, string dueDate, string creationDate, string author, int taskID, string boardName)
        {
            if (existTask(title, description, status, dueDate, creationDate, author, taskID, boardName))
            {
                return updateTask(title, description, status, dueDate, creationDate, author, taskID, boardName);
            }
            else
            {
                return saveNewTask(title, description, status, dueDate, creationDate, author, taskID, boardName);
            }
        }

        public static LinkedList<TaskD> Open(string author, string boardName)
        {
            string connetion_string = null;
            string database_name = "MyDatabase.sqlite";
            SQLiteConnection connection;
            SQLiteCommand command;
            connetion_string = $"Data Source={database_name};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            SQLiteDataReader dataReader;

            LinkedList < TaskD > listTaskD = new LinkedList<TaskD>();
            TaskD taskD = null;

            try
            {
                connection.Open();

                string sql = "select * from TaskTable WHERE BoardName = '" + boardName + "' AND Author ='" + author + "'";
                command = new SQLiteCommand(sql, connection);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    //if (((int)((long)dataReader["TaskID"]))==taskID)
                    {

                        string titleD = (string)dataReader.GetValue(0);
                        string descriptionD = (string)dataReader.GetValue(1);
                        int statusD = (int)((long)dataReader.GetValue(2));
                        string dueDateD = (string)dataReader.GetValue(3);
                        string creationDateD = (string)dataReader.GetValue(4);
                        string authorD= (string)dataReader.GetValue(5);
                        int taskIDD = (int)((long)dataReader.GetValue(6));
                        string boardNameD= (string)dataReader.GetValue(7);

                        taskD = new TaskD(titleD,descriptionD,statusD,dueDateD,creationDateD,authorD,taskIDD,boardNameD);
                        listTaskD.AddLast(taskD);
                    }
                }
                dataReader.Close();
                command.Dispose();
                Log.Info("the tasks in board: "+boardName+"of the user: "+author+"opened successfuly");
                return listTaskD;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to open the tasks in board: " + boardName + "of the user: " + author);
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public static Boolean saveNewTask(string title, string description, int status, string dueDate, string creationDate, string author, int taskID, string boardName)
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
                    "INSERT INTO TaskTable (Title,Description,Status,DueDate,CreationDate,Author,TaskID,BoardName) " +
                    "VALUES (@Title, @Description,@Status,@DueDate,@CreationDate,@Author,@TaskID,@BoardName)";
                SQLiteParameter Title_param = new SQLiteParameter(@"Title", title);
                SQLiteParameter Description_param = new SQLiteParameter(@"Description", description);
                SQLiteParameter Status_param = new SQLiteParameter(@"Status", status);
                SQLiteParameter DueDate_param = new SQLiteParameter(@"DueDate", dueDate);
                SQLiteParameter CreationDate_param = new SQLiteParameter(@"CreationDate", creationDate);
                SQLiteParameter Author_param = new SQLiteParameter(@"Author", author);
                SQLiteParameter TaskID_param = new SQLiteParameter(@"TaskID", taskID);
                SQLiteParameter BoardName_param = new SQLiteParameter(@"BoardName", boardName);


                command.Parameters.Add(Title_param);
                command.Parameters.Add(Description_param);
                command.Parameters.Add(Status_param);
                command.Parameters.Add(DueDate_param);
                command.Parameters.Add(CreationDate_param);
                command.Parameters.Add(Author_param);
                command.Parameters.Add(TaskID_param);
                command.Parameters.Add(BoardName_param);

                // Call Prepare after setting the Commandtext and Parameters.
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();
                Log.Info("Save task #: "+taskID+" in board: " + boardName + "of the user: " + author + " successfuly");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to save task #: " + taskID + " in board: " + boardName + "of the user: " + author);
                return false;

            }
            finally
            {
                connection.Close();
            }
        }

        public static Boolean updateTask(string title, string description, int status, string dueDate, string creationDate, string author, int taskID, string boardName)
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
                command = new SQLiteCommand("UPDATE TaskTable set Title=@Title,Description=@Description,Status=@Status,DueDate=@DueDate WHERE Author='" + author + "' AND TaskID='" + taskID + "' AND BoardName= '" + boardName + "'", connection);
                using (command)
                {
                    SQLiteParameter Title_param = new SQLiteParameter(@"Title", title);
                    SQLiteParameter Description_param = new SQLiteParameter(@"Description", description);
                    SQLiteParameter Status_param = new SQLiteParameter(@"Status", status);
                    SQLiteParameter DueDate_param = new SQLiteParameter(@"DueDate", dueDate);

                    command.Parameters.Add(Title_param);
                    command.Parameters.Add(Description_param);
                    command.Parameters.Add(Status_param);
                    command.Parameters.Add(DueDate_param);

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

        public static Boolean existTask(string title, string description, int status, string dueDate, string creationDate, string author, int taskID, string boardName)
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
                string sql = "select * from TaskTable WHERE TaskID='" + taskID + "' AND Author='" + author + "' AND BoardName='" + boardName + "'";
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
    }
}
