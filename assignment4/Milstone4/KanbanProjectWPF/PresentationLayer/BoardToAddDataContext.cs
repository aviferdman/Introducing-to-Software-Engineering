using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanProjectWPF.PresentationLayer
{
    class BoardToAddDataContext
    {
        String boardName;
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
