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
    /// Interaction logic for RemoveColumn.xaml
    /// </summary>
    public partial class RemoveColumn : Window
    {

        MainWindow main;
        string userName;
        RemoveColumnDataContext RCDC;
        public RemoveColumn(string userName, MainWindow main)
        {
            InitializeComponent();
            this.userName = userName;
            this.main = main;
            RCDC = new RemoveColumnDataContext();
            this.DataContext = RCDC;

        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Remove(object sender, RoutedEventArgs e)
        {
            removeColumn();
        }

        private void removeColumn()
        {
            InfoObject info = SystemInterface.removeColumn(userName, RCDC.ColumnName);
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
