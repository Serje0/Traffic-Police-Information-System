using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Traffic_Police_Information_System.Forms;

namespace Traffic_Police_Information_System
{
    public partial class MainWindow : Window
    {
        Users user;
        string number_driver;

        public MainWindow(Users _user = null)
        {
            InitializeComponent();
            user = _user;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LB_driver.ItemsSource = db_GAIEntities.GetContext().Drivers.ToList();
            LB_driver.SelectedIndex = 0;
            TB_surname_user.Text = user.Inspectors.surname_inspector;
            TB_name_user.Text = user.Inspectors.name_inspector;
            TB_patronymic_user.Text = user.Inspectors.patronymic_inspector;
            TB_name_role.Text = user.Roles.name_role;
            Image_user.Source = new BitmapImage(new Uri(user.Inspectors.image_face, UriKind.RelativeOrAbsolute));

            if (user.Roles.name_role.Equals("Инспектор"))
            {
                Menu_Drivers.Visibility = Visibility.Hidden;
                DG_Collection_Edit.Visibility = Visibility.Hidden;
                DG_Car_Edit.Visibility = Visibility.Hidden;
                Bt_DeleteCollection.Visibility = Visibility.Hidden;
                Bt_DeleteCar.Visibility = Visibility.Hidden;
            }
        }

        private void UpdateDG_Car()
        {
            var query_cars = from cars in db_GAIEntities.GetContext().Cars
                             where cars.Drivers.number_driver_license == number_driver
                             select cars;
            DG_Car.ItemsSource = query_cars.ToList();
        }

        private void UpdateDG_Collection()
        {
            var query_collections = from collections in db_GAIEntities.GetContext().Collections
                                    where collections.Drivers.number_driver_license == number_driver
                                    select collections;

            DG_Collection.ItemsSource = query_collections.ToList();
        }

        private void LB_driver_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LB_driver.SelectedItem != null)
            {
                number_driver = (LB_driver.SelectedItem as Drivers).number_driver_license;
                TB_Number.Text = number_driver;

                var query_driver = db_GAIEntities.GetContext().Drivers.FirstOrDefault(x => x.number_driver_license == number_driver);

                TB_Name.Text = query_driver.name_driver;
                TB_Surname.Text = query_driver.surname_driver;
                TB_Patronymic.Text = query_driver.patronymic_driver;
                TB_Adress.Text = query_driver.adress;
                TB_Phone.Text = query_driver.phone;
                UpdateDG_Car();
                UpdateDG_Collection();

                TB_CountDrivers.Text = "Количество водителей: " + db_GAIEntities.GetContext().Drivers.Count().ToString();
            }
        }

        private void TB_FindSurname_TextChanged(object sender, RoutedEventArgs e)
        {
            string text = TB_FindSurname.Text;
            var query_driver = from drivers in db_GAIEntities.GetContext().Drivers
                               where drivers.surname_driver.StartsWith(text)
                               select drivers;
            LB_driver.ItemsSource = query_driver.ToList();
            LB_driver.SelectedIndex = 0;
            TB_CountDrivers.Text = "Количество водителей: " + query_driver.Count().ToString();
            if (LB_driver.Items.Count == 0)
            {
                MessageBox.Show("Нет данных!", "Инфорация", MessageBoxButton.OK, MessageBoxImage.Information);
                TB_FindSurname.Text = "";
            }
        }

        private void Bt_AddCollection_Click(object sender, RoutedEventArgs e)
        {
            AddEditCollection addEditCollection = new AddEditCollection(number_driver);
            addEditCollection.ShowDialog();
            UpdateDG_Collection();
        }

        private void Bt_EditCollection_Click(object sender, RoutedEventArgs e)
        {            
            AddEditCollection addEditCollection = new AddEditCollection(number_driver, (sender as Button).DataContext as Collections, true);
            addEditCollection.ShowDialog();
            UpdateDG_Collection();
        }

        private void Bt_DeleteCollection_Click(object sender, RoutedEventArgs e)
        {
            var query_remove = DG_Collection.SelectedItems.Cast<Collections>().ToList();

            MessageBoxResult result = MessageBox.Show($"Вы точно хотите удалить следующие {query_remove.Count} элементов?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    db_GAIEntities.GetContext().Collections.RemoveRange(query_remove);
                    db_GAIEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!", "Инфорация", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateDG_Collection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Bt_AddCar_Click(object sender, RoutedEventArgs e)
        {
            AddEditCar addEditCar = new AddEditCar(number_driver);
            addEditCar.ShowDialog();
            UpdateDG_Car();
        }

        private void Bt_EditCar_Click(object sender, RoutedEventArgs e)
        {
            AddEditCar addEditCar = new AddEditCar(number_driver, (sender as Button).DataContext as Cars, true);
            addEditCar.ShowDialog();
            UpdateDG_Car();
        }

        private void Bt_DeleteCar_Click(object sender, RoutedEventArgs e)
        {
            var query_remove = DG_Car.SelectedItems.Cast<Cars>().ToList();

            MessageBoxResult result = MessageBox.Show($"Вы точно хотите удалить следующие {query_remove.Count} элементов?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    db_GAIEntities.GetContext().Cars.RemoveRange(query_remove);
                    db_GAIEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!", "Инфорация", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateDG_Car();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddDriver_Click(object sender, RoutedEventArgs e)
        {
            AddEditDriver addEditDriver = new AddEditDriver();
            addEditDriver.ShowDialog();
            LB_driver.ItemsSource = db_GAIEntities.GetContext().Drivers.ToList();
        }

        private void EditDriver_Click(object sender, RoutedEventArgs e)
        {
            AddEditDriver addEditDriver = new AddEditDriver(LB_driver.SelectedItem as Drivers, true);
            addEditDriver.ShowDialog();
            LB_driver.ItemsSource = db_GAIEntities.GetContext().Drivers.ToList();
        }

        private void DeleteDriver_Click(object sender, RoutedEventArgs e)
        {
            var query_remove = db_GAIEntities.GetContext().Drivers.FirstOrDefault(x => x.number_driver_license == number_driver);

            MessageBoxResult result = MessageBox.Show($"Вы точно хотите удалить водителя с номером {number_driver}?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (query_remove.Cars.Count == 0 && query_remove.Collections.Count == 0)
                {
                    try
                    {
                        db_GAIEntities.GetContext().Drivers.Remove(query_remove);
                        db_GAIEntities.GetContext().SaveChanges();
                        MessageBox.Show("Данные удалены!", "Инфорация", MessageBoxButton.OK, MessageBoxImage.Information);
                        LB_driver.ItemsSource = db_GAIEntities.GetContext().Drivers.ToList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Удалите сначала всех автомобилей и взысканий!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Bt_Exit_Click(object sender, RoutedEventArgs e)
        {
            FormLoginPassword formLoginPassword = new FormLoginPassword();
            formLoginPassword.Show();
            this.Close();
        }
    }
}
