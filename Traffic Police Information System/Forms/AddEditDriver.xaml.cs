using System;
using System.Linq;
using System.Windows;

namespace Traffic_Police_Information_System.Forms
{
    public partial class AddEditDriver : Window
    {
        Drivers driver;
        bool edit;
        bool UpdateNumberDriver = false;

        public AddEditDriver(Drivers _driver = null, bool _edit = false)
        {
            InitializeComponent();
            driver = _driver;
            edit = _edit;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (edit)
            {
                MTB_Number.Text = driver.number_driver_license;
                TB_Surname.Text = driver.surname_driver;
                TB_Name.Text = driver.name_driver;
                TB_Patronymic.Text = driver.patronymic_driver;
                TB_Adress.Text = driver.adress;
                MTB_Phone.Text = driver.phone;
            }
        }

        private void MTB_Number_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateNumberDriver = true;
        }

        private bool CheckNumberDriver()
        {
            var query_driver = db_GAIEntities.GetContext().Drivers.FirstOrDefault(x => x.name_driver == MTB_Number.Text);
            if (query_driver != null && !edit && UpdateNumberDriver)
            {
                MessageBox.Show("Номер занят!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void Bt_Save_Click(object sender, RoutedEventArgs e)
        {
            if (CheckNumberDriver())
            {
                if (driver == null)
                {
                    Drivers AddDriver = new Drivers()
                    {
                        number_driver_license = MTB_Number.Text,
                        surname_driver = TB_Surname.Text,
                        name_driver = TB_Name.Text,
                        patronymic_driver = TB_Patronymic.Text,
                        adress = TB_Adress.Text,
                        phone = MTB_Phone.Text,
                    };
                    db_GAIEntities.GetContext().Drivers.Add(AddDriver);
                }
                else
                {
                    driver.number_driver_license = MTB_Number.Text;
                    driver.surname_driver = TB_Surname.Text;
                    driver.name_driver = TB_Name.Text;
                    driver.patronymic_driver = TB_Patronymic.Text;
                    driver.adress = TB_Adress.Text;
                    driver.phone = MTB_Phone.Text;
                }

                try
                {
                    db_GAIEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена!", "Инфорация", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
