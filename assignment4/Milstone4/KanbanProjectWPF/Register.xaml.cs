using BespokeFusion;
using KanbanProjectWPF.PresentationLayer;
using Milstone2;
using System.Windows;
using System.Windows.Input;

namespace KanbanProjectWPF
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        RegisterDataContext RDC;

        public Register()
        {
            InitializeComponent();
            RDC = new RegisterDataContext();
            this.DataContext = RDC;
        }


        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Power_Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            register();
        }

        private void register()
        {
            InfoObject info = SystemInterface.register(RDC.UserName1, RDC.Password);
            if (info.getIsSucceeded())
            {
                Login login = new Login();
                login.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(info.getMessage());
            }
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            RDC.Password = PasswordBox.Password;
        }
    }
}
