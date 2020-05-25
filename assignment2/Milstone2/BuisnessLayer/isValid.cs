using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;


namespace Milstone2
{
    class IsValid
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        public IsValid() { }            //Empty constructor

        public static Boolean IsValidUser(String email, String password)
        {
            if ((password.Length < 4 | password.Length > 20))
                return false;
            Boolean capitalCharacter = false;
            Boolean smallCharacter = false;
            Boolean number = false;
            for (int i = 0; i < password.Length; i = i + 1)
            {
                char c = password[i];
                if (c >= 'a' & c <= 'z')
                    smallCharacter = true;
                if (c >= 'A' & c <= 'Z')
                    capitalCharacter = true;
                if (c >= '0' & c <= '9')
                    number = true;
            }
            if (!capitalCharacter | !smallCharacter | !number)
            {
                Log.Warn(email + " entered invalid password : " + password);
                return false;
            }
            if(!(email.Contains("@") && (email.Contains(".com") | (email.Contains(".co.il")))))
            {
                Log.Warn(email + " entered invalid password : " + password);
                return false;
            }
            return true;
        }

        public static Boolean IsValidTaskTitle(String title)
        {
            if (title.Length == 0)
            {
                Log.Warn("new title input : " + title + " is invalid input");
                return false;
            }
            if (title.Length > 50)
            {
                Log.Warn("new title input : " + title + " is invalid input");
                return false;
            }
            return true;
        }

        public static Boolean IsValidTaskDescreption(String descreption)
        {
            if (descreption.Length > 300)
            {
                Log.Warn("new descreption input : " + descreption + " is invalid input");
                return false;
            }
            return true;
        }

        public static Boolean IsValidTaskDueDate(String dueDate)
        {

            DateTime output;
            if (DateTime.TryParse(dueDate, out output))
            {
                if (DateTime.Compare(DateTime.Now.Date, output) > 0)
                {
                    Log.Warn("New due date input: " + output + " has been passed.");
                    return false;
                } else
                {
                    return true;
                }
            } else {
                Log.Warn("New due date input: " + output + " is invalid.");
                return false;
            }
        }
    }
}
