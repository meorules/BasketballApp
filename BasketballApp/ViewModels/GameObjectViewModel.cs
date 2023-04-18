using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

using BasketballApp.Models;
using BasketballApp.Services;
using BasketballApp.ViewModels;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;
using Xamarin.Essentials;
using System.Collections.ObjectModel;

namespace BasketballApp.ViewModels
{

  public class GameObjectViewModel : BindableObject
  {
    //Hardcoded Team Passed in
    Team currentTeam = null;
    GameObject currentGame = null;

    string[] playerNames;

    public Command ChangeQuarter { get; }

    public Command AddToAwayScore { get; }
    public Command RemoveFromAwayScore { get; }

    public Command Substitution { get; }    

    public GameObjectViewModel()
    {

      ChangeQuarter = new Command(updateQuarter);

      AddToAwayScore = new Command(awayScoreAdd);

      RemoveFromAwayScore = new Command(awayScoreRemove);

      Substitution = new Command(substitutePlayer);
      initaliseData();


    }

    public async void initaliseData()
    {
      if (ApplicationData.currentlySelectedTeam != null)
      {
        currentTeam = BasketballDBService.getTeam(ApplicationData.currentlySelectedTeam.Name);
      }
      else
      {
        await Shell.Current.DisplayAlert("Data Collection", "Cannot collect data with a team selected", "Ok");

        await Shell.Current.GoToAsync("//HomePage");
      }
      if (currentTeam == null)
      {
        throw new NotImplementedException("No Team available for currently signed in user");
      }
      if (ApplicationData.currentlySelectedGame != null)
      {
        currentGame = BasketballDBService.getGame(ApplicationData.currentlySelectedGame);
      }
      else
      {
        currentGame = BasketballDBService.addGame(currentTeam.Name, currentTeam.Location, DateTime.Today);
      }

      playerNames = new string[5];
      for (int i = 0; i < currentTeam.Players.Count; i++)
      {
        if (i < 5)
        {
          playerNames[i] = currentTeam.Players[i].Name;
        }
      }
    }


    //Type Represents Shot Types, 0 being all Shots, 1 Being only Makes and 2 being only Misses
    public List<GameLogActivity> returnShots(int type)
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
        shots.Add(activity);
      }

