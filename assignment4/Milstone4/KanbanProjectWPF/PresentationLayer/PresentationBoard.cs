using Milstone2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanProjectWPF.PresentationLayer
{
    public class PresentationBoard : INotifyPropertyChanged
    {
        string columnName;
        int columnIntName;
        string title;
        string descreption;
        DateTime dueDate;
        string creationDate;
        string taskUID;
        string task;

        public PresentationBoard(String columnN, int columnIntName, String titleN, String descreptionN, DateTime dueDateN, String creationDate, String taskUIDN)
        {
            this.columnName = columnN;
            this.columnIntName = columnIntName;
            this.title = titleN;
            this.descreption = descreptionN;
            this.dueDate = dueDateN;
            this.creationDate = creationDate;
            this.taskUID = taskUIDN;
        }

        public int getTaskUID()
        {
            return Int32.Parse(this.taskUID);
        }

        public int getTasColumn()
        {
            return this.columnIntName;

        }


        public String Creation
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CreationDate"));
            }
        }

        public String Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Title"));
            }
        }


        public String Description
        {
            get
            {
                return descreption;
            }
            set
            {
                descreption = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Description"));
            }
        }

        public DateTime DueDate
        {
            get
            {
                return dueDate;
            }
            set
            {
                dueDate = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DueDate"));
            }
        }


        public string Task
        {
            get
            {
                return "Title : " + this.title + "\n" + "Descreption : " + this.descreption + "\n" + "Creation Date : " + this.creationDate + "\n" + "Duedate : " + this.dueDate.ToShortDateString();
            }
            set
            {
                creationDate = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Task"));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
    }
}
