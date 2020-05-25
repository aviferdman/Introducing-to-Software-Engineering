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
    public class DataLayerUser2
    {

        static readonly ILog Log = LogManager.GetLogger(typeof(DataLayerUser2));

        public static Boolean save (string email, string password, Boolean logged_in, int numOfTasksAdded, string lastBoardUsed)
        {
            if (existUser(email, password, logged_in, numOfTasksAdded, lastBoardUsed))
            {
                return updateUser(email, password, logged_in, numOfTasksAdded, lastBoardUsed);
            }
            else
            {
                return saveNewUser(email, password, logged_in, numOfTasksAdded, lastBoardUsed);
            }
        }

        public static UserD open(string email)
        {
            string connetion_string = null;
            string database_name = "MyDatabase.sqlite";
            SQLiteConnection connection;
            SQLiteCommand command;
            connetion_string = $"Data Source={database_name};Version=3;";
            connection = new SQLiteConnection(connetion_string);
            SQLiteDataReader dataReader;

            UserD userD = null;

            try
            {

                connection.Open();

                string sql = "select * from UserTable WHERE UserID ='"+email+"'";
                command = new SQLiteCommand(sql, connection);
                dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                        Boolean login;
                        if(dataReader["Login"].Equals("true"))
                        {
                            login = true;
                        }
                        else
                        {
                            login = false;
                        }
                        string userName = (string)dataReader.GetValue(0);
                        string password = (string)dataReader.GetValue(1);
                        int numOfTasks = (int)((long)dataReader.GetValue(3));
                        string lastBoard = (string)dataReader.GetValue(4);
                        userD = new UserD(userName, password,login, numOfTasks, lastBoard);
                }
                dataReader.Close();
                command.Dispose();
                Log.Info("Open user: "+email);
                return userD;
            }
            catch (Exception ex)
            {
                Log.Info("Failed to Open user: " + email);
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public static Boolean saveNewUser(string email, string password, Boolean logged_in, int numOfTasksAdded, string lastBoardUsed)
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
                    "INSERT INTO UserTable (UserID,UserPassword,Login,NumberOfTasks,LastBoard) " +
                    "VALUES (@UserID, @UserPassword,@Login,@NumberOfTasks,@LastBoard)";
                SQLiteParameter UserID_param = new SQLiteParameter(@"UserID", email);
                SQLiteParameter UserPassword_param = new SQLiteParameter(@"UserPassword", password);
                string login;
                if (logged_in)
                {
                    login = "true";
                }
                else
                {
                    login = "false";
                }
                SQLiteParameter Login_param = new SQLiteParameter(@"Login", login);
                SQLiteParameter NumberOfTasks_param = new SQLiteParameter(@"NumberOfTasks", numOfTasksAdded);
                SQLiteParameter LastBoard_param = new SQLiteParameter(@"LastBoard", lastBoardUsed);


                command.Parameters.Add(UserID_param);
                command.Parameters.Add(UserPassword_param);
                command.Parameters.Add(Login_param);
                command.Parameters.Add(NumberOfTasks_param);
                command.Parameters.Add(LastBoard_param);

                // Call Prepare after setting the Commandtext and Parameters.
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();
                Log.Info("Save user: " + email);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to save user: " + email);
                return false;

            }
            finally
            {
                connection.Close();
            }
        }

        public static Boolean updateUser(string email, string password, Boolean logged_in, int numOfTasksAdded, string lastBoardUsed)
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
                command = new SQLiteCommand("UPDATE UserTable set Login=@Login, NumberOfTasks=@NumberOfTasks, LastBoard=@LastBoard WHERE UserID='" + email + "'", connection);
                using (command)
                {
                    SQLiteParameter UserID_param = new SQLiteParameter(@"UserID", email);
                    SQLiteParameter UserPassword_param = new SQLiteParameter(@"UserPassword", password);
                    string login;
                    if (logged_in)
                    {
                        login = "true";
                    }
                    else
                    {
                        login = "false";
                    }
                    SQLiteParameter Login_param = new SQLiteParameter(@"Login", login);
                    SQLiteParameter NumberOfTasks_param = new SQLiteParameter(@"NumberOfTasks", numOfTasksAdded);
                    SQLiteParameter LastBoard_param = new SQLiteParameter(@"LastBoard", lastBoardUsed);


                    command.Parameters.Add(UserID_param);
                    command.Parameters.Add(UserPassword_param);
                    command.Parameters.Add(Login_param);
                    command.Parameters.Add(NumberOfTasks_param);
                    command.Parameters.Add(LastBoard_param);
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

        public static Boolean existUser (string email, string password, Boolean logged_in, int numOfTasksAdded, string lastBoardUsed)
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
                string sql = "select UserID from UserTable WHERE UserID = '" + email + "'";
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

        public static Boolean removeUser(string email)
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
                string sql = "DELETE FROM UserTable WHERE UserID='" + email + "'; DELETE FROM BoardTable WHERE Author='" + email + "'; DELETE FROM ColumnTable WHERE Author='" + email + "'; DELETE FROM TaskTable WHERE Author ='" + email + "'";
                command = new SQLiteCommand(sql, connection);
                using (command)
                {
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
    }
}
