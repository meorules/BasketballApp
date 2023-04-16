using BasketballApp.Views;
using BasketballApp.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using BasketballApp.Services;

namespace BasketballApp
{
  public partial class AppShell : Xamarin.Forms.Shell
  {
    public AppShell()
    {
      InitializeComponent();
    }



    private async void Logout(object sender, EventArgs e)
    {
      ApplicationData.currentlySignedInUser = null;
      await Shell.Current.GoToAsync("//LoginPage");
    }
  }
}
