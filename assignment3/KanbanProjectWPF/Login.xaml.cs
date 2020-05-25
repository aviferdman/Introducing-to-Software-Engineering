using BespokeFusion;
using KanbanProjectWPF.PresentationLayer;
using Milstone2;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KanbanProjectWPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        LoginDataContext LDC;
        public Login()
        {
            InitializeComponent();
            LDC = new LoginDataContext();
            this.DataContext = this.LDC;
        }

        private void Power_Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_SignUp(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            register.Show();
            this.Close();
        }

        private void Button_Login(object sender, RoutedEventArgs e)
        {
            login();
        }

        public void login()
        {
            InfoObject info = SystemInterface.login(LDC.UserName1, LDC.Password);
            if (info.getIsSucceeded())
            {
                MainWindow main = new MainWindow(LDC.UserName1, LDC.Password);
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(info.getMessage());
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            LDC.Password = PasswordBox.Password;
        }
    }
}
