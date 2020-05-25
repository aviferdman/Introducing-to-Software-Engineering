using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using log4net;

namespace Milstone2
{
    [Serializable]
    class DataLayerUser
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        // this functins serialize and saves the user.
        public static Boolean save(string email, string password, Boolean logged_in, int numOfTasksAdded)
        {
            UserD newUser = new UserD(email, password, logged_in, numOfTasksAdded);
            Stream myFileStream = File.Create("data." + email);
            BinaryFormatter serializes = new BinaryFormatter();
            serializes.Serialize(myFileStream, newUser);
            myFileStream.Close();
            return true;
        }

        // this functins deserialize and return the user.
        public static UserD open(string email)
        {
            if (File.Exists("data." + email))
            {
                Stream myOtherFileStream = File.OpenRead("data." + email);
                BinaryFormatter deserializer = new BinaryFormatter();
                UserD user = (UserD)deserializer.Deserialize(myOtherFileStream);
                myOtherFileStream.Close();
                return user;
            }
            Log.Warn("Failed to find user " + email + " in the database.");
            return null;
        }
    }
}

