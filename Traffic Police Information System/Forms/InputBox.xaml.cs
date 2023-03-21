using System.Windows;

namespace Traffic_Police_Information_System.Forms
{
    public partial class InputBox : Window
    {
        public string text;
        public bool save = false;

        public InputBox()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            text = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            text = TB_Text.Text;
            save = true;
            this.Close();
        }
    }
}
