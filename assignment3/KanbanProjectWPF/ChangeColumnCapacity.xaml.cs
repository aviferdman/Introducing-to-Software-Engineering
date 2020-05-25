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
    /// Interaction logic for ChangeColumnCapacity.xaml
    /// </summary>
    public partial class ChangeColumnCapacity : Window
    {
        string userName;
        ChangeCapacityDataContext CCDC;
        DataGrid column;
        MainWindow main;
        public ChangeColumnCapacity(string userName, MainWindow main,DataGrid selectedColumn)
        {
            InitializeComponent();
            CCDC = new ChangeCapacityDataContext();
            this.column = selectedColumn;
            this.main = main;
            this.userName = userName;
            this.DataContext = this.CCDC;
        }

        private void Column_Capacity_edit_Click(object sender, RoutedEventArgs e)
        {
            InfoObject info = SystemInterface.changeColumnCapacity(this.userName, (int)this.column.Tag, CCDC.ColumnCapacity);
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

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
