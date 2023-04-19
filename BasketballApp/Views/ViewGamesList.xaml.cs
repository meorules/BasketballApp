using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using BasketballApp.Models;
using BasketballApp.ViewModels;
using BasketballApp.Services;

namespace BasketballApp.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class ViewGamesList : ContentPage
  {
    public ViewGamesList()
    {
      InitializeComponent();
      Title = "Browse Collected Data";

      this.BindingContext = new ViewGamesListViewModel();
    }

    protected override async void OnAppearing()
    {
      base.OnAppearing();
      var viewModel = (ViewGamesListViewModel)BindingContext;
      if (ApplicationData.currentlySelectedTeam == null)
      {
        await Shell.Current.DisplayAlert("Games List Error", "Please select a team to view their games", "OK");
        await Shell.Current.GoToAsync("//HomePage");
      }
      else
      {
        Header.Text = "View Games of Team :" + ApplicationData.currentlySelectedTeam.Name;
        viewModel.initialiseData();
        enableButtons(false);
      }
    }

    private void gameSelected(object sender, EventArgs e)
    {
      if(gamesListView.SelectedItem != null)
      {
        ApplicationData.currentlySelectedGame = (GameObject)gamesListView.SelectedItem;
        enableButtons(true);
      }
      else
      {
        enableButtons(false);
      }

    }

    private void enableButtons(bool enable)
    {
      if (enable)
      {
        EditDataButton.IsEnabled = true;
        DeleteGameButton.IsEnabled = true;
        ViewDataButton.IsEnabled = true;
        EditDataButton.BackgroundColor = Color.White;
        DeleteGameButton.BackgroundColor = Color.Red;
        ViewDataButton.BackgroundColor = Color.White;

      }
      else
      {
        EditDataButton.IsEnabled = false;
        DeleteGameButton.IsEnabled = false;
        ViewDataButton.IsEnabled = false;
        EditDataButton.BackgroundColor = Color.LightSlateGray;
        DeleteGameButton.BackgroundColor = Color.LightSlateGray;
        ViewDataButton.BackgroundColor = Color.LightSlateGray;
      }
    }

    private async void CreateGameClicked(object sender, EventArgs e)
    {
      ApplicationData.currentlySelectedGame = null;
      await Shell.Current.GoToAsync("//DataCollectionPage");
    }

    private async void EditGameClicked(object sender, EventArgs e)
    {
      await Shell.Current.GoToAsync("//DataCollectionPage");
    }

    private async void ViewDataClicked(object sender, EventArgs e)
    {
      //Go to View Data Page which will be implemented later
    }

    private async void DeleteGameClicked(object sender, EventArgs e)
    {
      if (ApplicationData.currentlySelectedGame == null)
      {
        await Shell.Current.DisplayAlert("Cannot Delete Game", "Please select a player to game", "OK");
      }
      else
      {
        bool result = await Shell.Current.DisplayAlert("Game Deletion", "Are you sure you want to delete this game? This is a non-reversible action!", "Delete", "Cancel");
        if (result)
        {
          //Delete Player
          bool deleteGame = BasketballDBService.deleteGame(ApplicationData.currentlySelectedGame);
          if (deleteGame)
          {
            await Shell.Current.DisplayAlert("Game Deletion", "Game successfully deleted", "OK");
            var viewModel = (ViewGamesListViewModel)BindingContext;
            viewModel.initialiseData();
          }
          else
          {
            await Shell.Current.DisplayAlert("Game Deletion", "Game was not deleted correctly", "OK");

          }

        }
      }
    }

  }
}