using System;
using log4net;

namespace Milstone2
{
    class User
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(User));

        private string email;
        private string password;
        private Boolean logged_in;
        private Board myBoard;
        private int numOfTaskAdded;
        private string ExistingColumns;

        // User constructor - email and password
        public User(string email, string password)
        {
            this.email = email;
            this.password = password;
            logged_in = false;
            myBoard = new Board(email, password);
            numOfTaskAdded = 0;
            ExistingColumns = "Backlog+In progress+Done+";
        }

        // this function return the email of the current user
        public string getEmail()
        {
            return this.email;
        }

        //this functiom return the board of the current user
        public Board getBoard()
        {
            return myBoard;
        }

        // this function return the password of the current user
        public string getPassword()
        {
            return this.password;
        }

        // this function return the number of tasks added by the current user
        public int getNumOfTasksAdded()
        {
            return numOfTaskAdded;
        }


        public string getExistingColumns()
        {
            return ExistingColumns;
        }

        public void setExistingColumns(string ExistingColumns)
        {
            this.ExistingColumns = ExistingColumns;
        }

        // this function sets the number of tasks added by the current user
        public void setNumberOfTasksAdded(int newNumber)
        {
            this.numOfTaskAdded = newNumber;
        }

        // this function sets the logged_In field to true when logged in and false when logged out
        private void setIsLoggedIn(Boolean isLoggedIn)
        {
            this.logged_in = isLoggedIn;
        }

        // this functin returns true if the current user is logged_in field is true and false if not
        public Boolean isLoggedIn()
        {
            return logged_in;
        }

        // this funtion adds 1 to the number of tasks added field.
        public void taskCounterUpdate()
        {
            this.numOfTaskAdded++;
        }

        // this function return User object from the data layer.
        public static User getByEmail(string email)
        {
            UserD openedUser = DataLayerUser.open(email);
            User returnedUser = null;
            if (openedUser != null)
            {
                returnedUser = new User(openedUser.getEmail(), openedUser.getPassword());
                returnedUser.setIsLoggedIn(openedUser.isLoggedIn());
                returnedUser.setNumberOfTasksAdded(openedUser.getNumOfTasksAdded());
                returnedUser.setExistingColumns(openedUser.getExistingColumns());
            }
            return returnedUser;
        }

        // this function update the logged_in field to true and forward to Board class to open all the tasks.
        public InfoObject login()
        {
            InfoObject info;
            logged_in = true;
            setNumberOfTasksAdded(myBoard.open(getEmail(), getExistingColumns()));
            save();
            Log.Info("The user " + getEmail() + " logged in. ");
            info = new InfoObject(true, "The user " + getEmail() + " logged in. ");
            return info;
        }

        // this function update the logged_in field to false 
        public InfoObject logout()
        {
            InfoObject info;
            logged_in = false;
            save();
            Log.Info("The user " + getEmail() + " logged out.");
            info = new InfoObject(true, "The user " + getEmail() + " logged out.");
            return info;
        }

        // this function checks if the current user is already registerd if not save the user to the data.
        public InfoObject register()
        {
            InfoObject info;
            if (DataLayerUser.open(getEmail()) == null)
            {
                save();
                Log.Info("The user " + getEmail() + " registered.");
                info = new InfoObject(true, "The user " + getEmail() + " registered.");
                return info;
            }
            else
            {
                Log.Info("The user " + getEmail() + " can't register because this email is already in use.");
                info = new InfoObject(false, "The user " + getEmail() + " can't register because this email is already in use.");
                return info;
            }
        }

        // this function checks if the current user is logged in and if he is then forward it to Board class to add the task to the current column.
        public InfoObject addTask(string title, string description, string dueDate)
        {
            if (isLoggedIn())
            {
                InfoObject info = myBoard.addTask(title, description, this.email, dueDate, this.numOfTaskAdded);
                if (info.getIsSucceeded())
                {
                    info.setMessage("Task #" + numOfTaskAdded + " added to the board by the user: " + getEmail());
                    numOfTaskAdded++;
                    save();
                    return info;
                }
                else
                {
                    return info;
                }
            }
            else
            {
                Log.Error("You are not allowed to add a task, please log in first.");
                InfoObject info2 = new InfoObject(false, "You are not allowed to add a task, please log in first.");
                return info2;
            }
        }

        // this function checks if the current user is logged in and if he is forwards it to Board class to move the task to next coulmn.
        public InfoObject moveTask(int taskId, int status)
        {
            if (isLoggedIn())
            {
                return (this.myBoard.moveTask(taskId, status));
            }
            else
            {
                Log.Error("You are not allowed to move a task, please log in first.");
                InfoObject info = new InfoObject(false, "You are not allowed to move a task, please log in first.");
                return info;
            }

        }

        // this function checks if the current user is logged in and if he is forwards to Board class.
        public InfoObject editTaskTitle(int taskId, int status, string newTitle)
        {
            if (isLoggedIn())
            {
                return myBoard.editTaskTitle(taskId, status, newTitle);
            }
            else
            {
                Log.Error("You are not allowed to edit task title, please log in first.");
                InfoObject info = new InfoObject(false, "You are not allowed to edit task title, please log in first.");
                return info;
            }
        }

        // this function checks if the current user is logged in and if he is forwards to Board class.
        public InfoObject editTaskDescreption(int taskId, int status, string newDescreption)
        {
            if (isLoggedIn())
            {
                return myBoard.editTaskDescreption(taskId, status, newDescreption);
            }
            else
            {
                Log.Error("You are not allowed to edit task description, please log in first.");
                InfoObject info = new InfoObject(false, "You are not allowed to edit task title, please log in first.");
                return info;
            }
        }

        // this function checks if the current user is logged in and if he is forwards to Board class.
        public InfoObject editTaskDueDate(int taskId, int status, string newDueDate)
        {
            if (isLoggedIn())
            {
                return myBoard.editTaskDueDate(taskId, status, newDueDate);
            }
            else
            {
                Log.Error("You are not allowed to edit task due date, please log in first.");
                InfoObject info = new InfoObject(false, "You are not allowed to edit task title, please log in first.");
                return info;
            }
        }

        // this function forwards the inputs to Board class to update the capacity of a column
        public InfoObject changeColumnCapacity(string author, int columnNumber, int capacity)
        {
            InfoObject info = myBoard.changeColumnCapacity(author, columnNumber, capacity);
            return info;
        }

        // this function forward the input to Board class to add new coulmn.
        public InfoObject addColumn(string author, string columnName)
        {
            InfoObject info = myBoard.addColumn(author, columnName);
            if (info.getIsSucceeded())
            {
                ExistingColumns = ExistingColumns + "+" + columnName;
                save();
            }
            return info;
        }

        // this function forward the input to Board class to remove coulmn.
        public InfoObject removeColumn(string columnName)
        {
            InfoObject info = myBoard.removeColumn(columnName);
            if (info.getIsSucceeded())
            {
                string[] splited = ExistingColumns.Split('+');
                string newExistingColumns = "";
                for (int i = 0; i < splited.Length; i = i + 1)
                {
                    if (!splited[i].Equals(columnName))
                    {
                        newExistingColumns = newExistingColumns + "" + splited[i] + "+";
                    }
                    else
                    {
                        newExistingColumns = newExistingColumns + "+";
                    }
                }
                setExistingColumns(newExistingColumns);
                save();
            }
            return info;
        }

        public InfoObject moveColumn (int columnNumber)
        {
            InfoObject info = myBoard.moveColumn(columnNumber);
            if (info.getIsSucceeded())
            {
                string moveItForward = myBoard.columnIntToName(columnNumber+1);
                string moveItBackwards = myBoard.columnIntToName(columnNumber);
                string[] split = getExistingColumns().Split('+');
                string[] cleanString = new string[split.Length];
                for (int k = 0; k < cleanString.Length; k = k + 1)
                {
                    cleanString[k] = "";
                }
                string newExistingColumns = "";
                int i = 0;
                foreach (string s in split)
                {
                    if (!s.Equals(""))
                    {
                        cleanString[i] = s;
                        i = i + 1;
                    }
                }
                for(int j = 0; j < i; j = j + 1)
                {
                        if (cleanString[j].Equals(moveItForward))
                        {
                            string temp = cleanString[j];
                            cleanString[j] = cleanString[j+1];
                            cleanString[j + 1] = temp;
                            j = j + 1;
                        }
                }
                for(int j = 0; j < cleanString.Length; j = j + 1)
                {
                    if (!cleanString[j].Equals(""))
                    {
                        newExistingColumns = newExistingColumns + "+" + cleanString[j];
                    }
                }
                setExistingColumns(newExistingColumns);
            }
            save();
            return info;
        }

        public InfoObject moveColumnBack (int columnNumber)
        {
            if (columnNumber == 1)
            {
                return new InfoObject(false, "Impossible to move the first column backwards");
            }
            else
            {
                return moveColumn(columnNumber - 1);
            }
        }

        // this function forward the current user to be saved in the data layer.
        public void save()
        {
            DataLayerUser.save(getEmail(), getPassword(), isLoggedIn(), getNumOfTasksAdded(), getExistingColumns());
            Log.Info("the user " + getEmail() + " saved to the database.");
        }
    }
}
