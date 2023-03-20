using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballApp.Models
{
  public class BoxScore
  {

    public string Id { get; set; }

    public Player player { get; set; }

    public TimeSpan minutesOnCourt { get; set; }

    public int FieldGoalsAttempted { get; set; }

    public int FieldGoalsMade { get; set; }

    public int ThreesAttempts { get; set; }

    public int ThreesMade { get; set; }

    public int FreeThrowAttempts { get; set; }

    public int FreeThrowMakes { get; set; }

    public int ORebs { get; set; }

    public int DRebs { get; set; }

    public int Assists { get; set; }

    public int Steals { get; set; }

    public int Blocks { get; set; }

    public int TurnOvers { get; set; }

    public int PersonalFouls { get; set; }

    public int PlusMinus { get; set; }

    public int Points { get; set; }
  }
}
