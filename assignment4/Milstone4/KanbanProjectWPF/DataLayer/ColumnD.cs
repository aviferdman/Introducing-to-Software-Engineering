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
        string boardName;
        int location;

        public ColumnD(string name, int maxCapacity,int location, string boardName)
        {
            this.name = name;
            this.maxCapacity = maxCapacity;
            this.boardName = boardName;
            this.location = location;
        }

        public string getName()
        {
            return name;
        }

        public int getMaxCapacity()
        {
            return maxCapacity;
        }
        public int getLocation()
        {
            return location;
        }
    }
}
