using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanProjectWPF.PresentationLayer
{
    class FilterDataContext : INotifyPropertyChanged
    {
        String stringToFilter;
        public String StringToFilter
        {
            get
            {
                return stringToFilter;
            }
            set
            {
                stringToFilter = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("StringToFilter"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
