using System;
using System.Collections;
using log4net;
using System.Collections.Generic;
using KanbanProjectWPF;

namespace Milstone2
{
    public class Column
    {


        private int MAX_TASKS;
        private string COLUMN_NAME;
        private int numOfTaskInCoulmn;
        private string boardName;
        private int location;

        static readonly ILog Log = LogManager.GetLogger(typeof(Column));
        private Hashtable taskHash;

        // defualt contractor
        public Column(string columnName,string boardName)
        {
            this.MAX_TASKS = int.MaxValue;
            this.COLUMN_NAME = columnName;
            taskHash = new Hashtable();
            this.numOfTaskInCoulmn = 0;
            this.boardName = boardName;
        }

        //this function returns the tasks hash table
        public Hashtable getTasksHash()
        {
            return taskHash;
        }
        public String getName()
        {
            return COLUMN_NAME;
        }

        public String getBoardName()
        {
            return this.boardName;
        }

        public int getLocation()
        {
            return this.location;
        }

        public void setLocation(int newLocation)
        {
            this.location = newLocation;
        }

        public String getColumnBoard()
        {
            return this.boardName;
        }

        // this function adds new task to the backlog column.
        public InfoObject addTask(string title, string description, string dueDate, string author, int taskCounter)
        {
            InfoObject info;
            if (this.numOfTaskInCoulmn == MAX_TASKS)
            {
                Log.Error("Column " + getName() + " reached its maximum capacity.");
                info = new InfoObject(false, "Column " + getName() + " reached its maximum capacity.");
            }
            else
            {
                Task newTask = new Task(title, description, dueDate, author, taskCounter,this.boardName);
                newTask.save();
                taskHash.Add(taskCounter, newTask);
                numOfTaskInCoulmn++;
                info = new InfoObject(true, "");
            }
            return info;
        }

        // this function adds existing task to the his column.
        public InfoObject addTask(string title, string description,int clumnStatus, string dueDate, string CreationDate, string author, int taskCounter)
        {
            InfoObject info;
            if (this.numOfTaskInCoulmn == MAX_TASKS)
            {
                Log.Error("Column " + getName() + " reached its maximum capacity.");
                info = new InfoObject(false, "Column " + getName() + " reached its maximum capacity.");
            }
            else
            {
                Task newTask = new Task(title, description, dueDate,CreationDate, author, taskCounter,this.boardName);
                newTask.setStatus(clumnStatus);
                newTask.save();
                taskHash.Add(taskCounter, newTask);
                numOfTaskInCoulmn++;
                info = new InfoObject(true, "");
            }
            return info;
        }

        public InfoObject moveTask(string title, string description, int clumnStatus, string dueDate, string author, int taskCounter)
        {
            InfoObject info;
            if (this.numOfTaskInCoulmn == MAX_TASKS)
            {
                Log.Error("Column " + getName() + " reached its maximum capacity.");
                info = new InfoObject(false, "Column " + getName() + " reached its maximum capacity.");
            }
            else
            {
                Task newTask = new Task(title, description, dueDate, author, taskCounter,this.boardName);
                newTask.setStatus(clumnStatus + 1);
                newTask.save();
                taskHash.Add(taskCounter, newTask);
                numOfTaskInCoulmn++;
                info = new InfoObject(true, "");
            }
            return info;
        }

        // this function return true if the current key is in the hash table that contains the current coulmn tasks.
        public Boolean isContainsKey(int key)
        {
            if (this.taskHash.ContainsKey(key))
            {
                return true;
            }
            return false;
        }

        // this function removes task from the current column and return the Task that had been removed.
        public Task removeTask(int taskId)
        {
            Task output = (Task)taskHash[taskId];
            taskHash.Remove(taskId);
            numOfTaskInCoulmn--;
            return output;
        }

        // this function forwards the inputs to Task class to edit the title
        public InfoObject editTaskTitle(int taskId, string newTitle)
        {
            InfoObject info;
            Task t = (Task)taskHash[taskId];
            if (t != null)
            {
                t.titleEdit(newTitle);
                t.save();
                Log.Info("The title of task #" + taskId + " has been updated.");
                info = new InfoObject(true, "The title of task #" + taskId + " has been updated.");
                return info;
            }
            else
            {
                Log.Warn("The title of task #" + taskId + " has not been updated beacuse there is not task like this");
                info = new InfoObject(false, "Can't find task #" + taskId);
                return info;
            }
        }

