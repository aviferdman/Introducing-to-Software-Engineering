using System;
using Milstone2.BuisnessLayer;

namespace Milstone2
{
    class SystemInterface
    {
        // this function attempts to login to the system.
        public static InfoObject login(string email, string password)
        {
            return UserControler.login(email, password);
        }

        // this function attempts to logout from the system.
        public static InfoObject logout(string email)
        {
            return UserControler.logout(email);
        }

        // this function attempts to register new user to the system.
        public static InfoObject register(string email, string password)
        {
            return UserControler.register(email, password);
        }

        // this function attempts to add new task to the board.
        public static InfoObject addTask(string email, string title, string description, string dueDate)
        {
            return UserControler.addTask(email, title, description, dueDate);
        }

        // this function attempts to edit the title of a task.
        public static InfoObject editTaskTitle(string email, int taskID, int status, string newTitle)
        {
            return UserControler.editTaskTitle(email, taskID, status, newTitle);
        }

        // this function attempts to edit the description of a task.
        public static InfoObject editTaskDescription(string email, int taskID, int status, string newDescription)
        {
            return UserControler.editTaskDescription(email, taskID, status, newDescription);
        }

        // this function attempts to edit the due date of a task.
        public static InfoObject editTaskDueDate(string email, int taskID, int status, string newDueDate)
        {
            return UserControler.editTaskDueDate(email, taskID, status, newDueDate);
        }

        // this functin attempts to forward task to the next coulmn in the board.
        public static InfoObject moveTask(string email, int taskID, int status)
        {
            return UserControler.moveTask(email, taskID, status);
        }

        // this function attempts to change the max capacity to a coulmn.
        public static InfoObject changeColumnCapacity(string email, int columnNumber, int capacity)
        {
            return UserControler.changeColumnCapacity(email, columnNumber, capacity);
        }

        // this function attempts to add new coulmn to the board.
        public static InfoObject addColumn(string email, string columnName)
        {
            return UserControler.addColumn(email, columnName);
        }

        // this function attempts to remove coulmn from the board.
        public static InfoObject removeColumn(string email, string columnName)
        {
            return UserControler.removeColumn(email, columnName);
        }

        // this function attempts to move coulmn forward in the board.
        public static InfoObject moveColumn(string email, int columnNumber)
        {
            return UserControler.moveColumn(email, columnNumber);
        }

        public static InfoObject moveColumnBack (string email, int columnNumber)
        {
            return UserControler.moveColumnBack(email, columnNumber);
        }


        public static Board getBoard(String user)
        {
            return UserControler.getBoard(user);
        }
    }
}
