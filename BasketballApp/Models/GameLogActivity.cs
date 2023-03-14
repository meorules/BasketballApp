using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballApp.Models
{
  public class GameLogActivity
  {
    
    public string Id { get; set; }
    public Stat StatCollected { get; set; } 
    public Player PlayerStat { get; set; }

    public int Quarter { get; set; }
    public TimeSpan Time { get; set; }

    public int positionX { get; set; }

    public int positionY { get; set; }

  }
}
