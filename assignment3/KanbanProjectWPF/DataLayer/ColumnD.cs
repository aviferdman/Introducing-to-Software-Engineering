using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milstone2
{
    [Serializable]
    class ColumnD
    {
        string name;
        int maxCapacity;

        public ColumnD(string name, int maxCapacity)
        {
            this.name = name;
            this.maxCapacity = maxCapacity;
        }

        public string getName()
        {
            return name;
        }

        public int getMaxCapacity()
        {
            return maxCapacity;
        }
    }
}
