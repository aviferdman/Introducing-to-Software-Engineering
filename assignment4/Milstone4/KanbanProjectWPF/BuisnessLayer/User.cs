using System;
using System.Collections;
using log4net;
using KanbanProjectWPF;
using System.Collections.Generic;

namespace Milstone2
{
    public class User
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(User));

        private string email;
        private string password;
        private Boolean logged_in;

        private int numOfTaskAdded;

        private static Hashtable myBoards;
        private Board lastBoardClosed;
        private String boardsCollection;


        // User constructor - email and password
        public User(string email, string password)
        {
            this.email = email;
            this.password = password;
            logged_in = false;
            numOfTaskAdded = 0;
            myBoards = new Hashtable();

        }

        // this function return the email of the current user
        public string getEmail()
        {
            return this.email;
        }

        public void addBoard(String boardName)
        {
            Board boardToAdd = new Board(email,boardName);
            boardToAdd.addColumn(getEmail(), "Backlog");
            boardToAdd.addColumn(getEmail(), "In progress");
            boardToAdd.addColumn(getEmail(), "Done");
            myBoards.Add(boardName, boardToAdd);
            this.boardsCollection += "+" + boardName;
            boardToAdd.save();
            save();
        }

        public string getBoardsNameCollection()
        {
            return this.boardsCollection;
        }

        public void setCurrBoard(String boardName)
        {
            this.lastBoardClosed = (Board)myBoards[boardName];
            save();
        }

        //this functiom return the board of the current user
        public Board getBoard()
        {
            return this.lastBoardClosed;
        }

        public Hashtable getBoards()
        {
            return myBoards;
        }

        // this function return the password of the current user
        public string getPassword()
        {
            return this.password;
        }

        // this function return the number of tasks added by the current user
        public int getNumOfTasksAdded()
        {
            return numOfTaskAdded;
        }

        // this function sets the number of tasks added by the current user
        public void setNumberOfTasksAdded(int newNumber)
        {
            this.numOfTaskAdded = newNumber;
        }

        // this function sets the logged_In field to true when logged in and false when logged out
        public void setIsLoggedIn(Boolean isLoggedIn)
        {
            this.logged_in = isLoggedIn;
        }

        private void setBoardsCollection (string boardsCollection)
        {
            this.boardsCollection = boardsCollection;
        }

        // this functin returns true if the current user is logged_in field is true and false if not
        public Boolean isLoggedIn()
        {
            return logged_in;
        }

        // this funtion adds 1 to the number of tasks added field.
        public void taskCounterUpdate()
        {
            this.numOfTaskAdded++;
        }

        // this function return User object from the data layer.
        public static User getByEmail(string email)
        {
            UserD openedUser = DataLayerUser2.open(email);
            User returnedUser = null;
            if (openedUser != null)
            {
                returnedUser = new User(openedUser.getEmail(), openedUser.getPassword());
                returnedUser.setIsLoggedIn(openedUser.isLoggedIn());
                returnedUser.setNumberOfTasksAdded(openedUser.getNumOfTasksAdded());
                //returnedUser.setBoardsCollection(openedUser.getExistingBoards());

                LinkedList<BoardD> ListBoardD = DataLayerBoard2.Open(email);
                String boardsList="";
                foreach (BoardD boardD in ListBoardD)
                {
                    Board b = new Board(email, boardD.getName());
                    b.setNumOfTasks(boardD.getNumberOfTasks());
                    b.open(email);
                    myBoards.Add(b.getBoardName(), b);
                    boardsList += "+" + boardD.getName();
                }
                returnedUser.setBoardsCollection(boardsList);
                returnedUser.setCurrBoard(openedUser.getLastBoard());
            }
            return returnedUser;
        }

        // this function update the logged_in field to true and forward to Board class to open all the tasks.
        public InfoObject login()
        {
            InfoObject info;
            logged_in = true;
            save();
            Log.Info("The user " + getEmail() + " logged in. ");
            info = new InfoObject(true, "The user " + getEmail() + " logged in. ");
            return info;
        }

        // this function update the logged_in field to false 
        public InfoObject logout()
        {
            InfoObject info;
            logged_in = false;
            save();
            Log.Info("The user " + getEmail() + " logged out.");
            info = new InfoObject(true, "The user " + getEmail() + " logged out.");
            return info;
        }

        // this function checks if the current user is already registerd if not save the user to the data.
        public InfoObject register()
        {
            InfoObject info;
            if (DataLayerUser2.open(getEmail()) == null)
            {
                this.lastBoardClosed = new Board(email, "Main");
                myBoards.Add("Main", this.lastBoardClosed);
                this.boardsCollection = lastBoardClosed.getBoardName();
                save();
                addColumn(getEmail(), "Backlog");
                addColumn(getEmail(), "In progress");
                addColumn(getEmail(), "Done");
                lastBoardClosed.save();
                Log.Info("The user " + getEmail() + " registered.");
                info = new InfoObject(true, "The user " + getEmail() + " registered.");
                return info;
            }
            else
            {
                Log.Info("The user " + getEmail() + " can't register because this email is already in use.");
                info = new InfoObject(false, "The user " + getEmail() + " can't register because this email is already in use.");
                return info;
            }
        }

        // this function checks if the current user is logged in and if he is then forward it to Board class to add the task to the current column.
        public InfoObject addTask(string title, string description, string dueDate)
        {
            if (isLoggedIn())
            {
                InfoObject info = this.lastBoardClosed.addTask(title, description, this.email, dueDate);
                if (info.getIsSucceeded())
                {
                    info.setMessage("Task #" + numOfTaskAdded + " added to the board by the user: " + getEmail());
                    save();
                    return info;
                }
                else
                {
                    return info;
                }
            }
            else
            {
                Log.Error("You are not allowed to add a task, please log in first.");
                InfoObject info2 = new InfoObject(false, "You are not allowed to add a task, please log in first.");
                return info2;
            }
        }

        // this function checks if the current user is logged in and if he is forwards it to Board class to move the task to next coulmn.
        public InfoObject moveTask(int taskId, int status)
        {
            if (isLoggedIn())
            {
                return (this.lastBoardClosed.moveTask(taskId, status));
            }
            else
            {
                Log.Error("You are not allowed to move a task, please log in first.");
                InfoObject info = new InfoObject(false, "You are not allowed to move a task, please log in first.");
                return info;
            }

        }

        // this function checks if the current user is logged in and if he is forwards to Board class.
        public InfoObject editTaskTitle(int taskId, int status, string newTitle)
        {
            if (isLoggedIn())
            {
                return this.lastBoardClosed.editTaskTitle(taskId, status, newTitle);
            }
            else
            {
                Log.Error("You are not allowed to edit task title, please log in first.");
                InfoObject info = new InfoObject(false, "You are not allowed to edit task title, please log in first.");
                return info;
            }
        }

        // this function checks if the current user is logged in and if he is forwards to Board class.
        public InfoObject editTaskDescreption(int taskId, int status, string newDescreption)
        {
            if (isLoggedIn())
            {
                return this.lastBoardClosed.editTaskDescreption(taskId, status, newDescreption);
            }
            else
            {
                Log.Error("You are not allowed to edit task description, please log in first.");
                InfoObject info = new InfoObject(false, "You are not allowed to edit task title, please log in first.");
                return info;
            }
        }

        // this function checks if the current user is logged in and if he is forwards to Board class.
        public InfoObject editTaskDueDate(int taskId, int status, string newDueDate)
        {
            if (isLoggedIn())
            {
                return this.lastBoardClosed.editTaskDueDate(taskId, status, newDueDate);
            }
            else
            {
                Log.Error("You are not allowed to edit task due date, please log in first.");
                InfoObject info = new InfoObject(false, "You are not allowed to edit task title, please log in first.");
                return info;
            }
        }

        // this function forwards the inputs to Board class to update the capacity of a column
        public InfoObject changeColumnCapacity(string author, int columnNumber, int capacity)
        {
            InfoObject info = this.lastBoardClosed.changeColumnCapacity(author, columnNumber, capacity);
            return info;
        }

        // this function forward the input to Board class to add new coulmn.
        public InfoObject addColumn(string author, string columnName)
        {
            InfoObject info = this.lastBoardClosed.addColumn(author, columnName);
            if (info.getIsSucceeded())
            {
                save();
            }
            return info;
        }

        // this function forward the input to Board class to remove coulmn.
        public InfoObject removeColumn(string email,string columnName)
        {
            InfoObject info = this.lastBoardClosed.removeColumn(email,columnName);
            if (info.getIsSucceeded())
            {
                save();
            }
            return info;
        }

        public InfoObject moveColumn (int columnNumber)
        {
            InfoObject info = this.lastBoardClosed.moveColumn(columnNumber,getEmail());
            if (info.getIsSucceeded())
            {
                save();
            }
            return info;
        }

        public InfoObject moveColumnBack (int columnNumber)
        {
            if (columnNumber == 1)
            {
                return new InfoObject(false, "Impossible to move the first column backwards");
            }
            else
            {
                return moveColumn(columnNumber - 1);
            }
        }

        // this function forward the current user to be saved in the data layer.
        public void save()
        {
            DataLayerUser2.save(getEmail(), getPassword(), isLoggedIn(), getNumOfTasksAdded(), lastBoardClosed.getBoardName());
            Log.Info("the user " + getEmail() + " saved to the database.");
        }

        //this function remove board
        public InfoObject removeBoard (String boardName)
        {
            InfoObject info = null;
            if (boardName.Equals(lastBoardClosed.getBoardName())){
                info = new InfoObject(false, "Can't remove the board you are working on currently");
                Log.Error("Board " + boardName + " hasn't been removed beacuse the user currently work on");
            }
            else
            {
                if (getBoards()[boardName] != null)
                {
                    getBoards().Remove(boardName);
                    DataLayerBoard2.deleteBoard(boardName, getEmail(), 0);
                    Log.Info("Board " + boardName + " has been removed");
                    info = new InfoObject(true, "Board "+ boardName+" has been removed");
                }
                else
                {
                    info = new InfoObject(false, "Couldn't find the board to remove");
                    Log.Error("Board " + boardName + " hasn't been removed beacuse it's cant be found");
                }
            }
            return info;
        }
    }
}
