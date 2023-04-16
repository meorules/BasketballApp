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

    public EditTeamViewModel() {

      initialiseData();

    }

    public void initialiseData()
    {
      if (ApplicationData.currentlySelectedTeam == null)
      {
        //Create New Team
        currentTeam = BasketballDBService.addTeam(ApplicationData.currentlySignedInUser.Name, "", "");
        BasketballDBService.getTeam(currentTeam.TeamID);
        ApplicationData.currentlySelectedTeam = currentTeam;
      }
      else
      {
        //Edit Team
        currentTeam = ApplicationData.currentlySelectedTeam;
      }
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
        updateTeam();
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
        updateTeam();
        OnPropertyChanged("teamName");
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
              playerNameList[i] = currentTeam.Players[i].Name + " " + currentTeam.Players[i].Position;
            }
            return playerNameList;
          }
          else return null;
        }
        else return null;
      }
    }

    private void updateTeam()
    {
      BasketballDBService.updateTeam(currentTeam);
    }


  }
}
