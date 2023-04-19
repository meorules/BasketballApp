using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BasketballApp.Models
{
  public class GameObject
  {
    [PrimaryKey, AutoIncrement]
    public int GameID { get; set; }
    public string Name { get; set; }
    public DateTime GameDate { get; set; }

    public string GameDateString
    {
      get { return GameDate.ToString("d-M-yyyy"); }
    }
    public string GameLocation { get; set; }
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }

    public string Score
    {
      get
      {
        return HomeScore.ToString() + " - " + AwayScore.ToString();
      }
    }

    public TimeSpan CurrentGameTime { get; set; }

    public string CurrentGameTimeString
    {
      get { return CurrentGameTime.ToString(@"mm\:ss\.f"); }
    }

    public TimeSpan CurrentShotClock { get; set; }

    public string CurrentShotClockString
    {
      get { return CurrentShotClock.ToString(@"mm\:ss\.f"); }
    }

    public int CurrentQuarter { get; set; }

    public string CurrentQuarterString
    {
      get
      {
        return "Q"+ CurrentQuarter.ToString();
      }
    }

    [OneToMany]
    public List<GameLogActivity> LogActivities { get; set; }

    [OneToMany]
    public List<BoxScore> BoxScores { get; set; }

    [ForeignKey(typeof(Team))]
    public int TeamID { get; set; }
  }
}
