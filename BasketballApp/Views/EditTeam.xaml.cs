using BasketballApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BasketballApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BasketballApp.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class EditTeam : ContentPage
  {
    public EditTeam()
    {
      this.BindingContext = new EditTeamViewModel();

      InitializeComponent();
      Title = "Build Or Edit Team";
    }

    protected override async void OnAppearing()
    {
      base.OnAppearing();
      if(ApplicationData.currentlySelectedTeam == null)
      {
        Header.Text = "Build a Team";
      }
      else
      {
        Header.Text = "Edit Team: " + ApplicationData.currentlySelectedTeam.Name;

      }
    }
    }
}