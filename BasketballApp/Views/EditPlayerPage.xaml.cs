using BasketballApp.Models;
using BasketballApp.Services;
using BasketballApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BasketballApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditPlayer : ContentPage
	{
		public EditPlayer ()
		{
      this.BindingContext = new EditPlayerViewModel();

      InitializeComponent();
      Title = "Create Or Edit Player";
      setUpTeamPicker();
    }

    protected override async void OnAppearing()
    {
      base.OnAppearing();
      var viewModel = (EditPlayerViewModel)BindingContext;

      if (ApplicationData.currentlySelectedPlayer == null)
      {
        Header.Text = "Create a Player";
      }
      else
      {
        Header.Text = "Edit Player: " + ApplicationData.currentlySelectedPlayer.Name;

      }
      viewModel.initaliseData();
      setUpTeamPicker();
    }

    private void setUpTeamPicker()
    {
      if (ApplicationData.currentlySignedInUser != null)
      {
        User currentUser = ApplicationData.currentlySignedInUser;
        if (currentUser.Teams != null)
        {
          String[] teamList = new string[currentUser.Teams.Count];
          for (int i = 0; i < currentUser.Teams.Count; i++)
          {
            if (currentUser.Teams[i].TeamID != ApplicationData.currentlySelectedTeam.TeamID)
            {
              teamList[i] = currentUser.Teams[i].Name;
            }
          }
          if (teamList.Length > 0 && teamList[0] != null)
          {
            TeamPicker.ItemsSource = teamList;
          }
          else
          {
            teamList[0] = "No Avaialble Teams";
            TeamPicker.ItemsSource = teamList;

          }
        }
        else
        {
          TeamPicker.ItemsSource = null;
        }
      }
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
          MovePlayerButton.Text = "Move player to " + teamName;
          MovePlayerButton.IsEnabled = true;
          MovePlayerButton.BackgroundColor = Color.White;
        }
        else
        {
          throw new Exception("Problem finding selected team");
        }
      }
      else
      {
        MovePlayerButton.Text = "Select a valid team to move a player";
        MovePlayerButton.IsEnabled = false;
        MovePlayerButton.BackgroundColor = Color.LightSlateGray;
      }
    }

    private async void BackToTeamClicked(object sender, EventArgs e)
    {
      await Shell.Current.GoToAsync("//EditTeamPage");
    }

    private async void DeletePlayerClicked(object sender, EventArgs e)
    {
      //Make delete function based on selected player, also add alert to inform users that this is a non-reversible action
      if (ApplicationData.currentlySelectedPlayer == null)
      {
        await Shell.Current.DisplayAlert("Cannot Delete Player", "Please select a player to delete", "OK");
      }
      else
      {
        bool result = await Shell.Current.DisplayAlert("Player Deletion", "Are you sure you want to delete " + ApplicationData.currentlySelectedPlayer.Name + ", this is a non-reversible action", "Delete", "Cancel");
        if (result)
        {
          //Delete Player
          bool deletePlayer = BasketballDBService.deletePlayer(ApplicationData.currentlySelectedPlayer);
          if (deletePlayer)
          {
            await Shell.Current.DisplayAlert("Player Deletion", "Player successfully deleted", "OK");
            await Shell.Current.GoToAsync("//EditTeamPage");
          }
          else
          {
            await Shell.Current.DisplayAlert("Player Deletion", "Player was not deleted correctly", "OK");

          }

        }
      }

    }
  }
}
