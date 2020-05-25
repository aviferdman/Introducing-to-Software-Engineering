using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanProjectWPF
{
    class BoardD
    {
        private string name;
        private string author;
        private int numOfTasks;

        public BoardD(string name, string author, int numOfTasks)
        {
            this.name = name;
            this.author = author;
            this.numOfTasks = numOfTasks;
        }

        public string getName()
        {
            return name;
        }

        public string getAuthor()
        {
            return author;
        }
        public int getNumberOfTasks()
        {
            return numOfTasks;
        }

        public void setName(string name)
        {
            this.name = name;
        }
        public void setAuthor (string author)
        {
            this.author = author;
        }
        public void setNumOfTasks (int numOfTasks)
        {
            this.numOfTasks = numOfTasks;
        }
    }
}
