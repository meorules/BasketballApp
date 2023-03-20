using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

using BasketballApp.Models;
using BasketballApp.ViewModels;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;
using Xamarin.Essentials;

namespace BasketballApp.ViewModels
{

  public class GameObjectViewModel:BindableObject
  {
    //Team team = DependencyService.Get<Team>();
    //Hardcoded Team Passed in
    Team currentTeam = new Team
    {
      Id = Guid.NewGuid().ToString(),
      Name = "Chicago Bulls",
      Players = new List<Player>
          {
            new Player { Id = 1, Name = "Lonzo Ball", Position = "PG", PlayerNumber = 2 },
            new Player { Id = 2, Name = "Javonte Green", Position = "SG", PlayerNumber = 24 },
            new Player { Id = 3, Name = "Dalen Terry", Position = "SG", PlayerNumber = 25 },
            new Player { Id = 4, Name = "Andre Drummond", Position = "C", PlayerNumber = 3 },
            new Player { Id = 5, Name = "Derrick Jones Jr.", Position = "SF", PlayerNumber = 5 },
            new Player { Id = 6, Name = "DeMar DeRozan", Position = "SG", PlayerNumber = 11 },
            new Player { Id = 7, Name = "Patrick Williams", Position = "PF", PlayerNumber = 44 },
            new Player { Id = 8, Name = "Nikola Vucevic", Position = "C", PlayerNumber = 9 },
            new Player { Id = 9, Name = "Terry Taylor", Position = "PF", PlayerNumber = 32 },
            new Player { Id = 11, Name = "Zach LaVine", Position = "SG", PlayerNumber = 8 },
            new Player { Id = 12, Name = "Alex Caruso", Position = "PG", PlayerNumber = 6 },
            new Player { Id = 13, Name = "Patrick Beverley", Position = "PG", PlayerNumber = 21},
            new Player { Id = 14, Name = "Coby White", Position = "SG", PlayerNumber = 0 },
            new Player { Id = 15, Name = "Ayo Dosunmu", Position = "PG", PlayerNumber = 12 },

          },
      Games = new List<GameObject>
          {
            new GameObject
            {
              GameID = 1,
              Name = "Bulls vs Celtics",
              GameDate = DateTime.Now,
              GameLocation = "United Center",
              HomeScore = 100,
              AwayScore = 95,
              CurrentGameTime = new TimeSpan(0, 7, 30),
              CurrentQuarter = 4,
              LogActivities = new List<GameLogActivity>
              {
                new GameLogActivity
                {
                  Id = Guid.NewGuid().ToString(),
                  StatCollected = new Stat { StatName = "FGM", pointWorth = 2,gameTime =  new TimeSpan(0, 5, 26),Quarter = 1,shotClock = new TimeSpan(0, 0, 0,22,500)},
                  PlayerStat = new Player { Id = 1 },
                  positionX = 10,
                  positionY = 20
                },
                new GameLogActivity
                {
                  Id = Guid.NewGuid().ToString(),
                  StatCollected = new Stat { StatName = "FGM", pointWorth = 2,gameTime =  new TimeSpan(0, 5, 05),Quarter = 1,shotClock = new TimeSpan(0, 0, 0,16,250)},
                  PlayerStat = new Player { Id = 2 },
                  positionX = 10,
                  positionY = 20
                }
              }
            }
          }
    };

    int currentGameIndex;
    string[] playerNames;

    bool play=false;

    TimeSpan shotClockLength = new TimeSpan(0, 0, 12);
    TimeSpan quarterLength = new TimeSpan(0, 12, 0);

    public Command ChangeQuarter { get; }
    public Command StopGame { get; }

    public Command AddToAwayScore { get; }
    public Command RemoveFromAwayScore { get; }


