using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanProjectWPF.PresentationLayer
{
    class SortDataContext
    {
        String toSortBy;
        public String ToSortBy
        {
            get
            {
                return toSortBy;
            }
            set
            {
                toSortBy = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ToSortBy"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
