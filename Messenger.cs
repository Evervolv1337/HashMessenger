namespace HashMessenger
{
    using System;
    using System.Reflection;
    using System.Threading;

    using HashMessenger.Socket;

    public class Messenger
    {
        private static SocketListener Listener { get; set; }
        private static SocketSender Sender { get; set; }

        public static void Init(string ip, string port)
        {
            Listener = new SocketListener();

            Thread listenThread = new Thread(Listener.Listen);
            listenThread.Start();

            Sender = new SocketSender(ip, port);

            Listener.OnMessageRecieve += Listener_OnMessageRecieve;

            try
            {
                SendMessageLoop();
            }
            catch (Exception e)
            {
                Console.WriteLine("[HashMessenger] " + e.Message);
            }

            //listenThread.Suspend();
            //listenThread.Abort();

            Listener.Dispose();
            Sender.Dispose();
        }

        private static void SendMessageLoop()
        {
            Console.Write("HashMessenger> ");
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