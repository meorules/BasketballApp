using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SQLite;

using BasketballApp.Models;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace BasketballApp.Services
{
  public static class BasketballDBService
  {
    static SQLiteConnection conn;

    public static void initialise(bool reset) {

      if(conn != null)
      {
        return;
      }
      string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyData.db");

      conn = new SQLiteConnection(databasePath);

      if (reset)
      {
        deleteTables();
      }
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

    public static User addUser(string username,string email,string password)
    {
      User lookForExisitingUser = getUser(username);
      if (lookForExisitingUser == null)
      {
        User user = new User()
        {
          Name = username,
          Email = email,
          Password = password,
          Teams = new List<Team>()
        };

        conn.Insert(user);
        return user;
      }
      else
      {
        return null;
      }
    }

    public static User getUser(string username)
    {
      var UserQuery = conn.Table<User>().Where(User => User.Name == username);
      if (UserQuery.Count() > 0)
      {
        var queryList = UserQuery.ToList();
        int userID = queryList[0].UserID;
        var TeamsQuery = conn.Table<Team>().Where(Team => Team.UserID == userID);
        if (TeamsQuery.Count() > 0)
        {
          queryList[0].Teams = TeamsQuery.ToList();
        }

        return queryList[0];
      }
      else
      {
        return null;
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
          ApplicationData.currentlySignedInUser = queryList[0];

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

    public static Team addTeam(string username,string teamName,string location)
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
          Location = location,
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

    public static Team getTeam(int teamID)
    {
      var TeamQuery = conn.Table<Team>().Where(Team => Team.TeamID == teamID);
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
        return null;
        throw new Exception("Team Not Found in DB");
      }
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

        if (TeamQueryItem.Games != null) {
          for (int i = 0; i < TeamQueryItem.Games.Count; i++)
          {
            TeamQueryItem.Games[i] = getGame(TeamQueryItem.Games[i]);
          }
        }
        return TeamQueryItem;
      }
      else
      {
        return null;
        throw new Exception("Team Not Found in DB");
      }
    }

    public static Team updateTeam(Team team)
    {
      if(team != null)
      {
        int rowsUpdated = conn.Update(team);
        if(rowsUpdated == 0)
        {
          return null;
        }
        else return getTeam(team.Name);
      }
      throw new Exception("Team Not Updated Successfully");

    }

    public static bool deleteTeam(Team team)
    {
      if(team != null)
      {
        int rowsDeleted = conn.Delete(team);
        if (rowsDeleted > 0)
        {
          return true;
        }
        else return false;
      }
      else return false;
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

    public static Player getPlayer(int playerID)
    {
      var PlayerQuery = conn.Table<Player>().Where(Player => Player.PlayerID == playerID);
      if (PlayerQuery.Count() > 0)
      {
        return PlayerQuery.ToList()[0];
      }
      else
      {
        return null;
      }

    }

    public static Player getPlayer(string playerName)
    {
      var PlayerQuery = conn.Table<Player>().Where(Player => Player.Name == playerName);
      if (PlayerQuery.Count() > 0)
      {
        return PlayerQuery.ToList()[0];
      }
      else
      {
        return null;
      }
    }


    public static Player getPlayer(Player player)
    {
        return getPlayer(player.PlayerID);
    }

    public static Player updatePlayer(Player toUpdate)
    {
      if (toUpdate != null)
      {
        int rowsUpdated = conn.Update(toUpdate);
        if (rowsUpdated == 0)
        {
          return null;
        }
      }
      return toUpdate;
    }

    public static bool deletePlayer(Player player)
    {
      if(player != null)
      {
        int rowsDeleted = conn.Delete(player);
        if (rowsDeleted > 0)
        {
          return true;
        }
        else
        {
          return false;
        }
      }
      else return false;
    }

    public static Team loadTeam()
    {
      var userTeams = conn.Table<Team>().Where(Team => Team.UserID == ApplicationData.currentlySignedInUser.UserID);
      if(userTeams.Count() > 0)
      {
        var userTeamList = userTeams.ToList();
        for(int i = 0; i < userTeamList.Count; i++)
        {
          int currentTeamID = userTeamList[i].TeamID;
          var teamPlayers = conn.Table<Player>().Where(Player => Player.TeamId == currentTeamID);
          if(teamPlayers.Count() > 0)
          {
            userTeamList[i].Players = teamPlayers.ToList();
          }

        }
        return userTeamList[0];
      }
      return null;
    }

    public static GameObject getGame(int gameID)
    {
      GameObject game = new GameObject
      {
        GameID = gameID,
      };

      return getGame(game);
    }

    public static GameObject getGame(GameObject game)
    {
      GameObject gameObject = conn.Find<GameObject>(game.GameID);

      if (gameObject != null)
      {

        if (gameObject.LogActivities == null)
        {
          gameObject.LogActivities = new List<GameLogActivity>();
        }
        //Get All Game Log Activities
        var GameLogQuery = conn.Table<GameLogActivity>().Where(GameLogActivity => GameLogActivity.GameObjectID == gameObject.GameID).ToList() ;
        if (GameLogQuery.Count > 0)
        {
          for(int i=0; i < GameLogQuery.Count; i++)
          {
            gameObject.LogActivities.Add(GetGameLogActivity(GameLogQuery[i]));

          }
        }

        if (gameObject.BoxScores == null)
        {
          gameObject.BoxScores = new List<BoxScore>();
        }
        // Get all Box Scores
        var BoxScoreQuery = conn.Table<BoxScore>().Where(BoxScore => BoxScore.GameObjectID == gameObject.GameID).ToList();
        if (BoxScoreQuery.Count > 0)
        {
          for (int i = 0; i < BoxScoreQuery.Count; i++)
          {
            gameObject.BoxScores.Add(GetBoxScore(BoxScoreQuery[i]));
          }
        }


        return gameObject;
      }
      
      return null;
      throw new Exception("Game not found");
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
        return addGame(teamName, gameName,gameLocation, gameDate);
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
        conn.Insert(game);
        for (int i = 0; i < currentTeam.Players.Count; i++)
        {
          BoxScore currentBoxScore = new BoxScore
          {
            GameObjectID = game.GameID,
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
        return game;
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


    public static bool deleteGame(GameObject currentlySelectedGame)
    {
      if (currentlySelectedGame != null)
      {
        if (currentlySelectedGame.BoxScores != null)
        {
          for (int i = 0; i < currentlySelectedGame.BoxScores.Count; i++)
          {
            int boxScoreDeleted = conn.Delete(currentlySelectedGame.BoxScores[i].Id);
            if (boxScoreDeleted <=0 )
            {
              return false;
            }
          }
        }
        if(currentlySelectedGame.LogActivities != null)
        {
          for (int i = 0; i < currentlySelectedGame.LogActivities.Count; i++)
          {
            //currentlySelectedGame.LogActivities[i];
            bool deleted = deleteGameLogActivity(currentlySelectedGame.LogActivities[i]);
            if (!deleted)
            {
              return false;
            }
          }
        }
        int gameDeleted = conn.Delete<GameObject>(currentlySelectedGame.GameID);
        if(gameDeleted > 0)
        {
          return true;
        }
        else
        {
          return false;
        }
      }
      return false;
    }


    public static BoxScore updateBoxScore(BoxScore score)
    {
      if (score != null)
      {
        conn.Update(score);
        return score;
      }
      throw new Exception("Box Score not updated succesfully");
    }

    public static GameLogActivity addGameLog(GameLogActivity activity)
    {
      if (activity != null)
      {
        conn.Insert(activity.StatCollected);
        activity.StatID = activity.StatCollected.Id;
        activity.PlayerID = activity.Player.PlayerID;

        conn.Insert(activity); 
        return GetGameLogActivity(activity);
      }
      throw new Exception("Game Log not added succesfully");
    }

    public static GameLogActivity GetGameLogActivity(GameLogActivity activity)
    {
      var foundGamelogActivity = conn.Find<GameLogActivity>(activity.Id);
      if (foundGamelogActivity != null)
      {
        var PlayersQuery = conn.Table<Player>().Where(Player => Player.PlayerID == foundGamelogActivity.PlayerID).ToList()[0];
        var StatQuery = conn.Table<Stat>().Where(Stat => Stat.Id == foundGamelogActivity.StatID).ToList()[0];
        foundGamelogActivity.StatCollected = StatQuery;
        foundGamelogActivity.Player = PlayersQuery;

        return foundGamelogActivity;
      }
      else
      {
        throw new Exception("Game Log Activity Not Found in DB");
      }
    }

    public static bool deleteGameLogActivity(GameLogActivity activity)
    {
      activity = GetGameLogActivity(activity);
      if (activity != null)
      {
        if(activity.StatCollected != null)
        {
          int statDelete = conn.Delete<Stat>(activity.StatID);
          if (statDelete > 0)
          {
            int activityRowsDeleted = conn.Delete<GameLogActivity>(activity.Id);
            if (activityRowsDeleted > 0)
            {
              return true;
            }
            else return false;
          }
          else return false;
        }
        else
        {
          int activityRowsDeleted = conn.Delete<GameLogActivity>(activity.Id);
          if (activityRowsDeleted > 0)
          {
            return true;
          }
          else return false;
        }
      }
      else return false;
    }

    public static BoxScore GetBoxScore(BoxScore boxScore)
    {
      var foundBoxScore = conn.Find<BoxScore>(boxScore.Id);
      if (foundBoxScore != null)
      {
        foundBoxScore.player = conn.Table<Player>().Where(Player => Player.PlayerID == foundBoxScore.PlayerID).ToList()[0];

        return foundBoxScore;
      }
      else
      {
        throw new Exception("Box Score Not Found in DB");
      }
    }

  }
  }
