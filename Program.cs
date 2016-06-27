namespace HashMessenger
{
    using System;
    using System.Reflection;
    using System.Threading;

    using HashMessenger.Socket;

    class Program
    {
        public static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        static void Main(string[] args)
        {
            Console.WriteLine($"HashMessenger {Version}\n"
                            + $"Created by Evervolv1337 - github.com/Evervolv1337\n"
                            + $"-------------------------------------------------");

            Messenger.Init();
        }
    }
}
