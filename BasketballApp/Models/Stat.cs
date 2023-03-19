  using System;
  using System.Collections.Generic;
  using System.Text;

  namespace BasketballApp.Models
  {
   public class Stat
    {

      public String StatName { get; set; }
      public int pointWorth { get; set; }

      public TimeSpan gameTime { get; set; }

      public TimeSpan shotClock { get; set; }

      public int Quarter { get; set; }
    }
  }
