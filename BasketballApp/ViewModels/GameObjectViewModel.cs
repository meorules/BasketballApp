using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

using BasketballApp.Models;
using BasketballApp.ViewModels;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;

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
                  StatCollected = new Stat { StatName = 1, pointWorth = 2 },
                  PlayerStat = new Player { Id = 1 },
                  Quarter = 1,
                  Time = new TimeSpan(0, 5, 0),
                  positionX = 10,
                  positionY = 20
                },
                new GameLogActivity
                {
                  Id = Guid.NewGuid().ToString(),
                  StatCollected = new Stat { StatName = 2, pointWorth = 3 },
                  PlayerStat = new Player { Id = 2 },
                  Quarter = 1,
                  Time = new TimeSpan(0, 5, 0),
                  positionX = 10,
                  positionY = 20
                }
              }
            }
          }
    };

    string[] playerNames;

    public GameObjectViewModel()
    {


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
      gameObject.LogActivities = new List<GameLogActivity>();
      gameObject.Name = "Game1";

    }

    

    public string Player1
    {
      get { return playerNames[1]; }
    }
    public string Player2
    {
      get { return playerNames[2]; }
    }
    public string Player3
    {
      get { return playerNames[3]; }
    }
    public string Player4
    {
      get { return playerNames[4]; }
    }
    public string Player5
    {
      get { return playerNames[5]; }
    }




  }
}
