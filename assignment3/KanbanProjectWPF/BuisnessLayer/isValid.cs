using System;
using log4net;


namespace Milstone2
{
    class IsValid
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(IsValid));

        public IsValid() { }            //Empty constructor

        public static Boolean IsValidUser(String email, String password)
        {
            if ((email==null|password==null)||(password.Length < 4 | password.Length > 20))
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
            if (title==null)
            {
                Log.Error("the title is null");
                return false;
            }
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
            if (descreption == null)
            {
                Log.Error("the descreption is null");
                return false;
            }
            if (descreption.Length > 300)
            {
                Log.Warn("new descreption input : " + descreption + " is invalid input");
                return false;
            }
            return true;
        }

        public static Boolean IsValidTaskDueDate(String dueDate)
        {
            if (dueDate == null)
            {
                Log.Error("the due date is null");
                return false;
            }
            DateTime output;
            if (DateTime.TryParse(dueDate, out output))
            {
                if (DateTime.Compare(DateTime.Now.Date, output) > 0)
                {
                    Log.Warn("New due date input: " + output + " has been passed.");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                Log.Warn("New due date input: " + output + " is invalid.");
                return false;
            }
        }
    }
}
