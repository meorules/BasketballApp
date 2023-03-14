using System;
using System.Collections.Generic;
using System.Text;
using BasketballApp.Models;

namespace BasketballApp.Models
{
  internal class Team
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Player> Players { get; set;}
    public List<GameObject> Games { get; set; }


  }
}
