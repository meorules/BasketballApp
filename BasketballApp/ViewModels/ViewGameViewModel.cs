using BasketballApp.Models;
using BasketballApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BasketballApp.ViewModels
{
  public class ViewGameViewModel : BindableObject
  {

    GameObject currentGame;

    public ViewGameViewModel()
    {

      initialiseData();
    }

    public async void initialiseData()
    {
      if(ApplicationData.currentlySelectedGame == null)
      {
        await Shell.Current.DisplayAlert("Cannot View Game", "Please select a game to View", "OK");
        await Shell.Current.GoToAsync("//HomePage");
      }
      else
      {
        currentGame = BasketballDBService.getGame(ApplicationData.currentlySelectedGame);
        ApplicationData.currentlySelectedGame = currentGame;
      }
      OnPropertyChanged("gameName");
      OnPropertyChanged("gameDate");
      OnPropertyChanged("gameLocation");
      OnPropertyChanged("quarter");
      OnPropertyChanged("gameTime");
      OnPropertyChanged("score");
      OnPropertyChanged("shotClock");
      OnPropertyChanged("boxScore");
    }

    public List<GameLogActivity> returnShots(int type, TimeSpan start, TimeSpan end,int quarter=0)
    {
      string statNameToCheck = "";
      switch (type)
      {
        case 0:
          statNameToCheck = "FG";
          break;
        case 1:
          statNameToCheck = "FGM";
          break;
        case 2:
          statNameToCheck = "FGA";
          break;
      }
      List<GameLogActivity> shots = new List<GameLogActivity>();


      foreach (GameLogActivity activity in currentGame.LogActivities)
      {
        if (activity.StatCollected.StatName.Contains(statNameToCheck))
        {
          if(activity.StatCollected.gameTime > start && activity.StatCollected.gameTime < end)
          {
            if (quarter <= 0)
            {
              shots.Add(activity);
            }
            else
            {
              if(activity.StatCollected.Quarter ==  quarter)
              {
                shots.Add(activity);
              }
            }
          }

        }
        else
        {
        }
      }

      return shots;
    }

    public string gameName
    {
      get
      {
        if (currentGame != null)
        {
          return " "+ currentGame.Name;
        }
        else return "";
      }
    }

    public string gameDate
    {
      get
      {
        if (currentGame != null)
        {
          return currentGame.GameDateString;
        }
        else return "";
      }
    }

    public string gameLocation
    {
      get
      {
        if (currentGame != null)
        {
          return currentGame.GameLocation;
        }
        else return "";
      }
    }

    public string quarter
    {
      get
      {
        if (currentGame != null)
        {
          return currentGame.CurrentQuarterString;
        }
        else return "";
      }
    }

    public string gameTime
    {
      get
      {
        if (currentGame != null)
        {
          return currentGame.CurrentGameTimeString;
        }
        else return "";
      }
    }

    public string score
    {
      get
      {
        if (currentGame != null)
        {
          return currentGame.Score;
        }
        else return "";
      }
    }

    public string shotClock
    {
      get
      {
        if (currentGame != null)
        {
          return currentGame.CurrentShotClockString;
        }
        else return "";
      }
    }

    public BoxScore boxScore
    {
      get
      {
        if (currentGame != null)
        {
          BoxScore score = currentGame.getTotalBoxScore();
          if (score != null)
          {
            return score;
          }
          else
          {
            return null;
          }
        }
        else
        {
          return null;
        }
      }
    }

    public List<BoxScore> currentBoxScores
    {
      get
      {
        return currentGame.BoxScores;
      }
    }

  }
}
