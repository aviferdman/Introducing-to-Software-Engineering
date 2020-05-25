using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using log4net;

namespace Milstone2
{
    class User
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        private string email;
        private string password;
        private Boolean logged_in;
        private Board myBoard;
        private int numOfTaskAdded;
        
        // User constructor - email and password
        public User(string email, string password) 
        {
            Log.Info("New user:" + email + " created.");
            this.email = email;
            this.password = password;
            logged_in = false;
            myBoard = new Board(email, password);
            numOfTaskAdded = 0;
        }


        // this function return the email of the current user
        public string getEmail()
        {
            return this.email;
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
            }
            return returnedUser;
        }

        // this function update the logged_in field to true and forward to Board class to open all the tasks.
        public InfoObject login()
        {
            InfoObject info;
            logged_in = true;
            setNumberOfTasksAdded(myBoard.open(getEmail()));
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
        public InfoObject changeColumnCapacity(int columnNumber, int capacity)
        {
            return myBoard.changeColumnCapacity(columnNumber,capacity);
        }

        // this function forward the input to Board class to add new coulmn.
        public InfoObject addColumn(string columnName)
        {
            return myBoard.addColumn(columnName);
        }

        // this function forward the current user to be saved in the data layer.
        public void save()
        {
            DataLayerUser.save(getEmail(), getPassword(), isLoggedIn(), getNumOfTasksAdded());
            Log.Info("the user " + getEmail() + " saved to the database.");
        }
    }
}
