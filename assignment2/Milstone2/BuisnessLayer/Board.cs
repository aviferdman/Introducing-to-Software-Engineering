using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using log4net;
using log4net.Config;


namespace Milstone2
{
    class Board
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        Hashtable columnsHashTable = new Hashtable();
        private string boardID;
        private int currColumnCount;


        // constructor - this function construct new board with 3 defulat column (Backlog, In progress and Done)
        // board_ID initialized with the user email
        public Board(String email, String password) 
        {
            columnsHashTable.Add(1, new Column("Backlog"));
            columnsHashTable.Add(2, new Column("in progress"));
            columnsHashTable.Add(3, new Column("Done"));

            this.currColumnCount = 3;
            this.boardID = email;
        }

        // this function returns the amount of columns in the current board
        public int getCurrColumnCount()
        {
            return this.currColumnCount;
        }

        // this function returns the board_ID 
        public string getBoardId()
        {
            return this.boardID;
        }

        // this function returns the HashTable where the columns saved
        public Hashtable getColumns()
        {
            return this.columnsHashTable;
        }

        // this function moves the task from one column to another by removing it from it current column and adding it in the other
        // the add is done by Coulmn class.
        public InfoObject moveTask(int taskID, int status)
        {
            if (status < columnsHashTable.Count)
            {
                if (((Column)columnsHashTable[status]).isContainsKey(taskID))
                {
                    Task t = ((Column)columnsHashTable[status]).removeTask(taskID);
                    InfoObject info = (((Column)columnsHashTable[status + 1]).addTask(t.getTitle(), t.getDescription(), t.getDueDate(), t.getAuthor(), t.getTaskUID()));
                    if (info.getIsSucceeded())
                    {
                        t.statusUpdate();
                        t.save();
                        Log.Info("Board moving task #" + taskID + " from column #" + status + " to next column.");
                        info.setMessage("Board moving task #" + taskID + " from column #" + status + " to next column.");
                        return info;
                    }
                    else
                    {
                        info.setIsSucceeded(false);
                        return info;
                    }
                }
                else
                {
                    Log.Warn("Cant find Task #" + taskID + " to move from column #" + status);
                    InfoObject info3 = new InfoObject(false, "Cant find Task #" + taskID + " to move from column #" + status);
                    return info3;

                }
            }

            Log.Error("You are not allowed to move task #" + taskID + " because it's in column #" + status);
            InfoObject info2 = new InfoObject(false, "You are not allowed to move task #" + taskID + " because it's in column #" + status);
            return info2;
        }
        
        // this function locate the right column and forward it to Coulmn class
        public InfoObject editTaskTitle(int taskID, int status, string newTitle)
        {
            if (status < columnsHashTable.Count)
            {
                return ((Column)columnsHashTable[status]).editTaskTitle(taskID, newTitle);
            }
            else
            {
                Log.Error("Illegal input of task status. Unable to edit task title.");
                InfoObject info = new InfoObject(false, "Illegal input of task status. Unable to edit task title.");
                return info;
            }
        }

        // this function locate the right column and forward it to Coulmn class
        public InfoObject editTaskDescreption(int taskID, int status, string newDescreption)
        {
            if (status < columnsHashTable.Count)
            {
                return ((Column)columnsHashTable[status]).editDescreption(taskID, newDescreption);
            }
            else
            {
                Log.Error("Illegal input of task status. Unable to edit task description.");
                InfoObject info = new InfoObject(false, "Illegal input of task status. Unable to edit description.");
                return info;
            }
        }

        // this function locate the right column and forward it to Coulmn class
        public InfoObject editTaskDueDate(int taskID, int status, string newDueDate)
        {
            if (status < columnsHashTable.Count)
            {
                return ((Column)columnsHashTable[status]).editDueDate(taskID, newDueDate);
            }
            else
            {
                Log.Error("Illegal input of task status. Unable to edit task due date.");
                InfoObject info = new InfoObject(false, "Illegal input of task status. Unable to edit task due date.");
                return info;
            }
        }

        // this function locate the right column and forward it to Coulmn class
        public InfoObject addTask(string title, string description, string author, string dueDate, int taskCounter)
        {
            Column column = (Column)columnsHashTable[1];
            InfoObject info = column.addTask(title, description, dueDate, author, taskCounter);
            return info;
        }

        // this function create a new coulmn and adds it to the Hash table that containes all the columns.
        public InfoObject addColumn(string name)
        {
            InfoObject info;
            if (name != null)
            {
                columnsHashTable.Add(columnsHashTable.Count + 1, new Column(name));
                this.currColumnCount++;
                Log.Info("A new Column named " + name + " added. The new column saved as column #" + this.currColumnCount);
                info = new InfoObject(true, "A new Column named " + name + " added. The new column saved as column number #" + this.currColumnCount);
                return info;
            }
            else
            {
                Log.Warn("The input name is null");
                info = new InfoObject(false, "The input name is null");
                return info;
            }
        }

        // this function changes the maximum capacity of a coulmn by updatinig the Max_Tasks field in column class
        public InfoObject changeColumnCapacity(int columnNumber, int capacity)
        {
            InfoObject info;
            if (columnNumber <= columnsHashTable.Count)
            {
                Column column = (Column)columnsHashTable[columnNumber];

                if (columnNumber != columnsHashTable.Count)
                {
                    return column.setMAX_TASKS(capacity);
                }
                else
                {
                    Log.Warn("Cant change Capacity of the last coulmn");
                    info = new InfoObject(false, "Cant change the capacity of the last column");
                    return info;
                }
            }
            else
            {
                Log.Error("You're trying to change the capacity of an illegal column.");
                info = new InfoObject(false, "You're trying to change the capacity of an illegal column.");
                return info;
            }
        }

        // this function collet all the tasks from data and forward to the specific column they belong to.
        public int open(string author)
        {
            int numberOfTasksAdded = 0;
            TaskD taskD = DataLayerTask.Open(numberOfTasksAdded, author);
            while (taskD != null)
            {
                Column column = (Column)columnsHashTable[taskD.getStatus()];
                column.addTask(taskD.getTitle(), taskD.getDescription(), taskD.getDueDate(), taskD.getAuthor(), taskD.getTaskUID());
                Log.Info("Task #" + numberOfTasksAdded + " loaded successfully from the database.");
                numberOfTasksAdded++;
                taskD = DataLayerTask.Open(numberOfTasksAdded, author);
            }
            return numberOfTasksAdded;
        }

        // this function saves all taks data in a specific coulmn
        public void save()
        {
            for (int i = 1; i <= columnsHashTable.Count; i = i + 1)
            {
                Column column = (Column)columnsHashTable[i];
                column.save();
            }
        }
    }
}