namespace HashMessenger.CLI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ConsoleInterface
    {
        static Dictionary<string, Action<IEnumerable<string>>> commands = new Dictionary<string, Action<IEnumerable<string>>>()
        {
            { "clear", RegisterSimpleCommand(Console.Clear) },
            { "messenger", RegisterCommand(typeof(Messenger), "Init") },
        };

        public static int State = 0;

        public static void Init()
        {
            while ("police" != "fucked up")
            {
                Console.Write("HashMessenger.CLI> ");
                string input = Console.ReadLine();

                if (input == "exit")
                {                
                    State = -1;
                    break;
                }

                var tokens = SplitIntoTokens(input);

                var command = tokens.FirstOrDefault();
                if (command == null) continue;

                if (commands.ContainsKey(command))
                {
                    try
                    {
                        commands[command](tokens.Skip(1));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Falied <{command}>: {e.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"Unrecognized command <{command}>");
                }
            }
        }

        static Action<IEnumerable<string>> RegisterSimpleCommand(Action a)
        {
            return args =>
                {
                    if (args.Any()) throw new ArgumentException("this command doesnt have arguments");
                    a();
                };
        }

        static Action<IEnumerable<string>> RegisterCommand(Type type, string method)
        {
            return args =>
                {
                    if (!args.Any()) throw new ArgumentException("this command needs arguments");
                    type.GetMethod(method).Invoke(null, args.ToArray());
                };
        }

        static IEnumerable<string> SplitIntoTokens(string s)
        {
            // тут надо бы что-то похитрее, как минимум учитывать группировку
            // кавычками и escape-символы
            return s.Split(null as char[], StringSplitOptions.RemoveEmptyEntries);
        }
    }
}