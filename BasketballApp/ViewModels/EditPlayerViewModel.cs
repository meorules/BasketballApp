using BasketballApp.Models;
using BasketballApp.Services;
using BasketballApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BasketballApp.ViewModels
{
  class EditPlayerViewModel : BindableObject
  {
    Player currentPlayer;

    public Command SavePlayer { get; }


    public EditPlayerViewModel(){
      SavePlayer = new Command(savePlayer);

      initaliseData();


    }

    public async void initaliseData()
    {
      if (ApplicationData.currentlySelectedTeam != null) {
        if (ApplicationData.currentlySelectedPlayer == null)
        {
          //Create New Player
          currentPlayer = BasketballDBService.addPlayer(ApplicationData.currentlySelectedTeam.Name, "New Player", "PG", 0);
          ApplicationData.currentlySelectedPlayer = currentPlayer;
        }
        else
        {
          //Edit Current Player
          currentPlayer = ApplicationData.currentlySelectedPlayer;
        }
      }
      else
      {
        await Shell.Current.DisplayAlert("Incorrect Access", "Make sure to select a team to be able to add players", "OK");
        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
      }

      OnPropertyChanged("playerName");
      OnPropertyChanged("playerPosition");
      OnPropertyChanged("playerNumber");


    }

    public string playerName
    {
      get
      {
        if (currentPlayer == null)
        {
          return "";
        }
        else return currentPlayer.Name;
      }
      set
      {
        currentPlayer.Name = value;
        OnPropertyChanged("playerName");
      }
    }

    public string playerPosition
    {
      get
      {
        if (currentPlayer == null)
        {
          return "";
        }
        else return currentPlayer.Position;
      }
      set
      {
        currentPlayer.Position = value;
        OnPropertyChanged("playerPosition");
      }
    }

    public string playerNumber
    {
      get
      {
        if (currentPlayer == null)
        {
          return "0";
        }
        else return currentPlayer.PlayerNumber.ToString();
      }
      set
      {
        if (value == "")
        {
        }
        else
        {
          currentPlayer.PlayerNumber = int.Parse(value);
          OnPropertyChanged("playerNumber");
        }
      }
    }

    private async void savePlayer(object obj)
    {
      currentPlayer = BasketballDBService.updatePlayer(currentPlayer);
      ApplicationData.currentlySelectedPlayer = null;
      await Shell.Current.GoToAsync("//EditTeamPage");
    }

  }
}
