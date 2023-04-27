using BasketballApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BasketballApp.Services;
using BasketballApp.Models;
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
      var viewModel = (EditTeamViewModel)BindingContext;
      if (ApplicationData.currentlySelectedTeam == null)
      {
        Header.Text = "Build a Team";
      }
      else
      {
        Header.Text = "Edit Team: " + ApplicationData.currentlySelectedTeam.Name;

      }
      viewModel.initialiseData();
      playerListView.SelectedItem = null;
    }

    private void playerListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      string playerText = playerListView.SelectedItem.ToString();
      string playerName = playerText.Split('|')[0];
      playerName = playerName.Trim();
      Player currentPlayer = BasketballDBService.getPlayer(playerName);
      if (currentPlayer != null)
      {
        ApplicationData.currentlySelectedPlayer = currentPlayer;
      }

    }

    private async void PlayerAddClicked(object sender, EventArgs e)
    {
      ApplicationData.currentlySelectedPlayer = null;
      await Shell.Current.GoToAsync("//EditPlayerPage");
    }

    private async void PlayerEditClicked(object sender, EventArgs e)
    {
      if(ApplicationData.currentlySelectedPlayer == null)
      {
        await Shell.Current.DisplayAlert("Cannot Edit Player", "Please select a player to edit", "OK");
        await Shell.Current.GoToAsync("//EditPlayerPage");
      }
    }

    private async void DeletePlayerClicked(object sender, EventArgs e)
    {
      //Make delete function based on selected player, also add alert to inform users that this is a non-reversible action
      if (ApplicationData.currentlySelectedPlayer == null)
      {
        await Shell.Current.DisplayAlert("Cannot Delete Player", "Please select a player to delete", "OK");
      }
      else{
        bool result = await Shell.Current.DisplayAlert("Player Deletion", "Are you sure you want to delete "+ ApplicationData.currentlySelectedPlayer.Name + ", this is a non-reversible action","Delete","Cancel");
        if (result)
        {
          //Delete Player
          bool deletePlayer = BasketballDBService.deletePlayer(ApplicationData.currentlySelectedPlayer);
          if (deletePlayer)
          {
            await Shell.Current.DisplayAlert("Player Deletion", "Player successfully deleted", "OK");
            var viewModel = (EditTeamViewModel)BindingContext;
            viewModel.initialiseData();
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