      for(int i =0; i<shots.Count;i++)
      {
        if (!shots[i].StatCollected.StatName.Contains(statNameToCheck))
        {
          shots[i] = null;
        }
      }
      return shots;
    }

    public void registerShot(float x, float y, string playerName, bool makeOrMiss, int pointWorth, TimeSpan gameClock, TimeSpan shotClock,string assister)
    {
      string statName = "FGA";
      if (makeOrMiss)
      {
        statName = "FGM";
        HomeScore = (currentGame.HomeScore + pointWorth).ToString();
      }
      Player current = new Player
      {
        Name = playerName
      };
      for (int j = 0; j < currentTeam.Players.Count; j++)
      {
        if (currentGame.BoxScores[j].player.Name == playerName)
        {
          currentGame.BoxScores[j].FieldGoalsAttempted+=1;
          if(statName == "FGA" && pointWorth == 3)
          {
            currentGame.BoxScores[j].ThreesAttempts += 1;
          }
          currentGame.BoxScores[j].FieldGoalsAttempted += 1;
          if (statName == "FGM" && pointWorth == 2)
          {
            currentGame.BoxScores[j].FieldGoalsMade += 1;
            currentGame.BoxScores[j].Points += 2;
          }
          else if(statName == "FGM" && pointWorth == 3)
          {
            currentGame.BoxScores[j].ThreesAttempts += 1;
            currentGame.BoxScores[j].FieldGoalsMade += 1;
            currentGame.BoxScores[j].ThreesMade += 1;
            currentGame.BoxScores[j].Points += 3;


          }
          BasketballDBService.updateBoxScore(currentGame.BoxScores[j]);

        }

      }
      GameLogActivity curr = new GameLogActivity
      {

        StatCollected = new Stat { StatName = statName, pointWorth = pointWorth, Quarter = currentGame.CurrentQuarter, gameTime = gameClock, shotClock = shotClock },
        Player = currentTeam.Players[currentTeam.Players.IndexOf(current)],
        positionX = x,
        positionY = y,
        GameObjectID = currentGame.GameID,
      };
      curr = BasketballDBService.addGameLog(curr);
      currentGame.LogActivities.Add(curr);

      if (assister != "No One")
      {
        for (int j = 0; j < currentTeam.Players.Count; j++)
        {
          if (currentGame.BoxScores[j].player.Name == assister)
          {
            currentGame.BoxScores[j].Assists += 1;
            BasketballDBService.updateBoxScore(currentGame.BoxScores[j]);
          }
        }
        Player assistPlayer = new Player
        {
          Name = assister
        };
        GameLogActivity assistLog = new GameLogActivity
        {

          StatCollected = new Stat { StatName = "Assist", pointWorth = pointWorth, Quarter = currentGame.CurrentQuarter, gameTime = gameClock, shotClock = shotClock },
          Player = currentTeam.Players[currentTeam.Players.IndexOf(assistPlayer)],
          positionX = 0,
          positionY = 0
        };
        assistLog = BasketballDBService.addGameLog(assistLog);
        currentGame.LogActivities.Add(assistLog);
      }
      updateGame();

    }
    public async void endGame(TimeSpan gameClock, TimeSpan shotClock)
    {

      currentGame.CurrentGameTime = gameClock;
      currentGame.CurrentShotClock = shotClock;
      //Save Game
      updateGame();

      //Set Currently Selected Game to null
      ApplicationData.currentlySelectedGame = null;
      //Go to Homepage
      await Shell.Current.GoToAsync("//HomePage");

    }


    private void awayScoreAdd(object obj)
    {
      AwayScore = (currentGame.AwayScore + 1).ToString();
    }

    private void awayScoreRemove(object obj)
    {
      AwayScore = (currentGame.AwayScore - 1).ToString();
    }

    private async void updateQuarter(object obj)
    {
      string quarter = await Shell.Current.DisplayActionSheet("Change Quarter", "Cancel", null, "1", "2","3","4");
      if (quarter != "Cancel" && quarter != null)
      {
        QuarterField = "Q"+ quarter;
      }

    }


    public string QuarterField
    {
      get { return "Q"+currentGame.CurrentQuarter.ToString(); }
      set {
        if (value.Contains("Q")){
          value  = value.Substring(1);
        }
        currentGame.CurrentQuarter = int.Parse(value);
        updateGame();
        OnPropertyChanged("QuarterField");
      }
    }

    public string AwayScore
    {
      get { return currentGame.AwayScore.ToString(); }
      set
      {
        currentGame.AwayScore = int.Parse(value);
        updateGame();
        OnPropertyChanged("AwayScore");
      }
    }

    public string HomeScore
    {
      get { return currentGame.HomeScore.ToString(); }
      set
      {
        currentGame.HomeScore = int.Parse(value);
        updateGame();
        OnPropertyChanged("HomeScore");
      }
    }

    public async void substitutePlayer(object obj)
    {
      if (currentTeam.Players.Count - 5 != 0)
      {
        string[] availableSubs = new string[currentTeam.Players.Count - 5];

        for (int i = 5; i < currentTeam.Players.Count; i++)
        {
          availableSubs[i - 5] = currentTeam.Players[i].Name;
        }

        string playerToSubIn = await Shell.Current.DisplayActionSheet("Pick a Player To Sub In", "Cancel", null, availableSubs);

        if (playerToSubIn != null && playerToSubIn != "Cancel")
        {
          string playerToSubOut = await Shell.Current.DisplayActionSheet("Pick a Player To Sub Out", "Cancel", null, Player1, Player2, Player3, Player4, Player5);
          if (playerToSubOut != null && playerToSubOut != "Cancel")
          {
            //Make Substitution
            //Start by grabbig the old player's name and storing it in a temp box
            for (int j = 0; j < 5; j++)
            {
              if (playerNames[j] == playerToSubOut)
              {
                switch (j)
                {
                  case 0:
                    Player1 = playerToSubIn;
                    break;
                  case 1:
                    Player2 = playerToSubIn;
                    break;
                  case 2:
                    Player3 = playerToSubIn;
                    break;
                  case 3:
                    Player4 = playerToSubIn;
                    break;
                  case 4:
                    Player5 = playerToSubIn;
                    break;
                  default:
                    //Something bad broke
                    break;
                }
              }
            }
            int playerToSubInIndex = -1;
            int playerToSubOutIndex = -1;
            for (int k = 0; k < currentTeam.Players.Count; k++)
            {
              if (currentTeam.Players[k].Name == playerToSubOut)
              {
                playerToSubOutIndex = k;
              }
              if (currentTeam.Players[k].Name == playerToSubIn)
              {
                playerToSubInIndex = k;
              }
            }
            if (playerToSubInIndex != -1 && playerToSubInIndex != -1)
            {
              Player tempPlayer = currentTeam.Players[playerToSubOutIndex];
              currentTeam.Players[playerToSubOutIndex] = currentTeam.Players[playerToSubInIndex];
              currentTeam.Players[playerToSubInIndex] = tempPlayer;
            }
          }

        }

      }
      else
      {
        await Shell.Current.DisplayAlert("Team Substitutions", "No Available Substitutions", "Ok");
      }
    }

    public TimeSpan getGameClock()
    {
      return currentGame.CurrentGameTime;
    }

    public TimeSpan getGameShotClock()
    {
      return currentGame.CurrentShotClock;
    }

    //statType Cases
    //0 = STL, 1 = TO, 2 = FOUL, 3 = OREB, 4 = DREB,5 = FT, 6 = BLOCK, 7 = ASST
    public async void addStat(int statType,TimeSpan gameClock,TimeSpan shotClock)
    {
      string playerName = await Shell.Current.DisplayActionSheet("Pick a Player", "Cancel", null, Player1, Player2, Player3, Player4, Player5);
      if (playerName != "Cancel" && playerName != null)
      {
        Player currentPlayer = new Player
        {
          Name = playerName
        };
        string stat = "";
        for (int j = 0; j < currentTeam.Players.Count; j++)
        {
          if (currentGame.BoxScores[j].player.Name == playerName)
          {
            switch (statType)
            {
              case 0:
                currentGame.BoxScores[j].Steals += 1;
                stat = "Steal";
                break;
              case 1:
                currentGame.BoxScores[j].TurnOvers += 1;
                stat = "Turnover";
                break;
              case 2:
                currentGame.BoxScores[j].PersonalFouls += 1;
                stat = "Foul";

                break;
              case 3:
                currentGame.BoxScores[j].ORebs += 1;
                stat = "OReb";

                break;
              case 4:
                currentGame.BoxScores[j].DRebs += 1;
                stat = "DReb";

                break;
              case 5:
                stat = "FT";
                string makeOrMiss = await Shell.Current.DisplayActionSheet("Made or Missed?", "Cancel", null, "Made", "Missed");
                if(makeOrMiss != null && makeOrMiss !="Cancel")
                {
                  if (makeOrMiss == "Made")
                  {
                    currentGame.BoxScores[j].FreeThrowMakes += 1;
                    currentGame.BoxScores[j].Points += 1;
                    currentGame.BoxScores[j].FreeThrowAttempts += 1;
                    GameLogActivity curr = new GameLogActivity
                    {

                      StatCollected = new Stat { StatName = "FTM", pointWorth = 1, Quarter = currentGame.CurrentQuarter, gameTime = gameClock, shotClock = shotClock },
                      Player = currentTeam.Players[currentTeam.Players.IndexOf(currentPlayer)],
                      positionX = 0,
                      positionY = 0
                    };
                    curr = BasketballDBService.addGameLog(curr);
                    currentGame.LogActivities.Add(curr);
                  }
                  else
                  {
                    currentGame.BoxScores[j].FreeThrowAttempts += 1;
                    GameLogActivity curr = new GameLogActivity
                    {

                      StatCollected = new Stat { StatName = "FTA", pointWorth = 1, Quarter = currentGame.CurrentQuarter, gameTime = gameClock, shotClock = shotClock },
                      Player = currentTeam.Players[currentTeam.Players.IndexOf(currentPlayer)],
                      positionX = 0,
                      positionY = 0
                    };
                    curr = BasketballDBService.addGameLog(curr);
                    currentGame.LogActivities.Add(curr);
                  }

                }
                break;
              case 6:
                currentGame.BoxScores[j].Blocks += 1;
                stat = "Block";
                break;
              case 7:
                currentGame.BoxScores[j].Assists += 1;
                stat = "Assist";
                break;
            }
            if (stat != "FT")
            {
              GameLogActivity curr = new GameLogActivity
              {

                StatCollected = new Stat { StatName = stat, Quarter = currentGame.CurrentQuarter, gameTime = gameClock, shotClock = shotClock },
                Player = currentTeam.Players[currentTeam.Players.IndexOf(currentPlayer)],
                positionX = 0,
                positionY = 0
              };
              curr = BasketballDBService.addGameLog(curr);
              currentGame.LogActivities.Add(curr);
            }
            BasketballDBService.updateBoxScore(currentGame.BoxScores[j]);
          }
        }
      }
      updateGame();
    }
    public void AddMinutes(TimeSpan timeSpan)
    {
      //Get all the five current starters, and then add to their minutes values
      for(int i=0;i<playerNames.Length;i++)
      {
        for (int j = 0; j < currentTeam.Players.Count;j++)
        {
          if (currentGame.BoxScores[j].player.Name== playerNames[i])
          {
            currentGame.BoxScores[j].minutesOnCourt += timeSpan;
            BasketballDBService.updateBoxScore(currentGame.BoxScores[j]);
          }
        }
      }
      updateGame();
    }

    public string Player1
    {
      get { return playerNames[0]; }
      set { playerNames[0] = value;
        OnPropertyChanged("Player1");

      }
    }
    public string Player2
    {
      get { return playerNames[1]; }
      set
      {
        playerNames[1] = value;
        OnPropertyChanged("Player2");

      }
    }
    public string Player3
    {
      get { return playerNames[2]; }
      set
      {
        playerNames[2] = value;
        OnPropertyChanged("Player3");
      }
    }
    public string Player4
    {
      get { return playerNames[3]; }
      set
      {
        playerNames[3] = value;
        OnPropertyChanged("Player4");
      }
    }
    public string Player5
    {
      get { return playerNames[4]; }
      set
      {
        playerNames[4] = value;
        OnPropertyChanged("Player5");
      } 
    }

    public List<BoxScore> currentBoxScores
    {
      get
      {
        return currentGame.BoxScores;
      }
    }


    private void updateGame()
    {
      currentGame = BasketballDBService.updateGame(currentGame);
    }

  }
}

