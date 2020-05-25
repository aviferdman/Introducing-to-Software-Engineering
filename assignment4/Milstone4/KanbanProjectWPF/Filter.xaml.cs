using KanbanProjectWPF.PresentationLayer;
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
    /// Interaction logic for Filter.xaml
    /// </summary>
    public partial class Filter : Window
    {
        MainWindow main;
        FilterDataContext FDC;
        public Filter(MainWindow mainW)
        {
            InitializeComponent();
            this.FDC = new FilterDataContext();
            this.main = mainW;
            this.DataContext = FDC;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.main.sortByWord(FDC.StringToFilter);
            this.Close();
        }

        private void Power_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
