using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using log4net;

namespace Milstone2
{
    [Serializable]
    class DataLayerTask
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        // this functins serialize and saves the task.
        public static void save(string title, string description, int status, string dueDate, string author, int taskID)
        {
            Log.Info("Saving task #" + taskID + ". Title: " +title);
            TaskD taskToSave = new TaskD(title, description, status, dueDate, author, taskID);
            Stream myFileStream = File.Create("Task" + author + taskID);
            BinaryFormatter serializes = new BinaryFormatter();
            serializes.Serialize(myFileStream, taskToSave);
            myFileStream.Close();
        }
        

        // this function deserialize and return task
        public static TaskD Open(int taskID,string author)
        {
            if (File.Exists("Task" +author+taskID))
            {
                Stream myOtherFileStream = File.OpenRead("Task" + author + taskID);
                BinaryFormatter deserializer = new BinaryFormatter();
                TaskD taskToOpen = (TaskD)deserializer.Deserialize(myOtherFileStream);
                myOtherFileStream.Close();
                return taskToOpen;
            }
            Log.Warn("Failed to find task #" + taskID + " in the database.");
            return null;
        }

        

    }
}
