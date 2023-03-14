using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballApp.Models
{
  public class GameObject
  {
    public string GameID { get; set; }
    public string Name { get; set; }
    public DateTime GameDate { get; set; }
    public string GameLocation { get; set; }
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }

    public TimeSpan CurrentGameTime { get; set; }

    public int CurrentQuarter { get; set; }

    public List<GameLogActivity> LogActivities { get; set; }
  }
}
