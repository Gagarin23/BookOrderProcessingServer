using System;
using System.Text;
using System.Threading.Channels;

namespace UsefulExtentions
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10000; i++)
            {
                char symbol = (char) i;
                if(!symbol.Equals('?'))
                    Console.Write(symbol);
            }
            Console.ReadLine();
        }
    }
}
