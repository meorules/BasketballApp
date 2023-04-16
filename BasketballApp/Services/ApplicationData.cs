using System;
using System.Collections.Generic;
using System.Text;


using BasketballApp.Models;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace BasketballApp.Services
{
  public static class ApplicationData
  {
    public static User currentlySignedInUser;
    public static GameObject currentlySelectedGame;
    public static Team currentlySelectedTeam; 
  }
}
