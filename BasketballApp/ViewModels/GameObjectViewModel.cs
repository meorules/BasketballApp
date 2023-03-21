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

    public Command Substitution { get; }
    

    public GameObjectViewModel()
    {

      ChangeQuarter = new Command(updateQuarter);

      StopGame = new Command(endGame);

      AddToAwayScore = new Command(awayScoreAdd);

      RemoveFromAwayScore = new Command(awayScoreRemove);

      Substitution = new Command(substitutePlayer);
     
      GameObject gameObject = new GameObject();





      gameObject.currentPlayers = new Player[5];
      gameObject.BoxScores = new List<BoxScore>();


      playerNames = new string[5];  
      for (int i = 0; i < currentTeam.Players.Count; i++)
      {
        if (i < 5)
        {
          playerNames[i] = currentTeam.Players[i].Name;
          gameObject.currentPlayers[i] = currentTeam.Players[i];
        }
        gameObject.BoxScores.Add(new BoxScore
        {
           Id = Guid.NewGuid().ToString(),
           player = currentTeam.Players[i],
           minutesOnCourt = TimeSpan.Zero,
           FieldGoalsAttempted = 0,
           FieldGoalsMade = 0,
           ThreesAttempts = 0,
           ThreesMade = 0,
           FreeThrowAttempts = 0,
           FreeThrowMakes = 0,
           ORebs = 0,
           DRebs = 0,
           Assists = 0,
           Steals = 0,
           Blocks = 0,
           TurnOvers = 0,
           PersonalFouls = 0,
           PlusMinus = 0,
           Points =0

        });
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

    public void registerShot(float x, float y, string playerName, bool makeOrMiss, int pointWorth, TimeSpan gameClock, TimeSpan shotClock,string assister)
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
      for (int j = 0; j < currentTeam.Players.Count; j++)
      {
        if (currentTeam.Games[currentGameIndex].BoxScores[j].player.Name == playerName)
        {
          currentTeam.Games[currentGameIndex].BoxScores[j].FieldGoalsAttempted+=1;
          if(statName == "FGA" && pointWorth == 3)
          {
            currentTeam.Games[currentGameIndex].BoxScores[j].ThreesAttempts += 1;
          }
          currentTeam.Games[currentGameIndex].BoxScores[j].FieldGoalsAttempted += 1;
          if (statName == "FGM" && pointWorth == 2)
          {
            currentTeam.Games[currentGameIndex].BoxScores[j].FieldGoalsMade += 1;
            currentTeam.Games[currentGameIndex].BoxScores[j].Points += 2;
          }
          else if(statName == "FGM" && pointWorth == 3)
          {
            currentTeam.Games[currentGameIndex].BoxScores[j].ThreesAttempts += 1;
            currentTeam.Games[currentGameIndex].BoxScores[j].FieldGoalsMade += 1;
            currentTeam.Games[currentGameIndex].BoxScores[j].ThreesMade += 1;
            currentTeam.Games[currentGameIndex].BoxScores[j].Points += 3;


          }
        }

      }
      currentTeam.Games[currentGameIndex].LogActivities.Add(new GameLogActivity
      {
        Id = Guid.NewGuid().ToString(),
        StatCollected = new Stat { StatName = statName, pointWorth = pointWorth, Quarter = currentTeam.Games[currentGameIndex].CurrentQuarter, gameTime = gameClock, shotClock = shotClock },
        PlayerStat = currentTeam.Players[currentTeam.Players.IndexOf(current)],
        positionX = x,
        positionY = y
      });

      if (assister != "No One")
      {
        for (int j = 0; j < currentTeam.Players.Count; j++)
        {
          if (currentTeam.Games[currentGameIndex].BoxScores[j].player.Name == assister)
          {
            currentTeam.Games[currentGameIndex].BoxScores[j].Assists += 1;
          }
        }
        Player assistPlayer = new Player
        {
          Name = assister
        };
        currentTeam.Games[currentGameIndex].LogActivities.Add(new GameLogActivity
        {
          Id = Guid.NewGuid().ToString(),
          StatCollected = new Stat { StatName = "Assist", pointWorth = pointWorth, Quarter = currentTeam.Games[currentGameIndex].CurrentQuarter, gameTime = gameClock, shotClock = shotClock },
          PlayerStat = currentTeam.Players[currentTeam.Players.IndexOf(assistPlayer)],
          positionX = 0,
          positionY = 0
        });
      }


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
          value  = value.Substring(1);
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

    public async void substitutePlayer(object obj)
    {

      string[] availableSubs = new string[currentTeam.Players.Count-5];

      for(int i=5;i< currentTeam.Players.Count; i++)
      {
        availableSubs[i - 5] = currentTeam.Players[i].Name;
      }

      string playerToSubIn = await Shell.Current.DisplayActionSheet("Pick a Player To Sub In", "Cancel", null, availableSubs);

      if(playerToSubIn!=null && playerToSubIn != "Cancel")
      {
        string playerToSubOut = await Shell.Current.DisplayActionSheet("Pick a Player To Sub Out", "Cancel", null, Player1, Player2, Player3, Player4, Player5);
        if (playerToSubOut != null && playerToSubOut != "Cancel")
        {
          //Make Substitution
          //Start by grabbig the old player's name and storing it in a temp box
          for(int j = 0; j < 5; j++)
          {
            if(playerNames[j]== playerToSubOut)
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
          int playerToSubInIndex = -1 ;
          int playerToSubOutIndex=-1;
          for (int k = 0; k < currentTeam.Players.Count; k++)
          {
            if(currentTeam.Players[k].Name == playerToSubOut)
            {
              playerToSubOutIndex =k;
            }
            if(currentTeam.Players[k].Name == playerToSubIn)
            {
              playerToSubInIndex =k;
            }
          }
          if(playerToSubInIndex != -1 && playerToSubInIndex != -1)
          {
            Player tempPlayer = currentTeam.Players[playerToSubOutIndex];
            currentTeam.Players[playerToSubOutIndex] = currentTeam.Players[playerToSubInIndex];
            currentTeam.Players[playerToSubInIndex] = tempPlayer;
          }
        }

      }


    }

    //statType Cases
    //0 = STL, 1 = TO, 2 = FOUL, 3 = OREB, 4 = DREB,5 = FT
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
          if (currentTeam.Games[currentGameIndex].BoxScores[j].player.Name == playerName)
          {
            switch (statType)
            {
              case 0:
                currentTeam.Games[currentGameIndex].BoxScores[j].Steals += 1;
                stat = "Steal";
                break;
              case 1:
                currentTeam.Games[currentGameIndex].BoxScores[j].TurnOvers += 1;
                stat = "Turnover";
                break;
              case 2:
                currentTeam.Games[currentGameIndex].BoxScores[j].PersonalFouls += 1;
                stat = "Foul";

                break;
              case 3:
                currentTeam.Games[currentGameIndex].BoxScores[j].ORebs += 1;
                stat = "OReb";

                break;
              case 4:
                currentTeam.Games[currentGameIndex].BoxScores[j].DRebs += 1;
                stat = "DReb";

                break;
              case 5:
                stat = "FT";
                string makeOrMiss = await Shell.Current.DisplayActionSheet("Made or Missed?", "Cancel", null, "Made", "Missed");
                if(makeOrMiss != null && makeOrMiss !="Cancel")
                {
                  if (makeOrMiss == "Made")
                  {
                    currentTeam.Games[currentGameIndex].BoxScores[j].FreeThrowMakes += 1;
                    currentTeam.Games[currentGameIndex].BoxScores[j].Points += 1;
                    currentTeam.Games[currentGameIndex].BoxScores[j].FreeThrowAttempts += 1;
                    currentTeam.Games[currentGameIndex].LogActivities.Add(new GameLogActivity
                    {
                      Id = Guid.NewGuid().ToString(),
                      StatCollected = new Stat { StatName = "FTM",pointWorth=1, Quarter = currentTeam.Games[currentGameIndex].CurrentQuarter, gameTime = gameClock, shotClock = shotClock },
                      PlayerStat = currentTeam.Players[currentTeam.Players.IndexOf(currentPlayer)],
                      positionX = 0,
                      positionY = 0
                    });
                  }
                  else
                  {
                    currentTeam.Games[currentGameIndex].BoxScores[j].FreeThrowAttempts += 1;
                    currentTeam.Games[currentGameIndex].LogActivities.Add(new GameLogActivity
                    {
                      Id = Guid.NewGuid().ToString(),
                      StatCollected = new Stat { StatName = "FTA", pointWorth = 1, Quarter = currentTeam.Games[currentGameIndex].CurrentQuarter, gameTime = gameClock, shotClock = shotClock },
                      PlayerStat = currentTeam.Players[currentTeam.Players.IndexOf(currentPlayer)],
                      positionX = 0,
                      positionY = 0
                    });
                  }

                }
                break;
            }
            if (stat != "FT")
            {
              currentTeam.Games[currentGameIndex].LogActivities.Add(new GameLogActivity
              {
                Id = Guid.NewGuid().ToString(),
                StatCollected = new Stat { StatName = stat, Quarter = currentTeam.Games[currentGameIndex].CurrentQuarter, gameTime = gameClock, shotClock = shotClock },
                PlayerStat = currentTeam.Players[currentTeam.Players.IndexOf(currentPlayer)],
                positionX = 0,
                positionY = 0
              });
            }
          }
        }
      }
    }
    public void AddMinutes(TimeSpan timeSpan)
    {
      //Get all the five current starters, and then add to their minutes values
      for(int i=0;i<playerNames.Length;i++)
      {
        for (int j = 0; j < currentTeam.Players.Count;j++)
        {
          if (currentTeam.Games[currentGameIndex].BoxScores[j].player.Name== playerNames[i])
          {
            currentTeam.Games[currentGameIndex].BoxScores[j].minutesOnCourt += timeSpan;
          }
        }
      }
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
        return currentTeam.Games[currentGameIndex].BoxScores;
      }
    }


  }
}
