using System;
using System.Collections;
using log4net;

namespace Milstone2
{
    public class Column
    {


        private int MAX_TASKS;
        private string COLUMN_NAME;
        private int numOfTaskInCoulmn;

        static readonly ILog Log = LogManager.GetLogger(typeof(Program));
        private Hashtable taskHash;

        // defualt contractor
        public Column(string columnName)
        {
            this.MAX_TASKS = int.MaxValue;
            this.COLUMN_NAME = columnName;
            taskHash = new Hashtable();
            this.numOfTaskInCoulmn = 0;
        }

        // this function adds task to the current column.
        public InfoObject addTask(string title, string description, string dueDate,string author, int taskCounter)
        {
            InfoObject info;
            if (this.numOfTaskInCoulmn == MAX_TASKS)
            {
                Log.Error("Column #" + MAX_TASKS + " reached its maximum capacity.");
                info = new InfoObject(false, "Column #" + MAX_TASKS + " reached its maximum capacity.");
            }
            else
            {
                Task newTask = new Task(title, description, dueDate, author, taskCounter);
                newTask.save();
                taskHash.Add(taskCounter, newTask);
                numOfTaskInCoulmn++;
                info = new InfoObject(true,"");
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
            if(t != null)
            {
                t.titleEdit(newTitle);
                t.save();
                Log.Info("The title of task #" + taskId + " has been updated.");
                info = new InfoObject(true, "The title of task #" + taskId + " has been updated.");
                return info;
            }
            else {
                Log.Warn("The title of task #" + taskId + " has not been updated beacuse there is not task like this");
                info = new InfoObject(false, "Can't find task#" + taskId);
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
                info = new InfoObject(true , "The description of task #" + taskId + " has been updated.");
                return info;
            }
            else
            {
                Log.Warn("The description of task #" + taskId + " has not been updated beacuse there is not task like this");
                info = new InfoObject(false , "Can't find task#" + taskId);
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
        public InfoObject setMAX_TASKS(int newMAX_TASKS)
        {
            InfoObject info;
            if (getCurrentTaskCount() > newMAX_TASKS)
            {
                Log.Warn("The capacity of column #" + newMAX_TASKS + " cant be changed to: " + newMAX_TASKS + ", beacuse there are more tasks than the new capacity number");
                info = new InfoObject(false , "The capacity of column #" + newMAX_TASKS + " cant be changed to: " + newMAX_TASKS + ", beacuse there are more tasks than the new capacity number");
                return info;
            }
            else
            {
                this.MAX_TASKS = newMAX_TASKS;
                Log.Info("The capacity of column " + COLUMN_NAME + " changed to: " + newMAX_TASKS);
                info = new InfoObject(true, "The capacity of column " + COLUMN_NAME + " changed to: " + newMAX_TASKS);
                return info;
            }
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
