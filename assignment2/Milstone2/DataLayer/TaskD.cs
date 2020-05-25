using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milstone2
{
    [Serializable]
    class TaskD
    {

        private string TITLE;
        private string DESCRIPTION;
        private int STATUS;
        private string AUTHOR;
        private DateTime CREATIONDATE;
        private DateTime DUEDATE;
        private int taskUID;

        // defualt constructor
        public TaskD(string title, string description,int status, string dueDate, string author, int taskCounter)
        {
            this.TITLE = title;
            this.DESCRIPTION = description;
            this.STATUS = status;
            this.CREATIONDATE = DateTime.Now.Date;
            this.DUEDATE = DateTime.Parse(dueDate);
            this.AUTHOR = author;
            this.taskUID = taskCounter;
        }
       
        // this function returns the title of the current task.
        public string getTitle()
        {
            return this.TITLE;
        }
        // this function returns the description of the current task.
        public string getDescription()
        {
            return this.DESCRIPTION;
        }

        // this function returns the column of the current task.
        public int getStatus()
        {
            return this.STATUS;
        }

        // this function returns the creation time of the current task.
        public String getCreationTime()
        {
            return this.CREATIONDATE.ToString();
        }

        // this function returns the due date of the current task.
        public string getDueDate()
        {
            return this.DUEDATE.ToShortDateString();
        }

        // this function returns the user who added the current task.
        public String getAuthor()
        {
            return this.AUTHOR;
        }

        // this function returns the ID of the current task.
        public int getTaskUID()
        {
            return this.taskUID;
        }

    }
}
