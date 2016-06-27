namespace HashMessenger.Socket
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Net;
    using System.Net.Sockets;

    class SocketListener : IDisposable
    {
        private string _ip;
        private int _port;

        private Socket Listener { get; set; }

        public delegate void MessageRecieveEvent(string message);
        public event MessageRecieveEvent OnMessageRecieve;

        public SocketListener(string port = "9654", string ip = "localhost")
        {
            this._ip = ip;
            this._port = Int32.Parse(port);
        }

        public void Listen()
        {
            // Устанавливаем для сокета локальную конечную точку
            var ipHost = Dns.GetHostEntry(_ip);
            var ipAddr = ipHost.AddressList[0];
            var ipEndPoint = new IPEndPoint(ipAddr, _port);

            Console.WriteLine("[SocketListener] OnInit -> Created new EndPoint(" + this._ip + ":" + this._port + ")");

            // Создаем сокет Tcp/Ip
            Listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            try
            {
                Listener.Bind(ipEndPoint);
                Listener.Listen(10);

                Console.WriteLine("[SocketListener] OnListenerBind -> Bound");

                // Начинаем слушать соединения
                while (true)
                {
                    Console.WriteLine("[SocketListener] Waiting for connection on port " + this._port);

                    // Программа приостанавливается, ожидая входящее соединение
                    var handler = Listener.Accept();
                    string data = null;

                    // Мы дождались клиента, пытающегося с нами соединиться

                    var bytes = new byte[1024];
                    var bytesRec = handler.Receive(bytes);

                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                    // Показываем данные на консоли
                    //Console.Write("[SocketListener] Recieved data: " + data);

                    OnMessageRecieve.Invoke(data);

                    if (data.IndexOf("<TheEnd>") > -1)
                    {
                        Console.WriteLine("[SocketListener] Connection aborted.");
                        break;
                    }

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[SocketListener] " + ex.Message);
            }
        }

        public void Dispose()
        {
            Listener.Close();

            Console.WriteLine("[SocketListener] Disposed.");
        }
    }
}