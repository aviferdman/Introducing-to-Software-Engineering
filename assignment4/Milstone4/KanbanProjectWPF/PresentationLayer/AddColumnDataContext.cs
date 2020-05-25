using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanProjectWPF.PresentationLayer
{
    class AddColumnDataContext
    {
        String columnName;
        public String ColumnName
        {
            get
            {
                return columnName;
            }
            set
            {
                columnName = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ColumnName"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
