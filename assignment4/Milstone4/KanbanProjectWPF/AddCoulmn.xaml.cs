using BespokeFusion;
using KanbanProjectWPF.PresentationLayer;
using Milstone2;
using System.Windows;
using System.Windows.Input;

namespace KanbanProjectWPF
{
    /// <summary>
    /// Interaction logic for AddCoulmn.xaml
    /// </summary>
    public partial class AddCoulmn : Window
    {
        string userName;
        AddColumnDataContext ACDC;
        MainWindow main;

        public AddCoulmn(string userName, MainWindow main)
        {
            InitializeComponent();
            ACDC = new AddColumnDataContext();
            this.main = main;
            this.userName = userName;
            this.DataContext = this.ACDC;
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Add(object sender, RoutedEventArgs e)
        {
            addColumn();
        }

        private void addColumn()
        {
            InfoObject info = SystemInterface.addColumn(userName, ACDC.ColumnName);
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
