using System;
using System.Collections.Generic;
using System.Linq;
using Milstone2;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;

namespace KanbanProjectWPF.PresentationLayer
{
    class BoardsNamePresention : INotifyPropertyChanged
    {

        string boardName;
        public BoardsNamePresention(String boardName)
        {
            this.boardName = boardName;
        }

        public String BoardName
        {
            get
            {
                return boardName;
            }
            set
            {
                boardName = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("BoardName"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
