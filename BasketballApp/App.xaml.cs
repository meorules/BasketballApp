//using BasketballApp.Services;
using BasketballApp.Services;
using BasketballApp.Models;
using BasketballApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BasketballApp
{
  public partial class App : Application
  {

    public App()
    {
      InitializeComponent();

      BasketballDBService.initialise(false);
      //addSimulatedData();

      MainPage = new AppShell();
    }

    protected override void OnStart()
    {
    }

    protected override void OnSleep()
    {
    }

    protected override void OnResume()
    {
    }

    private void addSimulatedData()
    {
      //Add various users, teams, players, etc for simulation purposes
      BasketballDBService.addUser("Meo", "mazen@gmail.com", "test123");
      Team currentTeam = BasketballDBService.addTeam("Meo", "Chicago Bulls","The United Centre");
      BasketballDBService.addPlayer(currentTeam.Name, "Lonzo Ball", "PG", 2);
      BasketballDBService.addPlayer(currentTeam.Name, "Javonte Green", "SG", 24);
      BasketballDBService.addPlayer(currentTeam.Name, "Dalen Terry", "SG", 25);
      BasketballDBService.addPlayer(currentTeam.Name, "Andre Drummond", "C", 3);
      BasketballDBService.addPlayer(currentTeam.Name, "Derrick Jones Jr.", "SF", 5);
      BasketballDBService.addPlayer(currentTeam.Name, "DeMar DeRozan", "SG", 11);
      BasketballDBService.addPlayer(currentTeam.Name, "Patrick Williams", "PF", 44);
      BasketballDBService.addPlayer(currentTeam.Name, "Nikola Vucevic", "C", 9);
      BasketballDBService.addPlayer(currentTeam.Name, "Terry Taylor", "PF", 32);
      BasketballDBService.addPlayer(currentTeam.Name, "Zach LaVine", "SG", 8);
      BasketballDBService.addPlayer(currentTeam.Name, "Alex Caruso", "PG", 6);
      BasketballDBService.addPlayer(currentTeam.Name, "Patrick Beverley", "PG", 21);
      BasketballDBService.addPlayer(currentTeam.Name, "Coby White", "SG", 0);
      BasketballDBService.addPlayer(currentTeam.Name, "Ayo Dosunmu", "PG", 12);

    }
  }
}
