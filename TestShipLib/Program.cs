using System;
using System.Threading.Tasks;
using ShipLib;

namespace TestShipLib
{
    class Program
    {
        static void Main(string[] args)
        {
            //test to attack
            OneDeckedShip first = new OneDeckedShip(100, 80, 90, Team.Red);
            OneDeckedShip second = new OneDeckedShip(100, 10, 30, Team.Red);
            OneDeckedShip third = new OneDeckedShip(100, 10, 30, Team.Blue);

            Console.WriteLine($"--load 1: {first.HitPoints} - first hp\n{second.HitPoints} - second hp\n{third.HitPoints} - third hp\n");

            first.Attack(third);
            /*COMPLETED
            //try
            //{
            //    first.Attack(second);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //second.Attack(third);
            //Console.WriteLine($"--load 2: {first.HitPoints} - first hp\n{second.HitPoints} - second hp\n{third.HitPoints} - third hp\n");
            //third.Attack(second);*/

            Console.WriteLine($"--load 3: {first.HitPoints} - first hp\n{second.HitPoints} - second hp\n{third.HitPoints} - third hp\n");
            Console.WriteLine($"{first.ResultLastAttack}");

            //test to dispose
            Console.WriteLine($"--load 4: {first?.ToString()} - first\n {second?.ToString()} - second\n{third?.ToString()} - third\n");
            first.Attack(third);
            // third.Attack(first); //worked!
            Console.WriteLine($"--load 5: {first?.ToString()} - first\n {second?.ToString()} - second\n{third?.ToString()} - third\n");

            //all tests good!
        }
    }
}
