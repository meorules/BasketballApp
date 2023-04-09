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

    public override string ToString() {
      string boxScoreString = " ";
      string playerName = player.Name[0] + "." + player.Name.Substring(player.Name.IndexOf(' ')+1);
      if(playerName.Length >= 8)
      {
        playerName = player.Name.Substring(player.Name.IndexOf(' ')+1);
      }
      if(playerName.Length >= 9) {
        playerName = playerName.Substring(0, 7) + ".";
      }
      string spaceSize = "";
      for(int i= playerName.Length; i < 11; i++)
      {
        spaceSize += " ";
      }
      boxScoreString += playerName + spaceSize + minutesOnCourt.ToString(@"mm\:ss") + "     " + Points.ToString();
      if (Points.ToString().Length == 2)
      {
        boxScoreString += "     " + (ORebs + DRebs).ToString();
      }
      else if(Points.ToString().Length == 1)
      {
        boxScoreString += "      " + (ORebs + DRebs).ToString();
      }

      if ((ORebs + DRebs).ToString().Length == 2)
      {
        boxScoreString += "     " + Assists.ToString();
      }
      else if ((ORebs + DRebs).ToString().Length == 1)
      {
        boxScoreString += "      " + Assists.ToString();
      }

      if (Assists.ToString().Length == 2)
      {
        boxScoreString += "     " + Steals.ToString();
      }
      else if (Assists.ToString().Length == 1)
      {
        boxScoreString += "      " + Steals.ToString();
      }

      if (Steals.ToString().Length == 2)
      {
        boxScoreString += "      " + Blocks.ToString();
      }
      else if (Steals.ToString().Length == 1)
      {
        boxScoreString += "       " + Blocks.ToString();
      }
      if (Blocks.ToString().Length == 2)
      {
        boxScoreString += "    ";
      }
      else if (Blocks.ToString().Length == 1)
      {
        boxScoreString += "     ";
      }

      boxScoreString += FieldGoalsMade.ToString() + "-" + FieldGoalsAttempted.ToString() + "  " + FreeThrowMakes.ToString() + "-" + FreeThrowAttempts.ToString();
      boxScoreString += "   " + ThreesMade.ToString() + "-" + ThreesAttempts.ToString()  + "     " + TurnOvers.ToString() + "       " + PersonalFouls.ToString() ;
      boxScoreString += "    " + PlusMinus.ToString();

      return boxScoreString;
    }

  }

}
 