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
      ApplicationData.clearAllData();
      await Shell.Current.GoToAsync("//LoginPage");
    }

    private async void StartGame(object sender, EventArgs e)
    {
      ApplicationData.currentlySelectedGame= null;
      await Shell.Current.GoToAsync("//DataCollectionPage");
    }

    private async void ViewGame(object sender, EventArgs e)
    {
      await Shell.Current.GoToAsync("//ViewGamesListPage");
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
            ViewDataButton.IsEnabled = true;
            ViewDataButton.BackgroundColor = Color.White;
            EditTeamButton.IsEnabled = true;
            EditTeamButton.Text = "Edit Team";
            EditTeamButton.BackgroundColor = Color.White;
            StartGameButton.IsEnabled = true;
            StartGameButton.BackgroundColor = Color.White;
          }
        }
        else
        {
          StartGameButton.IsEnabled = false;
          StartGameButton.BackgroundColor = Color.LightSlateGray;
          StatLeaderBox.BackgroundColor = Color.Gray;
          EditTeamButton.IsEnabled = false;
          EditTeamButton.BackgroundColor = Color.LightSlateGray;
          ViewDataButton.IsEnabled = false;
          ViewDataButton.BackgroundColor = Color.LightSlateGray;
      }
    }

    private async void setupUserIntro()
    {
      if (ApplicationData.currentlySignedInUser == null)
      {
        await Shell.Current.DisplayAlert("Login Failed", "Not correctly signed in", "OK");
        ApplicationData.currentlySignedInUser = null;
        await Shell.Current.GoToAsync("//LoginPage");

      }
      else
      {
        currentUser = BasketballDBService.getUser(ApplicationData.currentlySignedInUser.Name);
      }
      UsernameIntro.Text = "Hi " + currentUser.Name;
    }

    internal class TeamPickerArgs : EventArgs
    {
      public bool SettingTeamUp
      {
        get; set;
      }
    }

    private void setUpTeamPicker()
    {
      if (ApplicationData.currentlySelectedTeam != null)
      {
        String[] teamList = new string[currentUser.Teams.Count];
        for (int i = 0; i < currentUser.Teams.Count; i++)
        {
          teamList[i] = currentUser.Teams[i].Name;
        }
        TeamPicker.ItemsSource = teamList;
        TeamPicker.SelectedItem = ApplicationData.currentlySelectedTeam;
        TeamPicker.SelectedIndex = 0;
      }
      else
      {
        TeamPickerLabel.Text = "Pick a Team";
        if (currentUser != null)
        {
          if (currentUser.Teams != null && currentUser.Teams.Count != 0)
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
        StartGameButton.IsEnabled = false;
        StartGameButton.BackgroundColor = Color.LightSlateGray;
        EditTeamButton.IsEnabled = true;
        EditTeamButton.BackgroundColor = Color.White;
        EditTeamButton.Text = "Create a Team";
      }
    }

    private async void EditTeamClicked(object sender, EventArgs e)
    {
      //Go to edit team page
      if (TeamPicker.ItemsSource != null && EditTeamButton.Text=="Edit Team")
      {
        if (ApplicationData.currentlySelectedTeam == null)
        {
          await Shell.Current.DisplayAlert("Team Editing", "Please Select a Team to edit", "Ok");
        }

      }
      await Shell.Current.GoToAsync("//EditTeamPage");

    }
  }
}