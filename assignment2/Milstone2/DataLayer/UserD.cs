﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milstone2
{
    [Serializable]
    class UserD
    {
        private String password;
        private String email;
        private Boolean logged_in;
        private int numOfTasksAdded;

        // defualt constructor
        public UserD(String email, String password, Boolean logged_in, int numOfTasksAdded)
        {
            this.email = email;
            this.password = password;
            this.logged_in = logged_in;
            this.numOfTasksAdded = numOfTasksAdded;
        }

        // this function returns the email of the current user
        public String getEmail()
        {
            return email;
        }

        // this function returns the password of the current user
        public String getPassword()
        {
            return password;
        }

        // this function returns if the current user is logged in
        public Boolean isLoggedIn()
        {
            return logged_in;
        }

        // this function returns the amount of tasks added by the current user.
        public int getNumOfTasksAdded()
        {
            return numOfTasksAdded;
        }
    }
}
