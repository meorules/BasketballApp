using BasketballApp.Services;
using BasketballApp.Models;
using BasketballApp.ViewModels;
using BasketballApp.Views;

namespace TeamTests
  {
    [TestClass]
    public class teamTests
    {
    [TestInitialize] 
    public void Initialize()
    {
      BasketballDBService.initialise(true);
      BasketballDBService.addUser("Meo", "mazen@gmail.com", "test123");
      Team currentTeam = BasketballDBService.addTeam("Meo", "Chicago Bulls", "The United Centre");
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

      [TestMethod]
      public void TestRetrieveBulls()
      {
      Team result = BasketballDBService.getTeam("Chicago Bulls");
      Assert.IsNotNull(result);
      Assert.IsNotNull(result.Players);

      Assert.AreEqual(14, result.Players.Count);

      }

    [TestClass]
    public class testTeamPlayer
    {
      [TestMethod]
      public void TestGetPlayer()
      {
        Team result = BasketballDBService.getTeam("Chicago Bulls");
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Players);

        Assert.AreEqual(14, result.Players.Count);
        Assert.AreEqual(result.Players[0].Name, "Lonzo Ball");
        Assert.AreEqual(result.Players[0].Position, "PG");
        Assert.AreEqual(result.Players[0].PlayerNumber, 2);

      }

      [TestMethod]
      public void TestEditPlayer()
      {
        Team result = BasketballDBService.getTeam("Chicago Bulls");
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Players);

        Assert.AreEqual(14, result.Players.Count);
        Assert.AreEqual(result.Players[0].Name, "Lonzo Ball");
        Assert.AreEqual(result.Players[0].Position, "PG");
        Assert.AreEqual(result.Players[0].PlayerNumber, 2);

        result.Players[0].Name = "Mazen Omar";

        Player playerReturned = BasketballDBService.updatePlayer(result.Players[0]);
        Assert.IsNotNull(playerReturned);
        Assert.AreEqual(playerReturned.Name, "Mazen Omar");

        result.Players[0].Name = "Lonzo Ball";
        BasketballDBService.updatePlayer(result.Players[0]);

      }

      [TestMethod]
      public void TestDeletePlayer()
      {
        Team result = BasketballDBService.getTeam("Chicago Bulls");
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Players);

        bool deletionResult = BasketballDBService.deletePlayer(result.Players[13]);

        Assert.IsTrue(deletionResult);

        Player playerReturned = BasketballDBService.getPlayer(result.Players[13]);
        Assert.IsNull(playerReturned);

        BasketballDBService.addPlayer("Chicago Bulls", "Ayo Dosunmu", "PG", 12);


      }
    }
  }

}