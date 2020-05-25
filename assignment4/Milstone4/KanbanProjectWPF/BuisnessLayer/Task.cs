using System;
using log4net;
using KanbanProjectWPF;

namespace Milstone2
{
    public class Task
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(Task));

        private static int BackLog = 1;

        private string TITLE;
        private string DESCRIPTION;
        private int STATUS;
        private string AUTHOR;
        private DateTime CREATIONDATE;
        private DateTime DUEDATE;
        private int taskUID;
        private string boardName;

        // defualt constructor
        public Task(string title, string description, string dueDate, string author, int taskCounter,String boardName)
        {
            this.TITLE = title;
            this.DESCRIPTION = description;
            this.STATUS = BackLog;
            this.CREATIONDATE = DateTime.Now.Date;
            this.DUEDATE = DateTime.Parse(dueDate);
            this.AUTHOR = author;
            this.taskUID = taskCounter;
            this.boardName = boardName;
        }

        public Task(string title, string description, string dueDate,string creationDate, string author, int taskCounter, String boardName)
        {
            this.TITLE = title;
            this.DESCRIPTION = description;
            this.STATUS = BackLog;
            this.CREATIONDATE = DateTime.Parse(creationDate);
            this.DUEDATE = DateTime.Parse(dueDate);
            this.AUTHOR = author;
            this.taskUID = taskCounter;
            this.boardName = boardName;
        }

        public String getBoardName()
        {
            return this.boardName;
        }

        // ToString method that return all task details.
        public override string ToString()
        {
            string status1 = "Backlog";
            string status2 = "In Progress";
            string status3 = "Done";
            string status;

            if (this.STATUS == 1)
            {
                status = status1;
            }
            else if (this.STATUS == 2)
            {
                status = status2;
            }
            else
            {
                status = status3;
            }
            String output = "";

            output += "\n" + "Task ID: " + this.taskUID + "\n" +
                       "Task title: " + this.TITLE + "\n" +
                       "Task description: " + this.DESCRIPTION + "\n" +
                       "Author: " + this.AUTHOR + "\n" +
                       "Creation time: " + this.CREATIONDATE + "\n" +
                       "Task Status: " + status + "\n" +
                       "The due time is: " + this.DUEDATE.ToShortDateString() + "\n" + "\n";

            return output;
        }

        // this function return the title of the current tasks.
        public string getTitle()
        {
            return this.TITLE;
        }

        // this function return the description of the current tasks.
        public string getDescription()
        {
            return this.DESCRIPTION;
        }

        // this function return the coulmn where the current task is.
        public int getStatus()
        {
            return this.STATUS;
        }

        // this function return the creation date of the current tasks.
        public String getCreationTime()
        {
            return this.CREATIONDATE.ToString();
        }

        // this function return the due date of the current tasks.
        public String getDueDate()
        {
            return this.DUEDATE.ToShortDateString();
        }

        public String getCreationDate()
        {
            return this.CREATIONDATE.ToShortDateString();
        }

        // this function return the user who created the current tasks.
        public String getAuthor()
        {
            return this.AUTHOR;
        }

        // this function return the task id of the current tasks.
        public int getTaskUID()
        {
            return this.taskUID;
        }

        public void setStatus(int newStatus)
        {
            this.STATUS = newStatus;
        }

        // this function sets the coulmn of the current task to a new one.
        public void statusUpdate()
        {
            this.STATUS = this.STATUS+1;
        }

        // this function set the description of the current user to a new one.
        public void descreptionEdit(string newDescreption)
        {
            this.DESCRIPTION = newDescreption;
            save();
        }

        // this function set the due date of the current user to a new one.
        public void dueDateEdit(string newDueDate)
        {
            DateTime newDue = DateTime.Parse(newDueDate);
            this.DUEDATE = newDue;
            save();
        }

        // this function set the title of the current user to a new one.
        public void titleEdit(string newTitle)
        {
            this.TITLE = newTitle;
            save();
        }

        // this function forwards the task details to data layer to be saved.
        public void save()
        {
            DataLayerTask2.save(this.TITLE, this.DESCRIPTION, this.STATUS, this.DUEDATE.ToShortDateString(),this.CREATIONDATE.ToShortDateString(), this.AUTHOR, this.taskUID, boardName);
            Log.Info("Task #" + taskUID + " saved to the database successfully.");
        }

    }
}