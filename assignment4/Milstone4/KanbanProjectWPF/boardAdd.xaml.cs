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
    /// Interaction logic for boardAdd.xaml
    /// </summary>
    public partial class boardAdd : Window
    {
        String boardName;
        BoardToAddDataContext ADB;
        string userName;
        MainWindow main;

        public boardAdd(string userName, MainWindow main)
        {
            InitializeComponent();
            ADB = new BoardToAddDataContext();
            this.DataContext = this.ADB;
            this.main = main;
            this.userName = userName;
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void board_add_Click(object sender, RoutedEventArgs e)
        {
            InfoObject info = SystemInterface.AddBoard(userName, ADB.BoardName);
            if(info.getIsSucceeded())
            {
                main.show(SystemInterface.getBoard(userName));
                this.Close();
            }
            else
            {
                MessageBox.Show(info.getMessage());
            }
        }
    }
}
