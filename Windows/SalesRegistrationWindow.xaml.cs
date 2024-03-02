using ApiInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace ApiInterface.Windows
{
    /// <summary>
    /// Interaction logic for SalesRegistrationWindow.xaml
    /// </summary>
    public partial class SalesRegistrationWindow : Window
    {
        private HttpClient client;
        private SalesRegistration? salesRegistration;
        public SalesRegistrationWindow(String token)
        {
            InitializeComponent();
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Task.Run(() => LoadPrice());
        }
        public SalesRegistrationWindow(String token, SalesRegistration salesRegistration)
        {
            InitializeComponent();
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Task.Run(() => LoadPrice());
            DishName.Text = salesRegistration.DishName;
            DishCode.Text = salesRegistration.DishCode;
            TotalCost.Text = salesRegistration.TotalCost;
            QuantitySold.Text = salesRegistration.QuantitySold;
            SaleDate.SelectedDate = salesRegistration.SaleDate;
            cbPrice.SelectedItem = salesRegistration.PriceList!.DishName;
        }
        private async void LoadPrice()
        {
            List<PriceList>? list = await client.GetFromJsonAsync<List<PriceList>>("http://localhost:7084/api/PosudaAPI/DefaultPriceList");
            Dispatcher.Invoke(() =>
            {
                cbPrice.ItemsSource = list?.Select(p => p.DishName);
            });
        }
        public string? NameProperty
        {
            get { return DishName.Text; }
        }
        public string? CodeProperty
        {
            get { return DishCode.Text; }
        }
        public string? TotalCostProperty
        {
            get { return TotalCost.Text; }
        }
        public string? QuantitySoldProperty
        {
            get { return QuantitySold.Text; }
        }
        public DateTime? SaleDateProperty
        {
            get { return DateTime.Parse(SaleDate.Text); }
        }
        public async Task<int> getIdPriceList()
        {
            PriceList? priceList = await client.GetFromJsonAsync<PriceList>("http://localhost:7084/api/PosudaAPI/DefaultPriceList");
            return priceList!.Id;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
