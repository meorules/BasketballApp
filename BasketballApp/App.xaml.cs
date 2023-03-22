﻿//using BasketballApp.Services;
using BasketballApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BasketballApp
{
  public partial class App : Application
  {

    public App()
    {
      InitializeComponent();

      //DependencyService.Register<MockDataStore>();
      Xamarin.Forms.DataGrid.DataGridComponent.Init();


      MainPage = new AppShell();
    }

    protected override void OnStart()
    {
    }

    protected override void OnSleep()
    {
    }

    protected override void OnResume()
    {
    }
  }
}
