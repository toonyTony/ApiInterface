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
using System.Windows.Shapes;
using ApiInterface.Models;

namespace ApiInterface.Windows
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpClient httpClient;
        private MainWindow mainWindow;
        private string? token;

        public MainWindow(Response response, MainWindow window)
        {
            InitializeComponent();
            this.mainWindow = window;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + response.access_token);
            token = response.access_token;
            Task.Run(() => Load());
        }
        private async Task Load()
        {
            List<SalesRegistration>? list = await httpClient.GetFromJsonAsync<List<SalesRegistration>>("http://localhost:7084/api/PosudaAPI/DefaultSalesRegistration");
            foreach(SalesRegistration i in list!)
            {
                i.PriceList = await httpClient.GetFromJsonAsync<Models.PriceList>("http://localhost:7084/api/PosudaAPI/DefaultPriceList" + i.DishCode);
            }
            Dispatcher.Invoke(() =>
            {
                ListSalesReg.ItemsSource = null;
                ListSalesReg.Items.Clear();
                ListSalesReg.ItemsSource = list;
            });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.mainWindow.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PriceListWindow priceListWindow = new PriceListWindow(token!);
            priceListWindow.ShowDialog();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SalesRegistrationWindow salesRegistrationWindow = new SalesRegistrationWindow(token!);
            if (salesRegistrationWindow.ShowDialog() == true)
            {
                SalesRegistration salesRegistration = new SalesRegistration
                {
                    DishName = salesRegistrationWindow.NameProperty,
                    DishCode = salesRegistrationWindow.CodeProperty,
                    TotalCost = salesRegistrationWindow.TotalCostProperty,
                    QuantitySold = salesRegistrationWindow.QuantitySoldProperty,
                    SaleDate = salesRegistrationWindow.SaleDateProperty,
                    Id = await salesRegistrationWindow.getIdPriceList()
                };
                JsonContent content = JsonContent.Create(salesRegistrationWindow);
                using var response = await httpClient.PostAsync("http://localhost:7084/api/PosudaAPI/DefaultSalesRegistration", content);
                string responseText = await response.Content.ReadAsStringAsync();
                await Load();
            }
        }
        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SalesRegistration? st = ListSalesReg.SelectedItem as SalesRegistration;
            SalesRegistrationWindow salesRegistrationWindow = new SalesRegistrationWindow(token!, st!);
            if (salesRegistrationWindow.ShowDialog() == true)
            {
                st!.DishName = salesRegistrationWindow.NameProperty;
                st!.DishCode = salesRegistrationWindow.CodeProperty;
                st!.TotalCost = salesRegistrationWindow.TotalCostProperty;
                st!.QuantitySold = salesRegistrationWindow.QuantitySoldProperty;
                st!.SaleDate = salesRegistrationWindow.SaleDateProperty;
                st!.Id = await salesRegistrationWindow.getIdPriceList();
                JsonContent content = JsonContent.Create(st);
                using var response = await httpClient.PutAsync("http://localhost:7084/api/PosudaAPI/DefaultSalesRegistration", content);
                string responseText = await response.Content.ReadAsStringAsync();
                await Load();
            }
        }
        private async void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            SalesRegistration? st = ListSalesReg.SelectedItem as SalesRegistration;
            JsonContent content = JsonContent.Create(st);
            using var response = await httpClient.DeleteAsync("http://localhost:7084/api/PosudaAPI/DefaultSalesRegistration" + st!.Id);
            string responseText = await response.Content.ReadAsStringAsync();
            await Load();
        }

    }
}
