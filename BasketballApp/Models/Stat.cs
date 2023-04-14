  using System;
  using System.Collections.Generic;
  using System.Text;
  using SQLite;

  namespace BasketballApp.Models
  {
   public class Stat
    {
      [PrimaryKey, AutoIncrement]
      public int Id { get; set; }
      public String StatName { get; set; }
      public int pointWorth { get; set; }

      public TimeSpan gameTime { get; set; }

      public TimeSpan shotClock { get; set; }

      public int Quarter { get; set; }
    }
  }
