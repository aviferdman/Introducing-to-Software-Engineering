using BespokeFusion;
using KanbanProjectWPF.PresentationLayer;
using Milstone2;
using System;
using System.Windows;
using System.Windows.Input;

namespace KanbanProjectWPF
{
    /// <summary>
    /// Interaction logic for AddTask.xaml
    /// </summary>
    public partial class AddTask : Window
    {
        AddTaskDataContext ATDC;
        string userName;
        MainWindow main;
        public AddTask(string userName, MainWindow main)
        {
            InitializeComponent();
            ATDC = new AddTaskDataContext();
            this.userName = userName;
            this.main = main;
            this.DataContext = ATDC;
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Add(object sender, RoutedEventArgs e)
        {
            addTask();
        }

        private void addTask()
        {
            InfoObject info = SystemInterface.addTask(userName, ATDC.Title, ATDC.Description, ATDC.DueDateSet.ToShortDateString());
            if (info.getIsSucceeded())
            {
                main.show(SystemInterface.getBoard(userName));
                this.Close();
            }
            else
            {
                MessageBox.Show(info.getMessage());
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
