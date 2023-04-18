using BasketballApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using BasketballApp.Services;

using BasketballApp.Models;


namespace BasketballApp.ViewModels
{
  public class LoginViewModel : BaseViewModel
  {
    public Command LoginCommand { get; }

    public Command RegisterCommand { get; }


    public LoginViewModel()
    {
      LoginCommand = new Command(OnLoginClicked);

      RegisterCommand = new Command(RegisterUser);
    }

    private async void RegisterUser(object obj)
    {
      if (username != "" && password != "")
      {
        User user = BasketballDBService.addUser(username, "", password);
        if (user != null)
        {
          ApplicationData.currentlySignedInUser = BasketballDBService.getUser(user.Name);
          username = "";
          password = "";
          OnPropertyChanged("username");
          OnPropertyChanged("password");
          await Shell.Current.DisplayAlert("Registration Successful", "Welcome to the Dunk Analytics :)", "Next");
          await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
        else
        {
          await Shell.Current.DisplayAlert("Registration Failed", "Please try a different Username", "OK");
        }
      }
      else
      {
        await Shell.Current.DisplayAlert("Registration Failed", "Username and/or Password fields cannot be empty", "OK");
      }
    }


    private async void OnLoginClicked(object obj)
    {
      if (username != "" && password != "")
      {
        string result = BasketballDBService.checkUser(username, password);
        if (result == "Username Not Found")
        {
          await Shell.Current.DisplayAlert("Login Failed", "Incorrect Username", "OK");
        }
        else if (result == "Incorrect Password")
        {
          await Shell.Current.DisplayAlert("Login Failed", "Incorrect Password/Username Combo", "OK");
        }
        else if (result == "Successful Sign In")
        {
          ApplicationData.currentlySignedInUser = BasketballDBService.getUser(username);
          username = "";
          password = "";
          OnPropertyChanged("username");
          OnPropertyChanged("password");
          await Shell.Current.DisplayAlert("Login Successful", "Welcome to the Dunk Analytics :)", "Next");
          await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
      }
      else
      {
        await Shell.Current.DisplayAlert("Login Failed", "Username and/or Password fields cannot be empty", "OK");
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
