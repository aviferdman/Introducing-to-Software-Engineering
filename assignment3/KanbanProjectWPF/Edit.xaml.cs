using KanbanProjectWPF.PresentationLayer;
using Milstone2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KanbanProjectWPF
{
    /// <summary>
    /// Interaction logic for Edit.xaml
    /// </summary>
    public partial class Edit : Window
    {
        string userName;
        int CurrTaskStatus;
        int currTaskUID;
        MainWindow main;
        EditDataContext EDC;

        public Edit(string userName, PresentationBoard PTask, MainWindow main)
        {
            InitializeComponent();
            this.userName = userName;
            this.CurrTaskStatus = PTask.getTasColumn();
            this.currTaskUID = PTask.getTaskUID();
            EDC = new EditDataContext();
            EDC.Title = PTask.Title;
            EDC.Description = PTask.Description;
            EDC.DueDate = PTask.DueDate;
            this.main = main;
            this.DataContext = EDC;
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Task_Edit_Click(object sender, RoutedEventArgs e)
        {
            editTask();
        }

        private void editTask()
        {
            InfoObject titleInfo = SystemInterface.editTaskTitle(userName, currTaskUID, CurrTaskStatus, EDC.Title);
            InfoObject descriptionInfo = SystemInterface.editTaskDescription(userName, currTaskUID, CurrTaskStatus, EDC.Description);
            InfoObject dueDateInfo = SystemInterface.editTaskDueDate(userName, currTaskUID, CurrTaskStatus, EDC.DueDate.ToShortDateString());

            if (titleInfo.getIsSucceeded() && descriptionInfo.getIsSucceeded() && dueDateInfo.getIsSucceeded())
            {
                main.show(SystemInterface.getBoard(userName));
                this.Close();
            }
            else
            {
                MessageBox.Show(((titleInfo.getIsSucceeded()) ? "" : "\n" + titleInfo.getMessage()) + ((descriptionInfo.getIsSucceeded()) ? "" : "\n" + descriptionInfo.getMessage()) + ((dueDateInfo.getIsSucceeded()) ? "" : "\n" + dueDateInfo.getMessage()));
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
