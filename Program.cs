namespace HashMessenger
{
    using System;
    using System.Reflection;
    using System.Threading;

    using HashMessenger.Socket;

    class Program
    {
        private static SocketListener Listener { get; set; }
        private static SocketSender Sender { get; set; }

        public static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        [MTAThread]
        static void Main(string[] args)
        {
            Console.WriteLine(  $"HashMessenger {Version}\n"
                              + $"Created by Evervolv1337 - github.com/Evervolv1337\n"
                              + $"-------------------------------------------------");

            Listener = new SocketListener();

            Thread listenThread = new Thread(Listener.Listen);
            listenThread.Start();

            Sender = new SocketSender("localhost");

            Listener.OnMessageRecieve += Listener_OnMessageRecieve;

            try
            {
                SendMessageLoop();
            }
            catch (Exception e)
            {
                Console.WriteLine("[HashMessenger] " + e);
            }

            Listener.Dispose();
            Sender.Dispose();
        }

        private static void SendMessageLoop()
        {
            var message = Console.ReadLine();
            if (message != "exit")
            {
                Sender.SendMessage(message);
                SendMessageLoop();
            }
        }

        private static void Listener_OnMessageRecieve(string message)
        {
            Console.WriteLine("[HashMessenger] " + message);
        }
    }
}
