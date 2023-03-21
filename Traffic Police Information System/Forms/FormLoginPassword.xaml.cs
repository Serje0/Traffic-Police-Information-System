using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Traffic_Police_Information_System.Classes;
using static System.Net.Mime.MediaTypeNames;

namespace Traffic_Police_Information_System.Forms
{
    public partial class FormLoginPassword : Window
    {
        Users user;
        ObservableCollection<Captcha> captchas;
        CodeBlock codeBlock = new CodeBlock();

        public FormLoginPassword()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            captchas = new ObservableCollection<Captcha>()
            {
                new Captcha("/Images/Captcha/captcha1.png", "4HW7"),
                new Captcha("/Images/Captcha/captcha2.jpg", "8AnF")
            };

            Image.Source = new BitmapImage(new Uri(captchas[0].GetImage(), UriKind.RelativeOrAbsolute));
        }

        private void CB_Password_Checked(object sender, RoutedEventArgs e)
        {
            PB_Password.Visibility = Visibility.Hidden;
            TB_Password.Visibility = Visibility.Visible;
            TB_Password.Text = PB_Password.Password;
        }

        private void CB_Password_Unchecked(object sender, RoutedEventArgs e)
        {
            TB_Password.Visibility = Visibility.Hidden;
            PB_Password.Visibility = Visibility.Visible;
            PB_Password.Password = TB_Password.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!codeBlock.IsLocked)
            {
                string password = PB_Password.Password;
                if ((bool)CB_Password.IsChecked)
                {
                    password = TB_Password.Text;
                }
                PB_Password.Password = password;
                TB_Password.Text = password;

                if (TB_Login.Text.Length != 0 && TB_Password.Text.Length != 0 && PB_Password.Password.Length != 0 && TB_Captcha.Text.Length != 0)
                {
                    var query_user = db_GAIEntities.GetContext().Users.FirstOrDefault(x => x.login_user == TB_Login.Text && x.password_user == password);
                    if (query_user != null)
                    {
                        if (captchas[0].CheckKod(TB_Captcha.Text))
                        {
                            user = query_user;
                            MainWindow mainWindow = new MainWindow(user);
                            mainWindow.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Неверный код captcha!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            codeBlock.Block();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Не все заполнены!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

    }
}
