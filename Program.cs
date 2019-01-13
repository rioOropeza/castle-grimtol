using System;
using CastleGrimtol.Project;

namespace CastleGrimtol
{
  public class Program
  {
    public static void Main(string[] args)
    {
      System.Console.WriteLine("you were hunting a Rathian and then chickened out so now you just want to go back to base camp so the mean rathian can't hurt you.");
      System.Console.WriteLine("There's a farcaster somewhere around you that you can use to be sent back to base camp but you have to find it.");
      System.Console.WriteLine("find it and try not to let the Rathian eat you ok.");
      GameService GameService = new GameService();
      GameService.StartGame();
    }
  }
}