    public GameObjectViewModel()
    {

      ChangeQuarter = new Command(updateQuarter);

      StopGame = new Command(endGame);

      AddToAwayScore = new Command(awayScoreAdd);

      RemoveFromAwayScore = new Command(awayScoreRemove);


      GameObject gameObject = new GameObject();
      gameObject.currentPlayers = new Player[5];

      playerNames = new string[5];  
      for (int i = 0; i < 5; i++)
      {
        playerNames[i] = currentTeam.Players[i].Name;
        gameObject.currentPlayers[i] = currentTeam.Players[i];
      }

      //Hardcoding the data for the current game
      gameObject.AwayScore = 0;
      gameObject.HomeScore = 0;
      gameObject.GameID = 1;
      gameObject.GameLocation = "The gym";
      gameObject.CurrentQuarter = 4;
      gameObject.CurrentGameTime= new TimeSpan(0,12,0);
      gameObject.CurrentShotClock = new TimeSpan(0, 0, 24);
      gameObject.LogActivities = new List<GameLogActivity>();
      gameObject.Name = "Game1";

      currentTeam.Games.Add(gameObject);
      currentGameIndex=currentTeam.Games.IndexOf(gameObject);


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


      foreach (GameLogActivity activity in currentTeam.Games[currentGameIndex].LogActivities)
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

    public void registerShot(float x, float y, string playerName, bool makeOrMiss, int pointWorth, TimeSpan gameClock, TimeSpan shotClock)
    {
      string statName = "FGA";
      if (makeOrMiss)
      {
        statName = "FGM";
        HomeScore = (currentTeam.Games[currentGameIndex].HomeScore + pointWorth).ToString();
      }
      Player current = new Player
      {
        Name = playerName
      };


      currentTeam.Games[currentGameIndex].LogActivities.Add(new GameLogActivity
      {
        Id = Guid.NewGuid().ToString(),
        StatCollected = new Stat { StatName = statName, pointWorth = pointWorth, Quarter = currentTeam.Games[currentGameIndex].CurrentQuarter, gameTime = gameClock, shotClock = shotClock },
        PlayerStat = currentTeam.Players[currentTeam.Players.IndexOf(current)],
        positionX = x,
        positionY = y
      });
    }
    void endGame(object obj)
    {
      //DO SOMETHING TO STOP THE GAME
    }


    public void awayScoreAdd(object obj)
    {
      AwayScore = (currentTeam.Games[currentGameIndex].AwayScore + 1).ToString();
    }

    public void awayScoreRemove(object obj)
    {
      AwayScore = (currentTeam.Games[currentGameIndex].AwayScore - 1).ToString();
    }

    public async void updateQuarter(object obj)
    {
      string quarter = await Shell.Current.DisplayActionSheet("Change Quarter", "Cancel", null, "1", "2","3","4");
      if (quarter != "Cancel")
      {
        QuarterField = "Q"+ quarter;
      }

    }

    public string QuarterField
    {
      get { return "Q"+currentTeam.Games[currentGameIndex].CurrentQuarter.ToString(); }
      set {
        if (value.Contains("Q")){
          value.Substring(1);
        }
        currentTeam.Games[currentGameIndex].CurrentQuarter = int.Parse(value);

        OnPropertyChanged("QuarterField");
      }
    }

    public string AwayScore
    {
      get { return currentTeam.Games[currentGameIndex].AwayScore.ToString(); }
      set
      {
        currentTeam.Games[currentGameIndex].AwayScore = int.Parse(value);

        OnPropertyChanged("AwayScore");
      }
    }

    public string HomeScore
    {
      get { return currentTeam.Games[currentGameIndex].HomeScore.ToString(); }
      set
      {
        currentTeam.Games[currentGameIndex].HomeScore = int.Parse(value);

        OnPropertyChanged("HomeScore");
      }
    }

    public string Player1
    {
      get { return playerNames[0]; }
    }
    public string Player2
    {
      get { return playerNames[1]; }
    }
    public string Player3
    {
      get { return playerNames[2]; }
    }
    public string Player4
    {
      get { return playerNames[3]; }
    }
    public string Player5
    {
      get { return playerNames[4]; }
    }




  }
}
