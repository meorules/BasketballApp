using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace BasketballApp.Models
{
  public class User
  {
    [PrimaryKey, AutoIncrement]
    public int UserID { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    [OneToMany]
    public List<Team> Teams { get; set; }

  }
}
