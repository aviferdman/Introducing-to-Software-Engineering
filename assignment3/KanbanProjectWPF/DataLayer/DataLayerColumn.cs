using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Milstone2
{
    [Serializable]
    class DataLayerColumn
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(DataLayerColumn));

        // this functins serialize and saves the task.
        public static void save(string name, int maxCapacity, string author, string columnID)
        {
            Log.Info("Saving Column " + name);
            ColumnD columnToSave = new ColumnD(name, maxCapacity);
            Stream myFileStream = File.Create("Column" + author + columnID);
            BinaryFormatter serializes = new BinaryFormatter();
            serializes.Serialize(myFileStream, columnToSave);
            myFileStream.Close();
        }


        // this function deserialize and return task
        public static ColumnD Open(string columnID, string author)
        {
            if (File.Exists("Column" + author + columnID))
            {
                Stream myOtherFileStream = File.OpenRead("Column" + author + columnID);
                BinaryFormatter deserializer = new BinaryFormatter();
                ColumnD columnToOpen = (ColumnD)deserializer.Deserialize(myOtherFileStream);
                myOtherFileStream.Close();
                return columnToOpen;
            }
            Log.Warn("Failed to find column #" + columnID + " in the database.");
            return null;
        }
    }
}
