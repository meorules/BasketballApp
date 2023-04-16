using BasketballApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using BasketballApp.Models;
using BasketballApp.ViewModels;

namespace BasketballApp.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class HomePage : ContentPage
  {
    User currentUser;
    public HomePage()
    {
      InitializeComponent();
      setupUserIntro();
      setUpTeamPicker();

    }

    protected override async void OnAppearing()
    {
      base.OnAppearing();
      
      setupUserIntro();
      setUpTeamPicker();

    }

    private async void Logout(object sender, EventArgs e)
    {
      ApplicationData.currentlySignedInUser = null;
      await Shell.Current.GoToAsync("//LoginPage");
    }

    private async void StartGame(object sender, EventArgs e)
    {
      ApplicationData.currentlySelectedGame= null;
      await Shell.Current.GoToAsync("//DataCollectionPage");
    }

    private void changeSelectedTeam(object sender, EventArgs e)
    {
      if (TeamPicker.SelectedItem != null)
      {
        string teamName = TeamPicker.SelectedItem.ToString();
        Team team = BasketballDBService.getTeam(teamName);
        if (team != null)
        {
          ApplicationData.currentlySelectedTeam = team;
          TeamPickerLabel.Text = "Team Picked :)";
          StatLeaderBox.BackgroundColor = Color.White;
        }
        else
        {
          throw new Exception("Problem finding selected team");
        }
      }
      else
      {
        StatLeaderBox.BackgroundColor = Color.Gray;
      }
    }

    private async void setupUserIntro()
    {
      if (ApplicationData.currentlySignedInUser == null)
      {
        //For now, just set user to test user, otherwise, send back to login page
        currentUser = BasketballDBService.getUser("Meo");

        /*await Shell.Current.DisplayAlert("Login Failed", "Not correctly signed in", "OK");
        ApplicationData.currentlySignedInUser = null;
        await Shell.Current.GoToAsync("//LoginPage");*/

      }
      else
      {
        currentUser = BasketballDBService.getUser(ApplicationData.currentlySignedInUser.Name);
      }
      UsernameIntro.Text = "Hi " + ApplicationData.currentlySignedInUser.Name;
    }

    private void setUpTeamPicker()
    {
      if (currentUser != null)
      {
        if (currentUser.Teams != null)
        {
          String[] teamList = new string[currentUser.Teams.Count];
          for (int i = 0; i < currentUser.Teams.Count; i++)
          {
            teamList[i] = currentUser.Teams[i].Name;
          }
          TeamPicker.ItemsSource = teamList;
        }
        else
        {
          TeamPicker.ItemsSource = null;
        }
      }
    }
  }
}