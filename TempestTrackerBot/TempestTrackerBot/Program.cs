using System;

namespace TempestTrackerBot
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatListener.Init();
            Console.WriteLine("Listener active. Press q to stop.");
            while (Console.Read() != 'q') ;
        }
    }
}
