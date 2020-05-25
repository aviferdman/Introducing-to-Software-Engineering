using log4net;
using System.Collections;

namespace Milstone2.DataLayer
{
    public class UserControler
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(UserControler));
        private static Hashtable currUsers = new Hashtable();

        // this function forwards the input for validatoin check and for User class to login.
        // if the it passed all checks the user will be added to the logged in users hash table.
        public static InfoObject login(string email, string password)
        {

            InfoObject info;
            Log.Info("User: " + email + " log in attempt with password: " + password);
            if (IsValid.IsValidUser(email, password))
            {
                User user = User.getByEmail(email);
                if (user != null && user.getPassword() == password)
                {
                    user.login();
                    currUsers.Add(email, user);
                    info = new InfoObject(true, "");
                }
                else
                {
                    Log.Error("This user doesn't exist. Wrong email or password.");
                    info = new InfoObject(false, "This user doesn't exist. Wrong email or password.");
                }
            }
            else
            {
                Log.Error("Login of " + email + " failed. Illegal password");
                info = new InfoObject(false, "Login of " + email + " failed. Illegal password or email");
            }
            return info;
        }

        // this function forwards the email of the user to User class to logout, and remove the user from the logged in users hash table.
        public static InfoObject logout(string email)
        {
            InfoObject info;
            Log.Info("Logout by " + email);
            User CurrUser = (User)currUsers[email];
            if (CurrUser != null)
            {
                CurrUser.logout();
                currUsers.Remove(email);
                info = new InfoObject(true, "");
            }
            else
            {
                Log.Error("Logout of " + email + " failed.");
                info = new InfoObject(false, "Logout of " + email + " failed.");
            }
            return info;
        }

        // tgus function forwards the inputs for validatoin check and for User class to register.
        public static InfoObject register(string email, string password)
        {
            InfoObject info;
            Log.Info("New user registration attempt. Email: " + email + ", password: " + password);
            if (IsValid.IsValidUser(email, password))
            {
                User user = new User(email, password);
                user.register();
                info = new InfoObject(true, "");
            }
            else
            {
                Log.Error("Invalid input. Registration failed.");
                info = new InfoObject(false, "Invalid input. Registration failed.");
            }
            return info;
        }

        // this function forwards the inputs for validations check and then forwards to User class
        public static InfoObject addTask(string email, string title, string description, string dueDate )
        {
            Log.Info("Add new task request by: " + email + ". Task: " + title);
            User CurrUser = (User)currUsers[email];
            if (CurrUser != null && (IsValid.IsValidTaskTitle(title) & IsValid.IsValidTaskDescreption(description) & IsValid.IsValidTaskDueDate(dueDate)))
            {
                return CurrUser.addTask(title, description, dueDate);
            }
            else
            {
                Log.Error("Adding task failed.");
                InfoObject info = new InfoObject(false, "Adding task failed.");
                return info;
            }
        }

        // this function forwards the inputs for validations check and then forwards to User class
        public static InfoObject editTaskTitle(string email, int taskID, int status, string newTitle)
        {
            Log.Info("Edit existing task title request by: " + email + ". Task ID: " + taskID);
            User CurrUser = (User)currUsers[email];
            if (CurrUser != null && IsValid.IsValidTaskTitle(newTitle))
            {
                return CurrUser.editTaskTitle(taskID, status, newTitle);
            }
            else
            {
                Log.Error("Edit task title failed. the new title is over 50 chars");
                InfoObject info = new InfoObject(false, "Edit task title failed. Illegal title.");
                return info;
            }
        }

        // this function forwards the inputs for validations check and then forwards to User class
        public static InfoObject editTaskDescription(string email, int taskID, int status, string newDescription)
        {
            Log.Info("Edit existing task description request by: " + email + ". Task ID: " + taskID);
            User CurrUser = (User)currUsers[email];
            if (CurrUser != null && IsValid.IsValidTaskDescreption(newDescription))
            {
                return CurrUser.editTaskDescreption(taskID, status, newDescription);
            }
            else
            {
                Log.Error("Edit task discription failed. the new description is over 300 chars");
                InfoObject info = new InfoObject(false, "Edit task title failed. the new description is over 300 chars");
                return info;
            }
        }

        // this function forwards the inputs for validations check and then forwards to User class
        public static InfoObject editTaskDueDate(string email, int taskID, int status, string newDueDate)
        {
            Log.Info("Edit existing task due date request by: " + email + ". Task ID: " + taskID);
            User CurrUser = (User)currUsers[email];
            if (CurrUser != null && IsValid.IsValidTaskDueDate(newDueDate))
            {
                return CurrUser.editTaskDueDate(taskID, status, newDueDate);
            }
            else
            {
                Log.Error("Edit task due date failed. invadlid date or date has been past");
                InfoObject info = new InfoObject(false, "Edit task title failed. invadlid date or date has been past");
                return info;
            }
        }

        // this function forwards the inputs for User class
        public static InfoObject moveTask(string email, int taskID, int status)
        {
            Log.Info("Forward task request by: " + email + ". Task ID: " + taskID);
            User CurrUser = (User)currUsers[email];
            if (CurrUser != null)
            {
                return CurrUser.moveTask(taskID, status);
            }
            else
            {
                Log.Error("Moving task failed.");
                InfoObject info = new InfoObject(false, "Moving task failed. invadlid  task input");
                return info;
            }
        }

        // this function forwards the inputs to User class
        public static InfoObject changeColumnCapacity(string email, int columnNumber, int capacity)
        {
            Log.Info("Update column capacity request by: " + email + ". Column number: " + columnNumber);
            User CurrUser = (User)currUsers[email];
            if (CurrUser != null)
            {
                return CurrUser.changeColumnCapacity(email, columnNumber, capacity);
            }
            else
            {
                Log.Error("Column capacity change failed.");
                InfoObject info = new InfoObject(false, "Column capacity change failed.");
                return info;
            }
        }

        // this function forwards the inputs to User class
        public static InfoObject addColumn(string email, string columnName)
        {
            Log.Info("Add new column request by: " + email);
            User CurrUser = (User)currUsers[email];
            if (CurrUser != null & columnName != null)
            {
                return CurrUser.addColumn(email, columnName);
            }
            else
            {
                Log.Error("Adding new column failed.");
                InfoObject info = new InfoObject(false, "Adding new column failed.");
                return info;
            }
        }

        // this function forwards the inputs to User class
        public static InfoObject removeColumn(string email, string columnName)
        {
            Log.Info("remove column request by: " + email);
            User CurrUser = (User)currUsers[email];
            if (CurrUser != null & columnName != null)
            {
                return CurrUser.removeColumn(email,columnName);
            }
            else
            {
                Log.Error("remove column failed.");
                InfoObject info = new InfoObject(false, "remove column failed.");
                return info;
            }
        }

        // this function forwards the inputs to User class
        public static InfoObject moveColumn(string email, int columnNumber)
        {
            Log.Info("move column request by: " + email);
            User CurrUser = (User)currUsers[email];
            if (CurrUser != null)
            {
                return CurrUser.moveColumn(columnNumber);
            }
            else
            {
                Log.Error("move column failed.");
                InfoObject info = new InfoObject(false, "move column failed.");
                return info;
            }
        }

        public static InfoObject moveColumnBack(string email, int columnNumber)
        {
            Log.Info("move column request by: " + email);
            User CurrUser = (User)currUsers[email];
            if (CurrUser != null)
            {
                return CurrUser.moveColumnBack(columnNumber);
            }
            else
            {
                Log.Error("move column failed.");
                InfoObject info = new InfoObject(false, "move column failed.");
                return info;
            }
        }

        //this function forwards the inputs to User class
        public static Board getBoard(string email)
        {
            Log.Info("get board by: " + email);
            User CurrUser = (User)currUsers[email];
            if (CurrUser != null)
            {
                return CurrUser.getBoard();
            }
            else
            {
                Log.Error("get board failed.");
                return null;
            }
        }

        public static Hashtable getBoards(string email)
        {
            Log.Info("get boards by: " + email);
            User CurrUser = (User)currUsers[email];
            if (CurrUser != null)
            {
                return CurrUser.getBoards();
            }
            else
            {
                Log.Error("get board failed.");
                return null;
            }
        }

        public static InfoObject AddBoard(string email,string boardToAdd)
        {
            Log.Info("add new Board: " + boardToAdd);
            User CurrUser = (User)currUsers[email];
            if (CurrUser != null)
            {
                CurrUser.addBoard(boardToAdd);
                InfoObject info = new InfoObject(true, "Added new board " + boardToAdd);
                return info;
            }
            else
            {
                Log.Error("get board failed.");
                InfoObject info = new InfoObject(false, "Added new board failed");
                return info;
            }
        }

        public static InfoObject setCurrBaord(string email, string currBoardName)
        {
            User CurrUser = (User)currUsers[email];
            if (CurrUser != null)
            {
                CurrUser.setCurrBoard(currBoardName);
                InfoObject info = new InfoObject(true, "Current board set to : " + currBoardName);
                return info;
            }
            else
            {
                Log.Error("get board failed.");
                InfoObject info = new InfoObject(false, "Current board didnt updtaded");
                return info;
            }
        }

        public static InfoObject removeBoard (string email, string BoardName)
        {
            User CurrUser = (User)currUsers[email];
            if (CurrUser != null)
            {
                return CurrUser.removeBoard(BoardName);
            }
            else
            {
                Log.Error("remove board failed, couldn't find the user");
                InfoObject info = new InfoObject(false, "remove board failed, couldn't find the user");
                return info;
            }
        }

    }
}


