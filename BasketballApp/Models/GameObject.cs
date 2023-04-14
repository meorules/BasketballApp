using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballApp.Models
{
  public class GameObject
  {
    [PrimaryKey, AutoIncrement]
    public int GameID { get; set; }
    public string Name { get; set; }
    public DateTime GameDate { get; set; }
    public string GameLocation { get; set; }
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }

    public TimeSpan CurrentGameTime { get; set; }

    public TimeSpan CurrentShotClock { get; set; }

    public int CurrentQuarter { get; set; }

    [OneToMany]
    public List<GameLogActivity> LogActivities { get; set; }

    [OneToMany]
    public List<BoxScore> BoxScores { get; set; }

    [ForeignKey(typeof(Team))]
    public int TeamID { get; set; }
  }
}
