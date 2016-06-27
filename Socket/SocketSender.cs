namespace HashMessenger.Socket
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Net;
    using System.Net.Sockets;

    public class SocketSender : IDisposable
    {
        private string _ip;
        private int _port;
        private Socket Sender { get; set; }

        public SocketSender(string ip, string port = "9654")
        {
            this._ip = ip;
            this._port = Int32.Parse(port);

            try
            {
                Init();
            }
            catch (Exception e)
            {
                Console.WriteLine("[SocketSender] " + e.Message);
            }
        }

        private void Init()
        {
            Console.WriteLine("[SocketSender] Binding to " + this._ip + ":" + this._port);

            // Буфер для входящих данных
            byte[] bytes = new byte[1024];

            // Устанавливаем удаленную точку для сокета
            IPHostEntry ipHost = Dns.GetHostEntry(this._ip);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, this._port);

            Sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Соединяем сокет с удаленной точкой
            Sender.Connect(ipEndPoint);
        }

        public void SendMessage(string message)
        {
            if (Sender.RemoteEndPoint == null)
                return;

            Console.WriteLine("[SocketSender] Sending to "+this._ip+":"+this._port);
            byte[] msg = Encoding.UTF8.GetBytes(message);

            // Отправляем данные через сокет
            Sender.Send(msg);

            // Используем рекурсию для неоднократного вызова SendMessageFromSocket()
            if (message.IndexOf("<TheEnd>") == -1)
            {
                Init();
                //SendMessage(message);
            }
        }

        public void Dispose()
        {
            try
            {
                Sender.Shutdown(SocketShutdown.Both);
                Sender.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("[SocketSender] OnDispose -> " + e.Message);
            }

            Console.WriteLine("[SocketSender] Disposed.");
        }
    }
}