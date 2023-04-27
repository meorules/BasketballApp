using BasketballApp.Services;
using BasketballApp.Models;
using BasketballApp.ViewModels;
using BasketballApp.Views;

namespace DataCollectionTests {

    [TestClass]
    public class testDataCollection
  {
    [TestInitialize]
    public void Initialize()
    {
      BasketballDBService.initialise(true);
      BasketballDBService.addUser("Meo", "mazen@gmail.com", "test123");
      Team currentTeam = BasketballDBService.addTeam("Meo", "Munich Bulls", "The United Centre");
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
    public void addGame()
    {
     GameObject addedGame= BasketballDBService.addGame("Munich Bulls", "Place", DateTime.Today);
      Assert.IsNotNull(addedGame);
      Assert.AreEqual(addedGame.GameDate, DateTime.Today);
      Assert.AreEqual(addedGame.CurrentGameTime, TimeSpan.FromMinutes(12));

      BasketballDBService.deleteGame(addedGame);
    }

    [TestMethod]
    public void collectAndUpdateData()
    {
      GameObject addedGame = BasketballDBService.addGame("Munich Bulls", "Place", DateTime.Today);
      Assert.IsNotNull(addedGame);
      Assert.AreEqual(addedGame.GameDate, DateTime.Today);
      Assert.AreEqual(addedGame.CurrentGameTime, TimeSpan.FromMinutes(12));

      addedGame.CurrentGameTime = TimeSpan.FromMinutes(10);
      addedGame.HomeScore += 5;

      GameObject updatedGame = BasketballDBService.updateGame(addedGame);
      Assert.IsNotNull (updatedGame);
      Assert.AreEqual(updatedGame.CurrentGameTime, TimeSpan.FromMinutes(10));
      Assert.AreEqual(updatedGame.HomeScore, 5);
      BasketballDBService.deleteGame(addedGame);
    }


  }

}
    