using System;

public class Program
{
    public delegate void SimpleDelegate(string message);
    public static void Main(string[] args)
    {
        SimpleDelegate del = PrintMessage;
        del += PrintAnotherMessage;
        del("Hello, Multicast!");
        del -= PrintMessage;
        del("Hello after unsubscribing!");
    }
    public static void PrintMessage(string message)
    {
        Console.WriteLine(message);
    }
    public static void PrintAnotherMessage(string message)
    {
        Console.WriteLine("Another: " + message);
    }
}