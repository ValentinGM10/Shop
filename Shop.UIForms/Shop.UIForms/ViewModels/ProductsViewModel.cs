using Shop.Common.Models;
using Shop.Common.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace Shop.UIForms.ViewModels
{
    public class ProductsViewModel : BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<Product> products;
        private bool isRefreshing;

        public bool IsRefreshing 
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }


        // public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Product> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }


        public ProductsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadProducts();
        }


        // https://shopvale.azurewebsites.net/Api/Products

        private async void LoadProducts()
        {
            this.isRefreshing = true;

            var response = await this.apiService.GetListAsync<Product>(
                "https://shopvale.azurewebsites.net",
                "/api",
                "/Products");

            this.isRefreshing = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            var products = (List<Product>)response.Result;
            this.Products = new ObservableCollection<Product>(products);
        }

    }
}
