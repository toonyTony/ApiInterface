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
using System.Windows.Shapes;

namespace ApiInterface.Windows
{
    /// <summary>
    /// Interaction logic for PriceListWindow.xaml
    /// </summary>
    public partial class PriceListWindow : Window
    {
        private HttpClient client;
        private PriceList? price;
        public PriceListWindow(String token)
        {
            InitializeComponent();
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Task.Run(() => Load());
        }
        private async Task Load()
        {
            List<PriceList>? list = await client.GetFromJsonAsync<List<PriceList>>("http://localhost:7084/api/PosudaAPI/DefaultPriceList");
            Dispatcher.Invoke(() =>
            {
                ListPosudaPrice.ItemsSource = null;
                ListPosudaPrice.Items.Clear();
                ListPosudaPrice.ItemsSource = list;
            });
        }
        private async Task Save()
        {
            PriceList priceList = new PriceList
            {
                DishCode = DishCode.Text,
                DishName = DishName.Text,
                Price = Price.Text
            };
            JsonContent content = JsonContent.Create(priceList);
            using var response = await client.PostAsync("http://localhost:7084/api/PosudaAPI/DefaultPriceList", content);
            string responseText = await response.Content.ReadAsStringAsync();
            await Load();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Save();
        }

        private void ListPosudaPrice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            price = ListPosudaPrice.SelectedItem as PriceList;
            DishCode.Text = price?.DishCode;
            DishName.Text = price?.DishName;
            Price.Text = price?.Price;
        }

        private async Task Delete()
        {
            using var response = await client.DeleteAsync("http://localhost:7084/api/PosudaAPI/DefaultPriceList/" + price?.Id);
            string responseText = await response.Content.ReadAsStringAsync();
            await Load();
        }
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            await Delete();
        }

    }
}