        // this function forwards the inputs to Task class to edit the descreption
        public InfoObject editDescreption(int taskId, string newDescreption)
        {
            InfoObject info;
            Task t = (Task)taskHash[taskId];
            if (t != null)
            {
                t.descreptionEdit(newDescreption);
                t.save();
                Log.Info("The description of task #" + taskId + " has been updated.");
                info = new InfoObject(true, "The description of task #" + taskId + " has been updated.");
                return info;
            }
            else
            {
                Log.Warn("The description of task #" + taskId + " has not been updated beacuse there is not task like this");
                info = new InfoObject(false, "Can't find task#" + taskId);
                return info;
            }
        }

        // this function forwards the inputs to Task class to edit the due date
        public InfoObject editDueDate(int taskId, string newDueDate)
        {
            InfoObject info;
            Task t = (Task)taskHash[taskId];
            if (t != null)
            {
                t.dueDateEdit(newDueDate);
                t.save();
                Log.Info("The due date of task #" + taskId + " has been updated.");
                info = new InfoObject(true, "The due date of task #" + taskId + " has been updated.");
                return info;
            }
            else
            {
                Log.Warn("The due date of task #" + taskId + " has not been updated beacuse there is not task like this");
                info = new InfoObject(false, "Can't find task#" + taskId);
                return info;
            }
        }

        // this function sets the MAX_TASKS field 
        public InfoObject setMAX_TASKS(string author, int newMAX_TASKS)
        {
            InfoObject info;
            if (getCurrentTaskCount() > newMAX_TASKS)
            {
                Log.Warn("The capacity of column " + getName() + " cant be changed to: " + newMAX_TASKS + ", beacuse there are more tasks than the new capacity number");
                info = new InfoObject(false, "The capacity of column " + getName() + " cant be changed to: " + newMAX_TASKS + ", beacuse there are more tasks than the new capacity number");
                return info;
            }
            else
            {
                this.MAX_TASKS = newMAX_TASKS;
                DataLayerColumn2.save(getName(), newMAX_TASKS, author, getLocation(), boardName);
                Log.Info("The capacity of column " + COLUMN_NAME + " changed to: " + newMAX_TASKS);
                info = new InfoObject(true, "The capacity of column " + COLUMN_NAME + " changed to: " + newMAX_TASKS);
                return info;
            }
        }

        //this function checks if this column can be removed
        public InfoObject isPossibleToRemove()
        {
            InfoObject info;
            if (getCurrentTaskCount() > 0)
            {
                Log.Warn("The capacity of column " + COLUMN_NAME + " is  " + getCurrentTaskCount() + "and therefor it can't be removed");
                info = new InfoObject(false, "The capacity of column " + COLUMN_NAME + " is  " + getCurrentTaskCount() + "and therefor it can't be removed");
                return info;
            }
            else
            {
                Log.Info("column " + COLUMN_NAME + "can be removed");
                info = new InfoObject(true, "column " + COLUMN_NAME + "can be removed");
                return info;
            }
        }

        // this function update all the tasks status to be their status minus 1
        public void updateTaskHash()
        {
            var hash = taskHash;
            foreach (Task t in hash.Values)
            {
                t.setStatus(t.getStatus() - 1);
                t.save();
            }
        }

        public void updateTaskHash(int newStatus)
        {
            var hash = taskHash;
            foreach (Task t in hash.Values)
            {
                t.setStatus(newStatus);
                t.save();
            }
        }


        public Boolean canAddTask()
        {
            return (taskHash.Count < getColumnMaxTasks());
        }


        // this function saves all the tasks assigned to the current column.
        public void save()
        {
            for (int i = 0; i < taskHash.Count; i = i + 1)
            {
                Task t = (Task)taskHash[i];
                t.save();
            }
        }

        // this function return the number of tasks can be added to the current coulmn.
        public int getColumnMaxTasks()
        {
            return this.MAX_TASKS;
        }

        // this function return the number of tasks added to the current coulmn.
        public int getCurrentTaskCount()
        {
            return this.numOfTaskInCoulmn;
        }

    }
}
