using BasketballApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using BasketballApp.Services;


namespace BasketballApp.ViewModels
{
  public class LoginViewModel : BaseViewModel
  {
    public Command LoginCommand { get; }

    public LoginViewModel()
    {
      LoginCommand = new Command(OnLoginClicked);
    }

    private async void OnLoginClicked(object obj)
    {
      string result = BasketballDBService.checkUser(username, password);
      if (result == "Username Not Found")
      {
        await Shell.Current.DisplayAlert("Login Failed", "Incorrect Username", "OK");
      }
      else if(result == "Incorrect Password")
      {
        await Shell.Current.DisplayAlert("Login Failed", "Incorrect Password/Username Combo", "OK");
      }
      else if (result == "Successful Sign In")
      {
        await Shell.Current.DisplayAlert("Login Successful", "Welcome to the Basketball Data Collection App :)", "Next");
        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
      }
       
    }
    public string username
    {
      get;
      set;
    }

    public string password
    {
      get;
      set;
    }
  }
}
