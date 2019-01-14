using System.Collections.Generic;
using CastleGrimtol.Project.Interfaces;
using CastleGrimtol.Project.Models;

namespace CastleGrimtol.Project
{
  public class GameService : IGameService
  {
    public IRoom CurrentRoom { get; set; }
    public Player CurrentPlayer { get; set; }
    public bool win = false;
    public bool quit = false;
    public bool playing = true;

    public void GetUserInput()
    {
      System.Console.WriteLine("where do you wanna go " + CurrentPlayer.PlayerName + "??");
      string input = System.Console.ReadLine();
      string[] choiceArr = input.Split(" ");
      string choice = choiceArr[0];
      string value = "";
      if (choiceArr.Length > 1)
      {
        value = choiceArr[1];
      }
      switch (choice)
      {
        case "look":
          Look();
          break;
        case "go":
          Go(value);
          break;
        case "use":
          UseItem(value);
          break;
        case "quit":
          Quit();
          break;
        case "take":
          TakeItem(value);
          break;
        case "inventory":
          Inventory();
          break;
        case "help":
          Help();
          break;
        case "reset":
          Reset();
          break;

        default:
          System.Console.WriteLine("not a valid input");
          break;
      }

    }

    public void Go(string key)
    {
      if (CurrentRoom.Exits.ContainsKey(key) && CurrentRoom.Locked == false)
      {
        CurrentRoom = CurrentRoom.Exits[key];
        Look();
      }
      else if (key == "north" && CurrentRoom.Locked == true)
      {
        System.Console.WriteLine("it's too dark to see");
      }
      else if (key == "south" && CurrentRoom.Locked == true)
      {
        CurrentRoom = CurrentRoom.Exits[key];
        Look();
      }
      else
      {
        System.Console.WriteLine("you can't go that way");
      }
    }

    public void Help()
    {
      System.Console.WriteLine("type look to see your current room");
      System.Console.WriteLine("To move rooms type go followed by a direction like go east");
      System.Console.WriteLine("for help type help");
      System.Console.WriteLine("To quit type quit");
      System.Console.WriteLine("to see your inventory type inventory");
      System.Console.WriteLine("To start over type reset");
      System.Console.WriteLine("To pick up an item in the room type take and then the item name.");
      System.Console.WriteLine("To use an item in your inventory, type use and then the items name");
    }

    public void Inventory()
    {
      System.Console.WriteLine("you have these items in your inventory: ");
      foreach (Item Item in CurrentPlayer.Inventory)
      {
        System.Console.WriteLine(Item.Name);
      }
    }

    public void Look()
    {
      System.Console.WriteLine(CurrentRoom.Description);
    }

    public void Quit()
    {
      playing = false;
    }


    public void Reset()
    {
      System.Console.Clear();
      StartGame();
    }

    public void Setup()
    {
      Room zone3 = new Room("zone3", "this is zone 3.");
      Room zone4 = new Room("Zone4", "this is zone 4");
      Room zone5 = new Room("Zone5", "this is zone 5. there's a torch in here, it could come in handy.");
      Room zone6 = new Room("Zone6", "");
      Room zone7 = new Room("Zone7", "this is zone 7. The room is too dark to see anything right now.");
      Room zone8 = new Room("Zone8", "this is zone 8. You've entered a grassy area and OH MY GOD IT'S A FARCASTER! Throw that baby on the ground and let's go back to camp. ");

      Item Torch = new Item("torch", "a simple torch that will light up a dark zone");
      Item Farcaster = new Item("farcaster", "this item will summon a drake to carry you back to base camp safe and sound");

      zone7.Locked = true;
      CurrentRoom = zone3;
      System.Console.Write("what's your name?");
      string playerName = System.Console.ReadLine();
      CurrentPlayer = new Player(playerName);

      zone5.Items.Add(Torch);
      zone8.Items.Add(Farcaster);

      zone3.Exits.Add("north", zone4);
      zone4.Exits.Add("west", zone5);
      zone4.Exits.Add("east", zone6);
      zone4.Exits.Add("north", zone7);
      zone4.Exits.Add("south", zone3);
      zone5.Exits.Add("east", zone4);
      zone7.Exits.Add("north", zone8);
      zone7.Exits.Add("south", zone4);
      zone8.Exits.Add("south", zone7);


    }




    public void StartGame()
    {
      Setup();
      Look();
      while (playing)
      {
        if (CurrentRoom.Name == "Zone6")
        {
          System.Console.WriteLine("A Rathian is flying around in here and it eats you. you're dead");

          break;
        }
        GetUserInput();
      }
    }

    public void TakeItem(string itemName)
    {
      Item Item = CurrentRoom.Items.Find(i => i.Name == itemName);
      if (Item == null)
      {
        System.Console.WriteLine("item not in room");
      }
      else if (CurrentRoom.Name == "Zone5" && Item.Name == "torch")
      {
        CurrentRoom.Description = "this is zone 5.";
        CurrentRoom.Items.Remove(Item);
        CurrentPlayer.Inventory.Add(Item);
        Inventory();
      }
      else if (CurrentRoom.Name == "Zone8" && Item.Name == "farcaster")
      {
        CurrentRoom.Description = "this is zone 8. come on dude use the farcaster let's get outta here yeah?";
        CurrentRoom.Items.Remove(Item);
        CurrentPlayer.Inventory.Add(Item);
        Inventory();
      }
    }
    public void UseItem(string itemName)
    {
      Item Item = CurrentPlayer.Inventory.Find(item => item.Name == itemName);
      if (Item == null)
      {
        System.Console.WriteLine("Must use item");
      }
      else if (CurrentRoom.Name == "Zone7" && Item.Name == "torch")
      {
        CurrentRoom.Locked = false;
        System.Console.WriteLine("LET THERE BE LIGHT. You can now see an exit to the north.");
        CurrentRoom.Description = "This is zone7. you can now see an exit to the north.";
      }
      else if (CurrentRoom.Name == "Zone8" && Item.Name == "farcaster")
      {
        CurrentRoom.Win = true;
        System.Console.WriteLine("the farcaster teleported you back to base camp. mission accomplished");
        Quit();
      }

    }
  }
}
