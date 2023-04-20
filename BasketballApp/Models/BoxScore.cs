using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace BasketballApp.Models
{
  public class BoxScore
  {
    [PrimaryKey,AutoIncrement]
    public int Id { get; set; }

    [ForeignKey(typeof(GameObject))]
    public int GameObjectID { get; set; }

    [ManyToOne]
    public Player player { get; set; }

    [ForeignKey(typeof(Player))]
    public int PlayerID { get; set; }

    public TimeSpan minutesOnCourt { get; set; }

    public int FieldGoalsAttempted { get; set; }

    public int FieldGoalsMade { get; set; }

    public string FieldGoalPercentage
    {
      get
      {
        if (FieldGoalsAttempted == 0)
        {
          return "100%";
        }
        else
        {
          return Math.Round(100 * ((double)FieldGoalsMade / (double)FieldGoalsAttempted),2).ToString() + "%";
        }
      }
    }

    public int ThreesAttempts { get; set; }

    public int ThreesMade { get; set; }

    public string ThreePercentage
    {
      get
      {
        if (ThreesAttempts == 0)
        {
          return "100%";
        }
        else
        {
          return Math.Round(100 * ((double)ThreesMade / (double)ThreesAttempts),2).ToString() + "%";
        }
      }
    }

    public int FreeThrowAttempts { get; set; }

    public int FreeThrowMakes { get; set; }

    public string FreeThrowPercentage
    {
      get
      {
        if (FreeThrowAttempts == 0)
        {
          return "100%";
        }
        else
        {
          return Math.Round(100 * ((double)FreeThrowMakes / (double)FreeThrowAttempts), 2).ToString() + "%";
        }
      }
    }

    public int ORebs { get; set; }

    public int DRebs { get; set; }

    public int Rebs
    {
      get
      {
        return ORebs + DRebs;
      }
    }

    public int Assists { get; set; }

    public int Steals { get; set; }

    public int Blocks { get; set; }

    public int TurnOvers { get; set; }

    public int PersonalFouls { get; set; }

    public int PlusMinus { get; set; }

    public int Points { get; set; }

    public string toString()
    {
      string boxScoreString = " ";
      boxScoreString += Points.ToString();
      if (Points.ToString().Length == 2)
      {
        boxScoreString += "     " + (ORebs + DRebs).ToString();
      }
      else if (Points.ToString().Length == 1)
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
      boxScoreString += "   " + ThreesMade.ToString() + "-" + ThreesAttempts.ToString() + "     " + TurnOvers.ToString() + "       " + PersonalFouls.ToString();
      boxScoreString += "    " + PlusMinus.ToString();

      return boxScoreString;
    }

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


    public static BoxScore getTotalBoxScore(List<BoxScore> scoreList)
    {
      if (scoreList != null && scoreList.Count > 0)
      {
        BoxScore score = new BoxScore()
        {
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
          Points = 0,

        };
        for (int i = 0; i < scoreList.Count; i++)
        {
          score.Assists += scoreList[i].Assists;
          score.Points += scoreList[i].Points;
          score.PlusMinus += scoreList[i].PlusMinus;
          score.PersonalFouls += scoreList[i].PersonalFouls;
          score.TurnOvers += scoreList[i].TurnOvers;
          score.Blocks += scoreList[i].Blocks;
          score.Steals += scoreList[i].Steals;
          score.DRebs += scoreList[i].DRebs;
          score.ORebs += scoreList[i].ORebs;
          score.FreeThrowMakes += scoreList[i].FreeThrowMakes;
          score.FreeThrowAttempts += scoreList[i].FreeThrowAttempts;
          score.ThreesMade += scoreList[i].ThreesMade;
          score.ThreesAttempts += scoreList[i].ThreesAttempts;
          score.FieldGoalsMade += scoreList[i].FieldGoalsMade;
          score.FieldGoalsAttempted += scoreList[i].FieldGoalsAttempted;
          score.minutesOnCourt += scoreList[i].minutesOnCourt;
        }
        return score;
      }
      else return null;
    }
  }

}
 