using System;
using System.Data.SQLite;
using Milstone2;
using NUnit.Framework;
using Milstone2.DataLayer;
using KanbanProjectWPF;

namespace Project_Tests
{
    [TestFixture]

    class addTask_Tests
    {
        private string email, password, title, description, dueDate;
        InfoObject output;
        

        [OneTimeSetUp]
        public void SetUp()
        {
            email = "tester@gmail.com";
            password = "Aa123";
            SystemInterface.register(email, password);
            SystemInterface.login(email, password);
        }

        [Test]
        public void Test_AddTask()
        {
            title = "Test";
            description = "This is a test.";
            dueDate = "24/09/2019";
            output = SystemInterface.addTask(email, title, description, dueDate);
            Assert.AreEqual(output.getIsSucceeded(), true);
        }

        [Test]
        public void Test_AddTaskEmptyTitle()
        {
            title = "";
            description = "This is a test.";
            dueDate = "24/09/2019";
            output = SystemInterface.addTask(email, title, description, dueDate);
            Assert.AreEqual(output.getIsSucceeded(), false);
        }

        [Test]
        public void Test_AddTaskNullTitle()
        {
            title = null;
            description = "This is a test.";
            dueDate = "24/09/2019";
            output = SystemInterface.addTask(email, title, description, dueDate);
            Assert.AreEqual(output.getIsSucceeded(), false);
        }

        [Test]
        public void Test_AddTaskLongTitle()
        {
            title = "This title is over 50 charcaters and you can't do that here";
            description = "This is a test.";
            dueDate = "24/09/2019";
            output = SystemInterface.addTask(email, title, description, dueDate);
            Assert.AreEqual(output.getIsSucceeded(), false);
        }

        [Test]
        public void Test_AddTaskNullDescription()
        {
            title = "Test";
            description = null;
            dueDate = "24/09/2019";
            output = SystemInterface.addTask(email, title, description, dueDate);
            Assert.AreEqual(output.getIsSucceeded(), false);
        }

        [Test]
        public void Test_AddTaskLongDescription()
        {
            title = "";
            description = "This is a very very very very very very very very very very very very very very very very very very very" +
                " very very very very very very very very very very very very very very very very very very very very very very very very " +
                "very very very very very very very very very very very very very very very very very very very very very very very very " +
                "very very very very very very very very very very very very very very very very very very very very very very very very " +
                "very very very very very very very very very very very very very very very very very very very very very very very very " +
                "very very very very very very very very very very very very very very very very very very very very very very very very " +
                "very very very very very very very very very very very very very very very very very very very very very very very very " +
                "very very very very very very very very very very very very very very very very very very very very very very very very " +
                "very very very very very very very very very very very very very very very very very very very very very very very very " +
                "very very very very very very very very very very very very very very very very very very very very very very very very " +
                "very very very very very very very very very very very very very very very very very very very very very very very very " +
                "very very very very very very very very very very very very very very very very very very very very very very very very " +
                "very very very very very very very very very very very very very very very very very long description.";
            dueDate = "24/09/2019";
            output = SystemInterface.addTask(email, title, description, dueDate);
            Assert.AreEqual(output.getIsSucceeded(), false);
        }

        [Test]
        public void Test_AddTaskNullDueDate()
        {
            title = "Test";
            description = "This is a test.";
            dueDate = null;
            output = SystemInterface.addTask(email, title, description, dueDate);
            Assert.AreEqual(output.getIsSucceeded(), false);
        }

        [Test]
        public void Test_AddTaskPastDueDate()
        {
            title = "Test";
            description = "This is a test.";
            dueDate = "14/05/1948";
            output = SystemInterface.addTask(email, title, description, dueDate);
            Assert.AreEqual(output.getIsSucceeded(), false);
        }

        [Test]
        public void Test_AddTaskWrongDueDateFormat()
        {
            title = "Test";
            description = "This is a test.";
            dueDate = "2020";
            output = SystemInterface.addTask(email, title, description, dueDate);
            Assert.AreEqual(output.getIsSucceeded(), false);
        }

        [OneTimeTearDown]
        public void TearDown()                      //Delete test user from database
        {
            DataLayerUser2.removeUser(email);
        }   
    }
}
