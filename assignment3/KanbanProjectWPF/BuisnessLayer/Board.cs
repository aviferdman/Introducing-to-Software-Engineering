using System;
using System.Collections;
using log4net;
using System.Collections.Generic;

namespace Milstone2
{
    public class Board
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(Board));

        Hashtable columnsHashTable = new Hashtable();
        private string boardID;
        private int currColumnCount;


        // constructor - this function construct new board with 3 defulat column (Backlog, In progress and Done)
        // board_ID initialized with the user email
        public Board(String email, String password)
        {
            columnsHashTable.Add(1, new Column("Backlog"));
            columnsHashTable.Add(2, new Column("In progress"));
            columnsHashTable.Add(3, new Column("Done"));
            if (DataLayerColumn.Open("Backlog", email) == null)
            {
                DataLayerColumn.save("Backlog", int.MaxValue, email, "Backlog");
            }
            if (DataLayerColumn.Open("In progress", email) == null)
            {
                DataLayerColumn.save("In progress", int.MaxValue, email, "In progress");
            }
            if (DataLayerColumn.Open("Done", email) == null)
            {
                DataLayerColumn.save("Done", int.MaxValue, email, "Done");
            }
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
            return columnsHashTable;
        }

        // this function moves the task from one column to another by removing it from it current column and adding it in the other
        // the add is done by Coulmn class.
        public InfoObject moveTask(int taskID, int status)
        {
            if (status < columnsHashTable.Count)
            {
                if (((Column)columnsHashTable[status]).isContainsKey(taskID))
                {
                    if (((Column)columnsHashTable[status+1]).canAddTask())
                    {
                        Task t = ((Column)columnsHashTable[status]).removeTask(taskID);
                        InfoObject info = (((Column)columnsHashTable[status + 1]).moveTask(t.getTitle(), t.getDescription(), status, t.getDueDate(), t.getAuthor(), t.getTaskUID()));
                        if (info.getIsSucceeded())
                        {
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
                        return new InfoObject(false, "The next column is full.");
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
            InfoObject info2 = new InfoObject(false, "You are not allowed to move task #" + taskID + " because it's in the last column");
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
            InfoObject info;
            if (column != null)
            {
                info = column.addTask(title, description, dueDate, author, taskCounter);
            }
            else
            {
                info = new InfoObject(false, "there are no columns. Please add column before you add new task.");
            }
            return info;
        }

        // this function create a new coulmn and adds it to the Hash table that containes all the columns.
        public InfoObject addColumn(string author, string name)
        {
            InfoObject info;
            if (name != null)
            {
                Column column = new Column(name);
                columnsHashTable.Add(columnsHashTable.Count + 1, column);
                DataLayerColumn.save(name, int.MaxValue, author, name);
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

        //this function removing a column from the board
        public InfoObject removeColumn(String columnName)
        {
            InfoObject info;
            for (int i = 1; i <= columnsHashTable.Count; i = i + 1)
            {
                Column column = (Column)columnsHashTable[i];
                if (column.getName() == columnName)
                {
                    if (column.isPossibleToRemove().getIsSucceeded())
                    {
                        Log.Info("column #" + i + " has been removed");
                        info = new InfoObject(true, "column #" + i + " has been removed");
                        getColumns().Remove(i);
                        updateColumnHash(i + 1);
                        return info;
                    }
                    else
                    {
                        Log.Warn("column #" + i + " has not been removed");
                        info = new InfoObject(false, "column #" + i + " has not been removed");
                        return info;
                    }
                }
            }
            Log.Warn("column " + columnName + " can't be found");
            info = new InfoObject(false, "column " + columnName + " can't be found");
            return info;
        }
        public void updateColumnHash(int index)
        {
            for (int i = index; i <= currColumnCount; i = i + 1)
            {
                Column column = (Column)columnsHashTable[i];
                if (column != null)
                {
                    column.updateTaskHash();
                    getColumns().Add(i - 1, column);
                    getColumns().Remove(i);
                }
            }
            currColumnCount = currColumnCount - 1;
        }

        // this function changes the maximum capacity of a coulmn by updatinig the Max_Tasks field in column class
        public InfoObject changeColumnCapacity(string author, int columnNumber, int capacity)
        {
            InfoObject info;
            if (columnNumber <= columnsHashTable.Count)
            {
                Column column = (Column)columnsHashTable[columnNumber];

                if (columnNumber != columnsHashTable.Count)
                {
                    string name = column.getName();
                    info = column.setMAX_TASKS(author, capacity);
                    if (info.getIsSucceeded())
                    {
                        DataLayerColumn.save(name, capacity, author, name);
                        return info;
                    }
                    else
                    {
                        return info;
                    }
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

        public string columnIntToName(int number)
        {
            return ((Column)columnsHashTable[number]).getName();
        }


        //this function moves the colum one index forward
        public InfoObject moveColumn(int columnNumber)
        {
            InfoObject info;
            if (columnNumber == getColumns().Count)
            {
                info = new InfoObject(false, "You can't move the last column");
                Log.Error("You can't move the last column");
            }
            else if (columnNumber>getColumns().Count)
            {
                info = new InfoObject(false, "You selected illegal column");
                Log.Error("You selected illegal column");
            }
            else
            {
                Column Forward = (Column)getColumns()[columnNumber];
                Column Backwards = (Column)getColumns()[columnNumber+1];
                Forward.updateTaskHash(columnNumber + 1);
                Backwards.updateTaskHash(columnNumber);
                getColumns().Remove(columnNumber);
                getColumns().Remove(columnNumber + 1);
                getColumns().Add(columnNumber, Backwards);
                getColumns().Add(columnNumber + 1, Forward);
                info = new InfoObject(true, "the column #"+columnNumber+" moved forward");
                Log.Info("Column #"+columnNumber+" moved forward");
            }
            return info;
        }

        // this function collet all the tasks from data and forward to the specific column they belong to.
        public int open(string author, string existingColumns)
        {
            int numberOfColumns = 1;
            columnsHashTable = new Hashtable();
            this.currColumnCount = 0;
            string columnID;
            string[] splited = existingColumns.Split('+');
            for (int i = 0; i < splited.Length; i = i + 1)
            {
                if (!splited[i].Equals(""))
                {
                    columnID = splited[i];
                    ColumnD columnD = DataLayerColumn.Open(columnID, author);
                    Column column = new Column(columnD.getName());
                    column.setMAX_TASKS(author, columnD.getMaxCapacity());
                    columnsHashTable.Add(numberOfColumns, column);
                    this.currColumnCount = this.currColumnCount + 1;
                    numberOfColumns = numberOfColumns + 1;
                    Log.Info("Column " + column.getName() + " loaded successfully from the database.");
                }
            }
            int numberOfTasksAdded = 0;
            TaskD taskD = DataLayerTask.Open(numberOfTasksAdded, author);
            while (taskD != null)
            {
                Column column = (Column)columnsHashTable[taskD.getStatus()];
                column.addTask(taskD.getTitle(), taskD.getDescription(),taskD.getStatus(), taskD.getDueDate(),taskD.getCreationTime(), taskD.getAuthor(), taskD.getTaskUID());
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