using System;

namespace Motoryzacja.Providers
{
    public class ConsoleInputProvider : IConsoleInputProvider
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}