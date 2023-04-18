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

  }
}
