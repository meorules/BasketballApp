using BasketballApp.Services;
using BasketballApp.Models;
using BasketballApp.ViewModels;
using BasketballApp.Views;

namespace UserTests
  {
    [TestClass]
    public class addUser
    {
      [TestMethod]
      public void TestAddUser()
      {
      BasketballDBService.initialise(false);
      User returnedUser = BasketballDBService.addUser("newUser", "", "fhdsj2d");
      if (returnedUser != null)
      {
        Assert.IsNotNull(returnedUser);
        Assert.AreEqual(returnedUser.Name, "newUser");
        Assert.AreEqual(returnedUser.Password, "fhdsj2d");
      }

      }



    }

  [TestClass]
  public class loginUser
  {
    [TestMethod]
    public void SuccesfulTestUserLogin()
    {
      BasketballDBService.initialise(false);
      User returnedUser = BasketballDBService.addUser("newUser", "", "fhdsj2d");
      if (returnedUser != null)
      {
        Assert.IsNotNull(returnedUser);
        Assert.AreEqual(returnedUser.Name, "newUser");
        Assert.AreEqual(returnedUser.Password, "fhdsj2d");

        string result = BasketballDBService.checkUser("newUser", "fhdsj2d");
        Assert.IsNotNull(result);
        Assert.AreEqual(result, "Successful Sign In");

      }

    }

    [TestMethod]

    public void FailUserLogin()
    {
      BasketballDBService.initialise(false);
      User returnedUser = BasketballDBService.addUser("newUser", "", "fhdsj2d");
      if (returnedUser != null)
      {
        Assert.IsNotNull(returnedUser);
        Assert.AreEqual(returnedUser.Name, "newUser");
        Assert.AreEqual(returnedUser.Password, "fhdsj2d");


        string failResult = BasketballDBService.checkUser("newUser", "ewfwefwef");
        Assert.IsNotNull(failResult);
        Assert.AreEqual(failResult, "Incorrect Password");

      }

    }

    [TestMethod]
    public void UsernameNotFound()
    {
      BasketballDBService.initialise(false);
      User returnedUser = BasketballDBService.addUser("newUser", "", "fhdsj2d");
      if (returnedUser != null)
      {
        Assert.IsNotNull(returnedUser);
        Assert.AreEqual(returnedUser.Name, "newUser");
        Assert.AreEqual(returnedUser.Password, "fhdsj2d");

        string wrongUsername = BasketballDBService.checkUser("iuhfirwuhf", "ewfwefwef");
        Assert.IsNotNull(wrongUsername);
        Assert.AreEqual(wrongUsername, "Username Not Found");

      }

    }
  }

}