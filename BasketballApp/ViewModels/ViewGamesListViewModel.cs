using System;
using System.Collections.Generic;
using System.Text;
using BasketballApp.Models;
using BasketballApp.Services;
using Xamarin.Forms;

namespace BasketballApp.ViewModels
{
  public class ViewGamesListViewModel : BindableObject
  {
    Team currentTeam;
    public ViewGamesListViewModel()
    {
      initialiseData();
    }
    public async void initialiseData()
    {
      if(ApplicationData.currentlySelectedTeam == null)
      {
        await Shell.Current.DisplayAlert("Games List Error", "Please select a team to view their games", "OK");
        await Shell.Current.GoToAsync("//HomePage");
      }
      else
      {
        currentTeam = ApplicationData.currentlySelectedTeam;

      }
      OnPropertyChanged("gameObjects");
    }

    public List<GameObject> gameObjects
    {
      get
      {
        if (currentTeam != null)
        {
          if(currentTeam.Games!= null)
          {
            return currentTeam.Games;
          }
          else
          {
            currentTeam.Games = new List<GameObject>();
            return currentTeam.Games;
          }
        }
        return null;
      }
    }
  }
}
