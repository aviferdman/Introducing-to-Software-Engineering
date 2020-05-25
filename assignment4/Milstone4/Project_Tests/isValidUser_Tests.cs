using System;
using NUnit.Framework;
using Milstone2;

namespace Project_Tests
{
    [TestFixture]
    public class isValidUser_Tests
    {
        private string email, password;
        private Boolean output;

        [Test]
        public void Test_ValidInput() 
        {
            email = "Kawhi@gmail.com";
            password = "ImAFunGuy2";
            output = IsValid.IsValidUser(email, password);
            Assert.AreEqual(output, true);
        }

        [Test]
        public void Test_LongPassword()     //Password > 20 characters
        {
            email = "semyon@gmail.com";
            password = "MaKoreGiborBaalabait777";
            output = IsValid.IsValidUser(email, password);
            Assert.AreEqual(output, false);
        }

        [Test]
        public void Test_ShortPassword()   //Password < 4 characters
        {
            email = "netta@walla.co.il";
            password = "Toy";
            output = IsValid.IsValidUser(email, password);
            Assert.AreEqual(output, false);
        }

        [Test]
        public void Test_UserNull()    
        {
            email = null;
            password = "Lol123";
            output = IsValid.IsValidUser(email, password);
            Assert.AreEqual(output, false);
        }

        [Test]
        public void Test_PasswordNull()
        {
            email = "test@gmail.com";
            password = null;
            output = IsValid.IsValidUser(email, password);
            Assert.AreEqual(output, false);
        }

        [Test]
        public void Test_NoDigit()         
        {
            email = "gali@post.bgu.ac.il";
            password = "Shema";
            output = IsValid.IsValidUser(email, password);
            Assert.AreEqual(output, false);
        }

        [Test]
        public void Test_NoCapitalLetter()     
        {
            email = "bibi@hotmail.com";
            password = "milchen10";
            output = IsValid.IsValidUser(email, password);
            Assert.AreEqual(output, false);
        }

        [Test]
        public void Test_NoSmallLetter()
        {
            email = "sarah@hotmail.com";
            password = "BAMA1";
            output = IsValid.IsValidUser(email, password);
            Assert.AreEqual(output, false);
        }

        [Test]
        public void Test_InvalidEmailAdress()
        {
            email = "sarah@gmail";
            password = "Aa1234";
            output = IsValid.IsValidUser(email, password);
            Assert.AreEqual(output, false);
        }

    }
}
