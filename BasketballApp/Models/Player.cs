using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BasketballApp.Models
{
  public class Player
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Position { get; set; }
    public int PlayerNumber { get; set; }
    public  Image PlayerImage { get; set; }
  }
}
