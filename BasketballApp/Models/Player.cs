using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xamarin.Forms;

namespace BasketballApp.Models
{
  public class Player
  {
    [PrimaryKey, AutoIncrement]
    public int PlayerID { get; set; }
    public string Name { get; set; }
    public string Position { get; set; }
    public int PlayerNumber { get; set; }

    [ForeignKey(typeof(Team))]
    public int TeamId { get; set; }

    public static  bool operator ==(Player a, Player b)
    {
      // If both are null, or both are same instance, return true.
      if (System.Object.ReferenceEquals(a, b))
      {
        return true;
      }

      // If one is null, but not both, return false.
      if (((object)a == null) || ((object)b == null))
      {
        return false;
      }

      // Return true if the fields match:
      return a.Name == b.Name;
    }

    public override bool Equals(Object obj)
    {
      return obj is Player && this == (Player)obj;
    }

    public static bool operator !=(Player a, Player b)
    {
      // If both are null, or both are same instance, return true.
      if (System.Object.ReferenceEquals(a, b))
      {
        return true;
      }

      // If one is null, but not both, return false.
      if (((object)a == null) || ((object)b == null))
      {
        return false;
      }

      // Return true if the fields do not match:
      return a.Name != b.Name;
    }

  }
}
