using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milstone2
{
    public class InfoObject
    {
        private Boolean success;
        private string message;

        public InfoObject(Boolean success, string message)
        {
            this.success = success;
            this.message = message;
        }

        public Boolean getIsSucceeded()
        {
            return success;
        }

        public string getMessage()
        {
            return message;
        }

        public void setIsSucceeded(Boolean success)
        {
            this.success = success;
        }

        public void setMessage(string message)
        {
            this.message = message;
        }


        public void ToString()
        {
            if (success)
            {
                if (getMessage() == "")
                {
                    setMessage("Successfully completed.");
                }
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine(message);
            }
        }
    }
    
}
