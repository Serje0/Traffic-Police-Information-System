using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Traffic_Police_Information_System.Forms
{
    public partial class AddEditCollection : Window
    {
        string number_driver;
        Collections collection;
        bool edit;

        public AddEditCollection(string _number_driver, Collections _collection = null, bool _edit = false)
        {
            InitializeComponent();
            number_driver = _number_driver;
            collection = _collection;
            edit = _edit;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CB_TypeViolations.ItemsSource = db_GAIEntities.GetContext().Violations.ToList();
            CB_TypeViolations.SelectedIndex = 0;

            CB_Areas.ItemsSource = db_GAIEntities.GetContext().Areas.ToList();
            CB_Areas.SelectedIndex = 0;

            DP_Date.SelectedDate = DateTime.Now;
            DP_Date.DisplayDateEnd = DateTime.Now;
            TP_Time.Value = DateTime.Now;

            if (edit)
            {
                TB_IdCollection.Text = TB_IdCollection.Text + " №" + collection.id_collection;
                CB_TypeViolations.Text = collection.Violations.type_violation.ToString();
                DP_Date.SelectedDate = collection.date_violation;
                TimeSpan time = new TimeSpan(collection.time_violation.Hours, collection.time_violation.Minutes, 0);
                DateTime dateTime = DateTime.Today + time;
                TP_Time.Value = dateTime;
                CB_Areas.Text = collection.Areas.name_area.ToString();
                RB_Waring.IsChecked = collection.waring;

                if (collection.fine != null)
                {
                    RB_Fine.IsChecked = true;
                    IUD_Fine.Value = Convert.ToInt32(collection.fine * 1000);
                    CB_PaymentFine.IsChecked = collection.payment_fine;
                }
                else
                {
                    RB_Fine.IsChecked = false;
                }

                if (collection.period_dis != null)
                {
                    RB_PeriodDis.IsChecked = true;
                    IUD_PeriodDis.Value = Convert.ToInt32(collection.period_dis);
                }
                else
                {
                    RB_PeriodDis.IsChecked = false;
                }
            }
        }

        private void CB_TypeViolations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string type_violation = (CB_TypeViolations.SelectedItem as Violations).type_violation;
            var query_violation = db_GAIEntities.GetContext().Violations.FirstOrDefault(x => x.type_violation == type_violation);
            if (query_violation.waring == true)
            {
                RB_Waring.Visibility = Visibility.Visible;
            }
            else
            {
                RB_Waring.Visibility = Visibility.Hidden;
            }

            if (query_violation.max_share_BV != null)
            {
                RB_Fine.Visibility = Visibility.Visible;
                IUD_Fine.Visibility = Visibility.Visible;
                CB_PaymentFine.Visibility = Visibility.Visible;
                IUD_Fine.Minimum = Convert.ToInt32(query_violation.min_share_BV * 1000);
                IUD_Fine.Maximum = Convert.ToInt32(query_violation.max_share_BV * 1000);
                IUD_Fine.Value = Convert.ToInt32(query_violation.max_share_BV * 1000);
                IUD_Fine.ToolTip = $"от {query_violation.min_share_BV * 1000} до {query_violation.max_share_BV * 1000}";
            }
            else
            {
                RB_Fine.Visibility = Visibility.Hidden;
                IUD_Fine.Visibility = Visibility.Hidden;
                CB_PaymentFine.Visibility = Visibility.Hidden;
            }

            if (query_violation.max_period_dis != null)
            {
                RB_PeriodDis.Visibility = Visibility.Visible;
                IUD_PeriodDis.Visibility = Visibility.Visible;
                IUD_PeriodDis.Minimum = Convert.ToInt32(query_violation.min_period_dis);
                IUD_PeriodDis.Maximum = Convert.ToInt32(query_violation.max_period_dis);
                IUD_PeriodDis.Value = Convert.ToInt32(query_violation.max_period_dis);
                IUD_PeriodDis.ToolTip = $"от {query_violation.min_period_dis} до {query_violation.max_period_dis}";
            }
            else
            {
                RB_PeriodDis.Visibility = Visibility.Hidden;
                IUD_PeriodDis.Visibility = Visibility.Hidden;
            }
        }

        private void Bt_Save_Click(object sender, RoutedEventArgs e)
        {
            if (RB_Waring.IsChecked == false && RB_Fine.IsChecked == false && RB_PeriodDis.IsChecked == false)
            {
                MessageBox.Show("Выберите способа штрафа!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                string type_violation = (CB_TypeViolations.SelectedItem as Violations).type_violation;
                string area = (CB_Areas.SelectedItem as Areas).name_area;

                var query_driver = db_GAIEntities.GetContext().Drivers.FirstOrDefault(x => x.number_driver_license == number_driver);
                var query_violation = db_GAIEntities.GetContext().Violations.FirstOrDefault(x => x.type_violation == type_violation);
                var query_area = db_GAIEntities.GetContext().Areas.FirstOrDefault(x => x.name_area == area);
                var query_inspector = db_GAIEntities.GetContext().Inspectors.FirstOrDefault(x => x.number_inspector == "55 15 555555");

                if (collection == null)
                {
                    Collections AddCollection = new Collections()
                    {
                        id_violation = query_violation.id_violation,
                        date_violation = DP_Date.SelectedDate.Value,
                        time_violation = new TimeSpan(TP_Time.Value.Value.Hour, TP_Time.Value.Value.Minute, 0),
                        id_driver = query_driver.id_driver,
                        id_area = query_area.id_area,
                        waring = (bool)RB_Waring.IsChecked,
                        id_inspector = query_inspector.id_inspector,
                    };

                    if (RB_Fine.IsChecked == true)
                    {
                        AddCollection.fine = IUD_Fine.Value / 1000;
                        AddCollection.payment_fine = CB_PaymentFine.IsChecked;
                    }

                    if (RB_PeriodDis.IsChecked == true)
                    {
                        AddCollection.period_dis = IUD_PeriodDis.Value;
                    }

                    db_GAIEntities.GetContext().Collections.Add(AddCollection);
                }
                else
                {
                    collection.id_violation = query_violation.id_violation;
                    collection.date_violation = DP_Date.SelectedDate.Value;
                    collection.time_violation = new TimeSpan(TP_Time.Value.Value.Hour, TP_Time.Value.Value.Minute, 0);
                    collection.id_area = query_area.id_area;
                    collection.id_inspector = query_inspector.id_inspector;

                    if (RB_Waring.IsChecked == true)
                    {
                        collection.waring = (bool)RB_Waring.IsChecked;
                        collection.fine = null;
                        collection.payment_fine = null;
                        collection.period_dis = null;
                    }

                    if (RB_Fine.IsChecked == true)
                    {
                        collection.waring = false;
                        collection.fine = IUD_Fine.Value / 1000;
                        collection.payment_fine = CB_PaymentFine.IsChecked;
                        collection.period_dis = null;
                    }

                    if (RB_PeriodDis.IsChecked == true)
                    {
                        collection.waring = false;
                        collection.fine = null;
                        collection.payment_fine = null;
                        collection.period_dis = IUD_PeriodDis.Value;
                    }
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
