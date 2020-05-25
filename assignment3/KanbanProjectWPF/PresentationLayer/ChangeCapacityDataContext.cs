using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanProjectWPF.PresentationLayer
{
    class ChangeCapacityDataContext
    {
        int columnCapacity;
        public int ColumnCapacity
        {
            get
            {
                return columnCapacity;
            }
            set
            {
                columnCapacity = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ColumnCapacity"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
