using System;
using System.Collections.Generic;
using System.Text;
using BasketballApp.Models;
using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;

namespace BasketballApp.Models
{
  public class Team
  {
    [PrimaryKey, AutoIncrement]
    public int TeamID { get; set; }
    public string Name { get; set; }

    [OneToMany]
    public List<Player> Players { get; set;}
    [OneToMany]
    public List<GameObject> Games { get; set; }

    [ForeignKey(typeof(User))]
    public int UserID { get; set; }
  }
}
