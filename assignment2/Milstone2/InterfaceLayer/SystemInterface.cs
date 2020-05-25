using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Milstone2.BuisnessLayer;

namespace Milstone2
{
    class SystemInterface
    {
        // this function attempts to login to the system.
        public static Boolean login(string email, string password)
        {
            return UserControler.login(email, password).getIsSucceeded();
        }

        // this function attempts to logout from the system.
        public static Boolean logout(string email)
        {
            return UserControler.logout(email).getIsSucceeded();
        }

        // this function attempts to register new user to the system.
        public static Boolean register(string email, string password)
        {
            return UserControler.register(email, password).getIsSucceeded();
        }

        // this function attempts to add new task to the board.
        public static Boolean addTask(string email, string title, string description, string dueDate)
        {
            return UserControler.addTask(email, title, description, dueDate).getIsSucceeded();
        }

        // this function attempts to edit the title of a task.
        public static Boolean editTaskTitle(string email, int taskID, int status, string newTitle)
        {
            return UserControler.editTaskTitle(email, taskID, status, newTitle).getIsSucceeded();
        }

        // this function attempts to edit the description of a task.
        public static Boolean editTaskDescription(string email, int taskID, int status, string newDescription)
        {
            return UserControler.editTaskDescription(email, taskID, status, newDescription).getIsSucceeded();
        }

        // this function attempts to edit the due date of a task.
        public static Boolean editTaskDueDate(string email, int taskID, int status, string newDueDate)
        {
            return UserControler.editTaskDueDate(email, taskID, status, newDueDate).getIsSucceeded();
        }

        // this functin attempts to forward task to the next coulmn in the board.
        public static Boolean moveTask(string email, int taskID, int status)
        {
            return UserControler.moveTask(email, taskID, status).getIsSucceeded();
        }

        // this function attempts to change the max capacity to a coulmn.
        public static Boolean changeColumnCapacity(string email, int columnNumber, int capacity)
        {
            return UserControler.changeColumnCapacity(email, columnNumber, capacity).getIsSucceeded();
        }

        // this function attempts to add new coulmn to the board.
        public static Boolean addColumn(string email, string columnName)
        {
            return UserControler.addColumn(email,columnName).getIsSucceeded();
        }
    }
}
