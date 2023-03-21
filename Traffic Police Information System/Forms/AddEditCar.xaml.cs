using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Traffic_Police_Information_System.Forms
{
    public partial class AddEditCar : Window
    {
        string number_driver;
        Cars car;
        bool edit;
        bool UpdateNumberCar = false;

        public AddEditCar(string _number_driver, Cars _car = null, bool _edit = false)
        {
            InitializeComponent();
            number_driver = _number_driver;
            car = _car;
            edit = _edit;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CB_Brands.ItemsSource = db_GAIEntities.GetContext().Brands.ToList();
            CB_Brands.SelectedIndex = 0;
            CB_Colors.ItemsSource = db_GAIEntities.GetContext().Colors.ToList();
            CB_Colors.SelectedIndex = 0;
            IUD_year.Maximum = Convert.ToInt32(DateTime.Now.Year);
            IUD_year.Value = Convert.ToInt32(DateTime.Now.Year);
            DP_DateGUI.SelectedDate = DateTime.Now;

            if (edit)
            {
                TB_Number.Text = car.number_car;
                CB_Brands.Text = car.Models.Brands.name_brand;
                CB_Models.Text = car.Models.name_model;
                CB_Colors.Text = car.Colors.name_color;
                IUD_year.Value = Convert.ToInt32(car.year_issue.Year);
                DP_DateGUI.SelectedDate = car.date_GAI;
            }
        }

        private void TB_Number_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateNumberCar = true;
        }

        private void CB_Brands_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string brand = (CB_Brands.SelectedItem as Brands).name_brand;
            CB_Models.ItemsSource = db_GAIEntities.GetContext().Models.ToList().Where(x => x.Brands.name_brand == brand);
            CB_Models.SelectedIndex = 0;
        }

        private void Bt_AddBrand_Click(object sender, RoutedEventArgs e)
        {
            InputBox inputBox = new InputBox();
            inputBox.ShowDialog();
            if (!inputBox.text.Equals("") && inputBox.save == true)
            {
                var query_brand = db_GAIEntities.GetContext().Brands.FirstOrDefault(x => x.name_brand == inputBox.text);
                if (query_brand != null)
                {
                    MessageBox.Show("Не удалось сохранить! Этот элемент существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Brands AddBrand = new Brands()
                    {
                        name_brand = inputBox.text,
                    };
                    db_GAIEntities.GetContext().Brands.Add(AddBrand);
                    db_GAIEntities.GetContext().SaveChanges();
                    CB_Brands.ItemsSource = db_GAIEntities.GetContext().Brands.ToList();
                    CB_Brands.SelectedIndex = 0;
                }
            }
        }

        private void Bt_AddModel_Click(object sender, RoutedEventArgs e)
        {
            InputBox inputBox = new InputBox();
            inputBox.ShowDialog();
            if (!inputBox.text.Equals("") && inputBox.save == true)
            {
                var query_model = db_GAIEntities.GetContext().Models.FirstOrDefault(x => x.name_model == inputBox.text);
                if (query_model != null)
                {
                    MessageBox.Show("Не удалось сохранить! Этот элемент существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    string brand = (CB_Brands.SelectedItem as Brands).name_brand;

                    Models AddModel = new Models()
                    {
                        name_model = inputBox.text,
                        id_brand = db_GAIEntities.GetContext().Brands.FirstOrDefault(x => x.name_brand == brand).id_brand
                    };
                    db_GAIEntities.GetContext().Models.Add(AddModel);
                    db_GAIEntities.GetContext().SaveChanges();
                    CB_Models.ItemsSource = db_GAIEntities.GetContext().Models.ToList();
                    CB_Models.SelectedIndex = 0;
                }
            }
        }

        private void Bt_AddColor_Click(object sender, RoutedEventArgs e)
        {
            InputBox inputBox = new InputBox();
            inputBox.ShowDialog();
            if (!inputBox.text.Equals("") && inputBox.save == true)
            {
                var query_color = db_GAIEntities.GetContext().Colors.FirstOrDefault(x => x.name_color == inputBox.text);
                if (query_color != null)
                {
                    MessageBox.Show("Не удалось сохранить! Этот элемент существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Colors AddColor = new Colors()
                    {
                        name_color = inputBox.text,
                    };
                    db_GAIEntities.GetContext().Colors.Add(AddColor);
                    db_GAIEntities.GetContext().SaveChanges();
                    CB_Colors.ItemsSource = db_GAIEntities.GetContext().Colors.ToList();
                    CB_Colors.SelectedIndex = 0;
                }
            }
        }

        private bool CheckNumberCar()
        {
            var query_car = db_GAIEntities.GetContext().Cars.FirstOrDefault(x => x.number_car == TB_Number.Text);
            if (query_car != null && !edit && UpdateNumberCar)
            {
                MessageBox.Show("Номер занят!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void Bt_Save_Click(object sender, RoutedEventArgs e)
        {
            if (CheckNumberCar())
            {
                string model = (CB_Models.SelectedItem as Models).name_model;
                string color = (CB_Colors.SelectedItem as Colors).name_color;

                var query_driver = db_GAIEntities.GetContext().Drivers.FirstOrDefault(x => x.number_driver_license == number_driver);
                var query_model = db_GAIEntities.GetContext().Models.FirstOrDefault(x => x.name_model == model);
                var query_color = db_GAIEntities.GetContext().Colors.FirstOrDefault(x => x.name_color == color);


                if (car == null)
                {
                    Cars AddCar = new Cars()
                    {
                        number_car = TB_Number.Text,
                        id_driver = query_driver.id_driver,
                        id_model = query_model.id_model,
                        id_color = query_color.id_color,
                        year_issue = new DateTime((int)IUD_year.Value, 1, 1),
                        date_GAI = DateTime.Now
                    };
                    db_GAIEntities.GetContext().Cars.Add(AddCar);
                }
                else
                {
                    car.number_car = TB_Number.Text;
                    car.id_driver = query_driver.id_driver;
                    car.id_model = query_model.id_model;
                    car.id_color = query_color.id_color;
                    car.year_issue = new DateTime((int)IUD_year.Value, 1, 1);
                    car.date_GAI = DateTime.Now;
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
