﻿using GalaSoft.MvvmLight.Command;
using Shop.UIForms.Views;
using System.Windows.Input;
using Xamarin.Forms;


// Video 21 
namespace Shop.UIForms.ViewModels
{
    //public class LoginViewModel 
    public class LoginViewModel 
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public ICommand LoginCommand => new RelayCommand(this.Login);

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter an email", "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter a password", "Accept");
                return;
            }

            if (!this.Email.Equals("vgalvez10@gmail.com") || !this.Password.Equals("123456"))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Incorrect user or password", "Accept");
                return;
            }

            //await Application.Current.MainPage.DisplayAlert(
            //"Ok", 
            //"Fuck yeah!!!", 
            //"Accept");
            
            MainViewModel.GetInstance().Products = new ProductsViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new ProductsPage());


        }

    }
}