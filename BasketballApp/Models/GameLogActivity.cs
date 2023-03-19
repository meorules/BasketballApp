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

    public float positionX { get; set; }

    public float positionY { get; set; }

  }
}
