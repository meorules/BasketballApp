using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace BasketballApp.Models
{
  public class GameLogActivity
  {
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [OneToOne]
    public Stat StatCollected { get; set; }

    [ForeignKey(typeof(Stat))]
    public int StatID { get; set; }


    [ForeignKey(typeof(Player))]
    public int PlayerID { get; set; }

    [ManyToOne]
    public Player Player { get; set; } 

    public float positionX { get; set; }

    public float positionY { get; set; }

    [ForeignKey(typeof(GameObject))]
    public int GameObjectID { get; set; }

  }
}
