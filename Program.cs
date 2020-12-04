using System;
using System.Linq;
using System.Collections.Generic;
using Roommates.Repositories;
using Roommates.Models;

namespace Roomates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);

            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);

            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for chore"):
                        Console.Write("Chore Id: ");
                        int choreId = Int32.Parse(Console.ReadLine());

                        Chore chore = choreRepo.GetById(choreId);
                        Console.WriteLine($"{chore.Id} - {chore.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"):
                        Console.Write("Chore name: ");
                        string choreName = Console.ReadLine();

                        Chore choreToAdd = new Chore()
                        {
                            Name = choreName
                        };

                        choreRepo.Insert(choreToAdd);

                        Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for roommate"):
                        Console.Write("Roommate Id: ");
                        int roommateId = Int32.Parse(Console.ReadLine());

                        Roommate roommate = roommateRepo.GetById(roommateId);
                        Console.WriteLine($"{roommate.Id} - {roommate.Firstname} {roommate.Lastname}\nRoom: {roommate.Room.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all unassigned chores"):
                        List<Chore> unassignedChores = choreRepo.GetUnassignedChores();
                        foreach (Chore c in unassignedChores)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Assign a chore"):
                        AssignChoreToRoommate(choreRepo, roommateRepo);
                        break;
                    case ("Update a room"):
                        UpdateRoom(roomRepo);
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        /// helper method to let the user assign a chore to a roommate
        private static void AssignChoreToRoommate(ChoreRepository choreRepo, RoommateRepository roommateRepo)
        {
            /// list all the chores
            List<Chore> chores = choreRepo.GetAll();
            foreach (Chore c in chores)
            {
                Console.WriteLine($"{c.Id} - {c.Name}");
            }

            /// let the user pick a chore
            int choreIndex = 0;
            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select a chore > ");

                    string input = Console.ReadLine();
                    choreIndex = int.Parse(input) - 1;
                    break;
                }
                catch (Exception)
                {

                    continue;
                }
            }

            Chore selectedChore = chores[choreIndex];

            /// list all the roommates
            List<Roommate> roommates = roommateRepo.GetAll();
            foreach (Roommate r in roommates)
            {
                Console.WriteLine($"{r.Id} - {r.Firstname} {r.Lastname}");
            }

            /// let the user pick a roommate
            int roommateIndex = 0;
            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select a roommate > ");

                    string input = Console.ReadLine();
                    roommateIndex = int.Parse(input) - 1;
                    break;
                }
                catch (Exception)
                {

                    continue;
                }
            }
            Roommate selectedRoommate = roommates[roommateIndex];

            /// assign the chore
            choreRepo.AssignChore(selectedRoommate.Id, selectedChore.Id);

            /// display success message
            Console.WriteLine("The chore has been successfully assigned!");
            Console.Write("Press any key to continue");
            Console.ReadKey();
        }

        private static void UpdateRoom(RoomRepository roomRepo)
        {
            List<Room> roomOptions = roomRepo.GetAll();
            foreach (Room r in roomOptions)
            {
                Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
            }

            Console.Write("Which room would you like to update? ");
            int selectedRoomId = int.Parse(Console.ReadLine());
            Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

            Console.Write("New Name: ");
            selectedRoom.Name = Console.ReadLine();

            Console.Write("New Max Occupancy: ");
            selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

            roomRepo.Update(selectedRoom);

            Console.WriteLine($"Room has been successfully updated");
            Console.Write("Press any key to continue");
            Console.ReadKey();
        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
        {
            "Show all rooms",
            "Search for room",
            "Add a room",
            "Show all chores",
            "Search for chore",
            "Add a chore",
            "Search for roommate",
            "Show all unassigned chores",
            "Assign a chore",
            "Update a room",
            "Exit"
        };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}
