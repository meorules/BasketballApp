using BasketballApp.Models;
using BasketballApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BasketballApp.ViewModels
{
  public class EditTeamViewModel:BindableObject
  {
    Team currentTeam;

    public Command SaveTeam { get; }
    public Command DeleteTeam { get; }


    public EditTeamViewModel() {
      SaveTeam = new Command(updateTeam);
      DeleteTeam = new Command(deleteTeam);
      initialiseData();

    }

    public void initialiseData()
    {
      if (ApplicationData.currentlySelectedTeam == null)
      {
        //Create New Team
        currentTeam = BasketballDBService.addTeam(ApplicationData.currentlySignedInUser.Name,"Team 1","Team Location");
        BasketballDBService.getTeam(currentTeam.TeamID);
        ApplicationData.currentlySelectedTeam = currentTeam;
      }
      else
      {
        //Edit Team
        currentTeam = BasketballDBService.getTeam(ApplicationData.currentlySelectedTeam.TeamID);

      }
      OnPropertyChanged("teamName");
      OnPropertyChanged("teamLocation");
      OnPropertyChanged("playerList");
    }

    public string teamName
    {
      get { 
        if(currentTeam == null)
        {
          return "";
        }
        else return currentTeam.Name;
      }
      set
      {
        currentTeam.Name = value;
        OnPropertyChanged("teamName");
      }
    }

    public string teamLocation
    {
      get
      {
        if (currentTeam == null)
        {
          return "";
        }
        else return currentTeam.Location;
      }
      set
      {
        currentTeam.Location = value;
        OnPropertyChanged("teamLocation");
      }
    }

    public string[] playerList
    {
      get {
        if (currentTeam != null)
        {
          if (currentTeam.Players != null)
          {
            string[] playerNameList = new string[currentTeam.Players.Count];
            for (int i = 0; i < currentTeam.Players.Count; i++)
            {
              playerNameList[i] = currentTeam.Players[i].Name + " | " + currentTeam.Players[i].Position;
            }
            return playerNameList;
          }
          else return null;
        }
        else return null;
      }
    }

    private async void updateTeam()
    {
      currentTeam = BasketballDBService.updateTeam(currentTeam);
      if(currentTeam == null)
      {
        await Shell.Current.DisplayAlert("Team Save", "Team did not save succesfully", "Ok");
      }
      else
      {
        await Shell.Current.DisplayAlert("Team Save", "Team saved succesfully", "Ok");
      }
    }

    private async void deleteTeam()
    {
      bool result = await Shell.Current.DisplayAlert("Team Deletion", "Are you sure you want to delete " + ApplicationData.currentlySelectedTeam.Name + ", this is a non-reversible action", "Delete", "Cancel");
      if (result)
      {
         bool doubleResult = await Shell.Current.DisplayAlert("Team Deletion", "Are you sure? All team data will be deleted!!", "Delete", "Cancel");
        if (doubleResult)
        {
          bool deleteTeam = BasketballDBService.deleteTeam(currentTeam);
          if (deleteTeam)
          {
            await Shell.Current.DisplayAlert("Team Deletion", "Team deleted succesfully", "Ok");
            currentTeam = null;
            ApplicationData.currentlySelectedTeam = null;
            await Shell.Current.GoToAsync("//HomePage");
          }
          else
          {
            await Shell.Current.DisplayAlert("Team Deletion", "Team not deleted succesfully", "Ok");
          }
        }
      }
    }


  }
}
