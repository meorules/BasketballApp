using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SQLite;

using BasketballApp.Models;

namespace BasketballApp.Services
{
  public static class BasketballDBService
  {
    static SQLiteConnection conn;
    static User currentUser = null;

    public static void initialise() {

      if(conn != null)
      {
        return;
      }
      string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyData.db");

      conn = new SQLiteConnection(databasePath);
      deleteTables();
      setupTables();
    }

    private static void deleteTables()
    {
      conn.DropTable<User>();
      conn.DropTable<Team>();
      conn.DropTable<Player>();
      conn.DropTable<GameObject>();
      conn.DropTable<Stat>();
      conn.DropTable<GameLogActivity>();
      conn.DropTable<BoxScore>();
    }

    private static void setupTables()
    {
      conn.CreateTable<User>();
      conn.CreateTable<Team>();
      conn.CreateTable<Player>();
      conn.CreateTable<GameObject>();
      conn.CreateTable<Stat>();
      conn.CreateTable<GameLogActivity>();
      conn.CreateTable<BoxScore>();
    }

    public static void addUser(string username,string email,string password)
    {
      User user = new User()
      {
        Name = username,
        Email = email,
        Password = password,
        Teams = new List<Team>()
      };

      conn.Insert(user);
    }

    public static User getUser(string username)
    {
      var UserQuery = conn.Table<User>().Where(User => User.Name == username);
      if (UserQuery.Count() > 0)
      {
        var queryList = UserQuery.ToList();
        var TeamsQuery = conn.Table<Team>().Where(Team => Team.UserID == queryList[0].UserID);
        queryList[0].Teams = TeamsQuery.ToList();

        return queryList[0];
      }
      else
      {
        throw new Exception("User Not Found in DB");
      }

    }

    public static string checkUser(string username,string password)
    {
      var query = conn.Table<User>().Where(User => User.Name == username);
      if(query.Count() > 0 )
      {
        var queryList = query.ToList();
        if(queryList[0].Name == username && queryList[0].Password == password)
        {
          currentUser = queryList[0];

          return "Successful Sign In";
        }
        else
        {
          return "Incorrect Password";
        }
      }
      else
      {
        return "Username Not Found";
      }
    }

    public static void deleteUser(string username)
    {
      User user = getUser(username);
      conn.Delete(user);
    }

    public static Team addTeam(string username,string teamName)
    {
      User user = getUser(username);
      if (user != null) {
        if(user.Teams == null)
        {
          user.Teams = new List<Team>();
        }
        Team team = new Team()
        {
          Name = teamName,
          UserID = user.UserID,
          Games = new List<GameObject>(),
          Players = new List<Player>()
        };
        conn.Insert(team);
        user.Teams.Add(team);
        conn.Update(user);
        return team;
      }

      throw new Exception("Team not added succesfully");
    }

    public static Team getTeam(string teamName)
    {
      var TeamQuery = conn.Table<Team>().Where(Team => Team.Name == teamName);
      if (TeamQuery.Count() > 0)
      {
        var TeamQueryItem = TeamQuery.ToList()[0];
        var PlayersQuery = conn.Table<Player>().Where(Player => Player.TeamId == TeamQueryItem.TeamID).ToList();
        var GameQuery = conn.Table<GameObject>().Where(GameObject => GameObject.TeamID == TeamQueryItem.TeamID).ToList();
        TeamQueryItem.Games = GameQuery;
        TeamQueryItem.Players = PlayersQuery;

        return TeamQueryItem;
      }
      else
      {
        throw new Exception("Team Not Found in DB");
      }
    }

    public static Player addPlayer(string teamName,string playerName,string position,int playerNumber)
    {
      Team currentTeam = getTeam(teamName);
      if (currentTeam != null)
      {
        if(currentTeam.Players == null)
        {
          currentTeam.Players = new List<Player>();
        }
        var Player = new Player()
        {
          Name = playerName,
          Position = position,
          PlayerNumber = playerNumber,
          TeamId = currentTeam.TeamID
        };
        conn.Insert(Player);
        currentTeam.Players.Add(Player);
        conn.Update(currentTeam);
        return Player;
      }
      throw new Exception("Player not added succesfully");

    }

    public static Team loadTeam()
    {
      var userTeams = conn.Table<Team>().Where(Team => Team.UserID == currentUser.UserID);
      if(userTeams.Count() > 0)
      {
        return userTeams.ToList()[0];
      }
      return null;
    }

    public static GameObject addGame(string teamName, string gameLocation, DateTime gameDate)
    {
      Team currentTeam = getTeam(teamName);
      if (currentTeam != null)
      {
        if (currentTeam.Games == null)
        {
          currentTeam.Games = new List<GameObject>();
        }
        string gameName = "Game " + currentTeam.Games.Count.ToString();
        addGame(gameName, gameName,gameLocation, gameDate);
      }
      throw new Exception("Game not added succesfully");
    }

    public static GameObject addGame(string teamName, string gameName, string gameLocation, DateTime gameDate)
    {
      Team currentTeam = getTeam(teamName);
      if (currentTeam != null)
      {
        if (currentTeam.Games == null)
        {
          currentTeam.Games = new List<GameObject>();
        }
        GameObject game = new GameObject()
        {
          Name = gameName,
          GameDate = gameDate,
          GameLocation = gameLocation,
          HomeScore = 0,
          AwayScore = 0,
          CurrentGameTime = new TimeSpan(0, 12, 0),
          CurrentQuarter = 4,
          CurrentShotClock = new TimeSpan(0, 0, 24),
          BoxScores = new List<BoxScore>(),
          LogActivities = new List<GameLogActivity>(),
          TeamID = currentTeam.TeamID
        };
        for (int i = 0; i < currentTeam.Players.Count; i++)
        {
          BoxScore currentBoxScore = new BoxScore
          {
            player = currentTeam.Players[i],
            PlayerID = currentTeam.Players[i].PlayerID,
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
            Points = 0
          };

          conn.Insert(currentBoxScore);
          game.BoxScores.Add(currentBoxScore);
          conn.Update(game);

        }

        currentTeam.Games.Add(game);
        conn.Update(currentTeam);
      }
      throw new Exception("Game not added succesfully");
    }


    public static GameObject updateGame(GameObject game)
    {
      if (game != null) { 
        conn.Update(game);
        return game;
      }
      throw new Exception("Game not updated succesfully");
    }

    /*Team currentTeam = new Team
    {
      Name = "Chicago Bulls",
      Players = new List<Player>
      {


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
              LogActivities = new List<GameLogActivity>()
            }
          }
    };*/


  }
  }
